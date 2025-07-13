using Microsoft.AspNetCore.Mvc;
using ConcertManagementSystem.Api.Models;
using ConcertManagementSystem.Api.Repositories;
using ConcertManagementSystem.Api.Models.Responses;
using ConcertManagementSystem.Api.Models.Requests;
using System.Net;

namespace ConcertManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _eventRepository;
    private readonly ITicketRepository _ticketRepository;

    public EventsController(IEventRepository eventRepository, ITicketRepository ticketRepository)
    {
        _eventRepository = eventRepository;
        _ticketRepository = ticketRepository;
    }

    /// <summary>
    /// Retrieves all concert events.
    /// </summary>
    /// <returns>List of all events.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(GetAllEventsResponse), (int)HttpStatusCode.OK)]
    public ActionResult<GetAllEventsResponse> GetAll()
    {
        var events = _eventRepository.GetAll();

        var response = new GetAllEventsResponse
        {
            Events = events.ToList()
        };

        return Ok(response);
    }

    /// <summary>
    /// Retrieves a specific event by ID.
    /// </summary>
    /// <param name="id">The ID of the event.</param>
    /// <returns>Event details.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetEventResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public ActionResult<GetEventResponse> GetById(Guid id)
    {
        var concertEvent = _eventRepository.GetById(id);
        if (concertEvent == null)
            return NotFound("Event not found");

        var response = new GetEventResponse
        {
            Id = concertEvent.Id,
            Name = concertEvent.Name,
            Venue = concertEvent.Venue,
            Date = concertEvent.Date,
            Description = concertEvent.Description,
            TotalCapacity = concertEvent.TotalCapacity
        };

        return Ok(response);
    }

    /// <summary>
    /// Creates a new concert event.
    /// </summary>
    /// <param name="request">Event creation details.</param>
    /// <returns>The ID of the newly created event.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateEventResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public ActionResult<CreateEventResponse> Create([FromBody] CreateEventRequest request)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Date = request.Date,
            Venue = request.Venue,
            TotalCapacity = request.TotalCapacity,
            TicketTypes = new List<TicketType>()
        };

        _eventRepository.Add(newEvent);

        var response = new CreateEventResponse { Id = newEvent.Id };
        return CreatedAtAction(nameof(GetById), new { id = newEvent.Id }, response);
    }

    /// <summary>
    /// Updates an existing concert event.
    /// </summary>
    /// <param name="id">ID of the event to update.</param>
    /// <param name="request">Updated event data.</param>
    /// <returns>Confirmation of update.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UpdateEventResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public ActionResult<UpdateEventResponse> Update(Guid id, [FromBody] UpdateEventRequest request)
    {
        var concertEvent = _eventRepository.GetById(id);
        if (concertEvent == null)
            return NotFound("Event not found");

        var totalTypeQuantity = concertEvent.TicketTypes.Sum(tt => tt.TotalQuantity);
        if (request.TotalCapacity < totalTypeQuantity)
            return BadRequest("Total capacity cannot be less than the sum of ticket type total quantities.");


        concertEvent.Name = request.Name;
        concertEvent.Description = request.Description;
        concertEvent.Venue = request.Venue;
        concertEvent.Date = request.Date;
        concertEvent.TotalCapacity = request.TotalCapacity;

        _eventRepository.Update(concertEvent);

        var response = new UpdateEventResponse
        {
            Id = concertEvent.Id
        };

        return Ok(response);
    }

    /// <summary>
    /// Adds a new ticket type to a concert event.
    /// </summary>
    /// <param name="id">ID of the event.</param>
    /// <param name="request">Ticket type details.</param>
    /// <returns>The ID of the created ticket type.</returns>
    [HttpPost("{id}/ticket-types")]
    [ProducesResponseType(typeof(CreateTicketTypeResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public ActionResult<CreateTicketTypeResponse> CreateTicketType(Guid id, [FromBody] CreateTicketTypeRequest request)
    {
        var concertEvent = _eventRepository.GetById(id);
        if (concertEvent == null)
            return NotFound("Event not found");

        var currentTotal = concertEvent.TicketTypes.Sum(t => t.TotalQuantity);
        if (currentTotal + request.TotalQuantity > concertEvent.TotalCapacity)
            return BadRequest("Ticket type exceeds event capacity.");

        var ticketType = new TicketType
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Price = request.Price,
            TotalQuantity = request.TotalQuantity,
            EventId = id
        };

        concertEvent.TicketTypes.Add(ticketType);
        _eventRepository.Update(concertEvent);

        var response = new CreateTicketTypeResponse
        {
            Id = ticketType.Id
        };

        return CreatedAtAction(nameof(GetById), new { id = concertEvent.Id }, response);
    }

    /// <summary>
    /// Retrieves ticket availability for a specific concert event, including per ticket type and overall totals.
    /// </summary>
    /// <param name="id">The ID of the concert event.</param>
    /// <returns>Availability details for each ticket type and aggregated totals.</returns>
    /// <response code="200">Returns the ticket availability information for the event.</response>
    /// <response code="404">Event not found.</response>
    /// <response code="500">Unexpected server error.</response>
    [HttpGet("{id}/ticket-availability")]
    [ProducesResponseType(typeof(EventTicketAvailabilityResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    public ActionResult<EventTicketAvailabilityResponse> GetTicketAvailability(Guid id)
    {
        var concertEvent = _eventRepository.GetById(id);
        if (concertEvent == null)
            return NotFound("Event not found");

        var tickets = _ticketRepository.GetByEventId(id);

        var response = new EventTicketAvailabilityResponse
        {
            EventId = concertEvent.Id,
            EventName = concertEvent.Name,
            TotalCapacity = concertEvent.TotalCapacity,
            TicketTypes = new List<TicketTypeAvailability>()
        };

        foreach (var tt in concertEvent.TicketTypes)
        {
            var reserved = tickets.Count(t => t.TicketTypeId == tt.Id && t.Status == TicketStatus.Reserved);
            var purchased = tickets.Count(t => t.TicketTypeId == tt.Id && t.Status == TicketStatus.Purchased);

            var availability = new TicketTypeAvailability
            {
                TicketTypeId = tt.Id,
                Name = tt.Name,
                Price = tt.Price,
                TotalQuantity = tt.TotalQuantity,
                ReservedCount = reserved,
                PurchasedCount = purchased
            };

            response.TicketTypes.Add(availability);
        }

        return Ok(response);
    }

}

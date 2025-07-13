using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using ConcertManagementSystem.Api.Models;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Repositories;
using ConcertManagementSystem.Api.Services;
using ConcertManagementSystem.Api.Models.Responses;

namespace ConcertManagementSystem.Tests.Services;

public class TicketServiceTests
{
    [Fact]
    public void ReserveTicket_ShouldReturnTicket_WhenAvailable()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var ticketTypeId = Guid.NewGuid();

        var ticketType = new TicketType
        {
            Id = ticketTypeId,
            Name = "VIP",
            Price = 100,
            TotalQuantity = 10,
            EventId = eventId
        };

        var mockEvent = new Event
        {
            Id = eventId,
            Name = "Concert",
            Venue = "Arena",
            Date = DateTime.UtcNow.AddDays(1),
            TotalCapacity = 100,
            TicketTypes = new List<TicketType> { ticketType }
        };

        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(r => r.GetById(eventId)).Returns(mockEvent);

        var mockTicketRepo = new Mock<ITicketRepository>();
        mockTicketRepo.Setup(r => r.GetByEventId(eventId)).Returns(new List<Ticket>());

        var mockPayment = new Mock<IPaymentProcessingService>();

        var service = new TicketService(mockTicketRepo.Object, mockEventRepo.Object, mockPayment.Object);

        var request = new ReserveTicketRequest
        {
            EventId = eventId,
            TicketTypeId = ticketTypeId
        };

        // Act
        var result = service.ReserveTicket(request);

        // Assert
        Assert.IsType<ReserveTicketResponse>(result);
        Assert.NotEqual(Guid.Empty, result.TicketId);
        Assert.NotNull(result.ExpiresAt);
        mockTicketRepo.Verify(r => r.Add(It.Is<Ticket>(t => t.EventId == eventId && t.TicketTypeId == ticketTypeId)), Times.Once);
    }
}

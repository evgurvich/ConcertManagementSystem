using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ConcertManagementSystem.Api.Controllers;
using ConcertManagementSystem.Api.Repositories;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Models.Responses;
using ConcertManagementSystem.Api.Models;
namespace ConcertManagementSystem.Tests.Controllers;

public class EventsControllerTests
{
    [Fact]
    public void Create_ShouldReturnCreatedAtAction_WithValidEventId()
    {
        // Arrange
        var mockEventRepo = new Mock<IEventRepository>();
        var mockTicketRepo = new Mock<ITicketRepository>();
        var controller = new EventsController(mockEventRepo.Object, mockTicketRepo.Object);

        // Act
        var request = new CreateEventRequest
        {
            Name = "Test Concert",
            Date = DateTime.UtcNow.AddDays(1),
            Venue = "Test Venue",
            Description = "Some details",
            TotalCapacity = 1000
        };

        var result = controller.Create(request);
        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<CreateEventResponse>(createdAtActionResult.Value);
        Assert.NotEqual(Guid.Empty, response.Id);
    }

    [Fact]
    public void GetAll_ShouldReturnAllEvents()
    {
        // Arrange
        var mockEventRepo = new Mock<IEventRepository>();
        var mockTicketRepo = new Mock<ITicketRepository>();
        var testEvents = new List<Event> {
          new Event { Id = Guid.NewGuid(), Name = "Event 1" },
          new Event { Id = Guid.NewGuid(), Name = "Event 2" }
       };

        mockEventRepo.Setup(repo => repo.GetAll()).Returns(testEvents);

        var controller = new EventsController(mockEventRepo.Object, mockTicketRepo.Object);

        // Act
        var result = controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<GetAllEventsResponse>(okResult.Value);
        Assert.Equal(2, response.Events.Count);
    }

}

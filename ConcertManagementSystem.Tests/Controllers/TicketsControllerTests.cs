using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ConcertManagementSystem.Api.Controllers;
using ConcertManagementSystem.Api.Repositories;
using ConcertManagementSystem.Api.Models.Requests;
using ConcertManagementSystem.Api.Models.Responses;
using ConcertManagementSystem.Api.Models;
using ConcertManagementSystem.Api.Services;

namespace ConcertManagementSystem.Tests.Controllers;

public class TicketsControllerTests
{
    [Fact]
    public void ReserveTicket_ShouldReturnOk_WhenTicketIsReserved()
    {
        // Arrange
        var request = new ReserveTicketRequest
        {
            EventId = Guid.NewGuid(),
            TicketTypeId = Guid.NewGuid()
        };

        var expectedResponse = new ReserveTicketResponse
        {
            TicketId = Guid.NewGuid(),
            ExpiresAt = DateTime.UtcNow.AddMinutes(15)
        };

        var mockService = new Mock<ITicketService>();
        mockService.Setup(s => s.ReserveTicket(request)).Returns(expectedResponse);

        var controller = new TicketsController(mockService.Object);

        // Act
        var result = controller.ReserveTicket(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualResponse = Assert.IsType<ReserveTicketResponse>(okResult.Value);
        Assert.Equal(expectedResponse.TicketId, actualResponse.TicketId);
    }
}

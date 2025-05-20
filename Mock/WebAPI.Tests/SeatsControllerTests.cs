using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    private Mock<SeatsService> _mockService;
    private Mock<SeatsController> _seatsControllerMock;

    [TestInitialize]
    public void SetUp()
    {
        _mockService = new Mock<SeatsService>();
        _seatsControllerMock = new Mock<SeatsController>(_mockService.Object) { CallBase = true };

    }

    [TestMethod]
    public void ReserveSeatSuccessful()
    {
        string userId = "user1";
        int seatNumber = 10;
        Seat expectedSeat = new Seat { Id = 1, Number = seatNumber, ExamenUserId = userId };
        _mockService.Setup(s => s.ReserveSeat(userId, seatNumber)).Returns(expectedSeat);
        _seatsControllerMock.Setup(c => c.UserId).Returns(userId);

        var actionResult = _seatsControllerMock.Object.ReserveSeat(seatNumber);

        Assert.IsInstanceOfType(actionResult.Result, typeof(OkObjectResult));
        var okResult = actionResult.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(expectedSeat, okResult.Value);
    }


    [TestMethod]
    public void ReserveSeatAlreadyReservedByOther()
    {
        string userId = "user1";
        int seatNumber = 10;
        _mockService.Setup(s => s.ReserveSeat(userId, seatNumber)).Throws(new SeatAlreadyTakenException());
        _seatsControllerMock.Setup(c => c.UserId).Returns(userId);

        var actionResult = _seatsControllerMock.Object.ReserveSeat(seatNumber);

        Assert.IsInstanceOfType(actionResult.Result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void ReserveSeatOutOfBounds()
    {
        string userId = "user1";
        int seatNumber = 101;
        _mockService.Setup(s => s.ReserveSeat(userId, seatNumber)).Throws(new SeatOutOfBoundsException()); 
        _seatsControllerMock.Setup(c => c.UserId).Returns(userId);

        var actionResult = _seatsControllerMock.Object.ReserveSeat(seatNumber);

        Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundObjectResult));
    }


    [TestMethod]
    public void ReserveSeatAlreadySeated()
    {
        string userId = "user1";
        int seatNumber = 10;
        _mockService.Setup(s => s.ReserveSeat(userId, seatNumber)).Throws(new UserAlreadySeatedException());
        _seatsControllerMock.Setup(c => c.UserId).Returns(userId);

        var actionResult = _seatsControllerMock.Object.ReserveSeat(seatNumber);

        Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestResult));
    }

}

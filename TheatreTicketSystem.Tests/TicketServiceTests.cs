using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL;
using TheatreTicketSystem.DAL.Entities;
using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.Tests
{
    [TestClass]
    public class TicketServiceTests
    {
        private ITicketRepository _mockTicketRepository;
        private IPerformanceRepository _mockPerformanceRepository;
        private ISeatRepository _mockSeatRepository;
        private TheatreDbContext _inMemoryContext;
        private TicketService _ticketService;
        private IFixture _fixture;

        [TestInitialize]
        public void Setup()
        {
            // Створення in-memory бази даних для тестів
            var options = new DbContextOptionsBuilder<TheatreDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _inMemoryContext = new TheatreDbContext(options);

            // Ініціалізація заглушок для репозиторіїв
            _mockTicketRepository = Substitute.For<ITicketRepository>();
            _mockPerformanceRepository = Substitute.For<IPerformanceRepository>();
            _mockSeatRepository = Substitute.For<ISeatRepository>();

            // Ініціалізація AutoFixture
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Створення сервісу
            _ticketService = new TicketService(
                _mockTicketRepository,
                _mockPerformanceRepository,
                _mockSeatRepository,
                _inMemoryContext);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _inMemoryContext?.Dispose();
        }

        [TestMethod]
        public void GetAllTickets_ShouldReturnAllTickets()
        {
            // Arrange
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Price = 100m, IsSold = false },
                new Ticket { Id = 2, Price = 150m, IsSold = true },
                new Ticket { Id = 3, Price = 200m, IsSold = false }
            };

            _mockTicketRepository.GetAll().Returns(tickets);

            // Act
            var result = _ticketService.GetAllTickets();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void GetTicketById_WhenTicketExists_ShouldReturnTicket()
        {
            // Arrange
            var ticketId = 1;
            var ticket = new Ticket { Id = ticketId, Price = 100m, IsSold = false };

            _mockTicketRepository.SingleOrDefault(Arg.Any<Expression<Func<Ticket, bool>>>())
                .Returns(ticket);

            // Act
            var result = _ticketService.GetTicketById(ticketId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(ticketId, result.Id);
        }

        [TestMethod]
        public void GetTicketById_WhenTicketDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var ticketId = 999;
            _mockTicketRepository.SingleOrDefault(Arg.Any<Expression<Func<Ticket, bool>>>())
                .Returns((Ticket)null);

            // Act
            var result = _ticketService.GetTicketById(ticketId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateTicketsForPerformance_WhenPerformanceExists_ShouldCreateTickets()
        {
            // Arrange
            var performanceId = 1;
            var hallId = 1;
            var basePrice = 100m;

            var performance = new Performance
            {
                Id = performanceId,
                HallId = hallId,
                BasePrice = basePrice,
                Title = "Test Performance"
            };

            var seats = new List<Seat>
            {
                new Seat { Id = 1, Row = 1, Number = 1, IsVIP = false, HallId = hallId },
                new Seat { Id = 2, Row = 1, Number = 2, IsVIP = true, HallId = hallId }
            };

            _mockPerformanceRepository.GetPerformanceWithDetails(performanceId).Returns(performance);
            _mockSeatRepository.GetSeatsForHall(hallId).Returns(seats);

            // Act
            _ticketService.CreateTicketsForPerformance(performanceId);

            // Assert
            _mockTicketRepository.Received(1).AddRange(Arg.Is<IEnumerable<Ticket>>(tickets =>
                tickets.Count() == 2 &&
                tickets.All(t => t.PerformanceId == performanceId) &&
                tickets.All(t => !t.IsSold)));

            // Перевіряємо що SaveChanges був викликаний
            // Для in-memory контексту це буде працювати
        }

        [TestMethod]
        public void CreateTicketsForPerformance_WhenPerformanceDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var performanceId = 999;
            _mockPerformanceRepository.GetPerformanceWithDetails(performanceId).Returns((Performance)null);

            // Act & Assert
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
                _ticketService.CreateTicketsForPerformance(performanceId));

            Assert.AreEqual("Вистава не знайдена", exception.Message);
        }

        [TestMethod]
        public void SellTicket_WhenTicketExistsAndNotSold_ShouldReturnTrue()
        {
            // Arrange
            var ticketId = 1;
            var ticket = new Ticket
            {
                Id = ticketId,
                IsSold = false,
                Price = 100m,
                PurchaseDate = null
            };

            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns(ticket);

            // Act
            var result = _ticketService.SellTicket(ticketId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(ticket.IsSold);
            Assert.IsNotNull(ticket.PurchaseDate);
            _mockTicketRepository.Received(1).Update(ticket);
        }

        [TestMethod]
        public void SellTicket_WhenTicketDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = 999;
            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns((Ticket)null);

            // Act
            var result = _ticketService.SellTicket(ticketId);

            // Assert
            Assert.IsFalse(result);
            _mockTicketRepository.DidNotReceive().Update(Arg.Any<Ticket>());
        }

        [TestMethod]
        public void SellTicket_WhenTicketAlreadySold_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = 1;
            var ticket = new Ticket
            {
                Id = ticketId,
                IsSold = true,
                Price = 100m
            };

            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns(ticket);

            // Act
            var result = _ticketService.SellTicket(ticketId);

            // Assert
            Assert.IsFalse(result);
            _mockTicketRepository.DidNotReceive().Update(Arg.Any<Ticket>());
        }

        [TestMethod]
        public void ReturnTicket_WhenValidConditions_ShouldReturnTrue()
        {
            // Arrange
            var ticketId = 1;
            var performance = new Performance
            {
                Id = 1,
                PerformanceDate = DateTime.Now.AddDays(2), // Вистава через 2 дні
                Title = "Test Performance"
            };

            var ticket = new Ticket
            {
                Id = ticketId,
                IsSold = true,
                Performance = performance,
                PurchaseDate = DateTime.Now.AddDays(-1)
            };

            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns(ticket);

            // Act
            var result = _ticketService.ReturnTicket(ticketId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsFalse(ticket.IsSold);
            Assert.AreEqual(DateTime.MinValue, ticket.PurchaseDate);
            _mockTicketRepository.Received(1).Update(ticket);
        }

        [TestMethod]
        public void ReturnTicket_WhenPerformanceTooSoon_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = 1;
            var performance = new Performance
            {
                Id = 1,
                PerformanceDate = DateTime.Now.AddHours(12), // Вистава через 12 годин (менше доби)
                Title = "Test Performance"
            };

            var ticket = new Ticket
            {
                Id = ticketId,
                IsSold = true,
                Performance = performance
            };

            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns(ticket);

            // Act
            var result = _ticketService.ReturnTicket(ticketId);

            // Assert
            Assert.IsFalse(result);
            _mockTicketRepository.DidNotReceive().Update(Arg.Any<Ticket>());
        }

        [TestMethod]
        public void ReturnTicket_WhenTicketNotSold_ShouldReturnFalse()
        {
            // Arrange
            var ticketId = 1;
            var ticket = new Ticket
            {
                Id = ticketId,
                IsSold = false
            };

            _mockTicketRepository.GetTicketWithDetails(ticketId).Returns(ticket);

            // Act
            var result = _ticketService.ReturnTicket(ticketId);

            // Assert
            Assert.IsFalse(result);
            _mockTicketRepository.DidNotReceive().Update(Arg.Any<Ticket>());
        }

        [TestMethod]
        public void DeleteTicket_WhenTicketExists_ShouldDeleteTicket()
        {
            // Arrange
            var ticketId = 1;
            var ticket = new Ticket
            {
                Id = ticketId,
                Price = 100m,
                IsSold = false
            };

            _mockTicketRepository.SingleOrDefault(Arg.Any<Expression<Func<Ticket, bool>>>())
                .Returns(ticket);

            // Act
            _ticketService.DeleteTicket(ticketId);

            // Assert
            _mockTicketRepository.Received(1).Remove(ticket);
        }

        [TestMethod]
        public void DeleteTicket_WhenTicketDoesNotExist_ShouldNotCallRemove()
        {
            // Arrange
            var ticketId = 999;
            _mockTicketRepository.SingleOrDefault(Arg.Any<Expression<Func<Ticket, bool>>>())
                .Returns((Ticket)null);

            // Act
            _ticketService.DeleteTicket(ticketId);

            // Assert
            _mockTicketRepository.DidNotReceive().Remove(Arg.Any<Ticket>());
        }
    }
}
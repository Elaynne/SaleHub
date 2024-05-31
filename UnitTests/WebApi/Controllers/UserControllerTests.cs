using Application.UseCases.Users.CreateUser;
using Application.UseCases.Users.RetrieveAllUsers;
using Application.UseCases.Users.RetrieveUserById;
using Application.UseCases.Users.UpdateUser;
using Domain.Enums;
using Domain.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using SalesHub.WebApi.Controllers;

namespace UnitTests.WebApi.Controllers
{
    public class UsersControllerTests
    {
        private readonly IMediator _mediator;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            _controller = new UsersController(_mediator);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext.Items["userRole"] = UserRole.Admin;
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithUsers()
        {
            var users = new List<global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput> { new global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput() };
            _mediator.Send(Arg.Any<RetrieveAllUsersInput>()).Returns(users);

            var result = await _controller.GetAll();

            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(users, okResult.Value);
        }

        [Fact]
        public async Task GetUser_WithValidId_ReturnsOkResult_WithUser()
        {
            var userId = Guid.NewGuid();
            var user = new global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput { Id = userId };
            _mediator.Send(Arg.Any<RetrieveUserByIdInput>()).Returns(user);

            var result = await _controller.GetUser(userId);

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult?.Value.Should().Be(user);
        }

        [Fact]
        public async Task GetUser_WithInvalidId_ReturnsNotFoundResult()
        {
            var userId = Guid.NewGuid();
            _mediator.Send(Arg.Any<RetrieveUserByIdInput>()).Returns<global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput>((global::Application.UseCases.Users.RetrieveUserById.RetrieveUserByIdOutput)null);

            var result = await _controller.GetUser(userId);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateUser_WithUserRoleSellerAndInputRoleClient_ReturnsOkResult()
        {
            var input = new CreateUserInput { Role = UserRole.Client };
            _controller.ControllerContext.HttpContext.Items["userRole"] = UserRole.Seller;

            var result = await _controller.CreateUser(input);

            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task CreateUser_WithUserRoleSellerAndInputRoleAdmin_ReturnsForbidResult()
        {
            var input = new CreateUserInput { Role = UserRole.Admin };
            _controller.ControllerContext.HttpContext.Items["userRole"] = UserRole.Seller;

            var result = await _controller.CreateUser(input);

            result.Result.Should().BeOfType<ForbidResult>();
        }
        [Fact]
        public async Task UpdateUser_WithUserRoleSellerAndInputUserRoleNotClient_ReturnsForbidResult()
        {
            var input = new UpdateUserInput { User = new Domain.Models.User { Role = UserRole.Seller } };
            _controller.ControllerContext.HttpContext.Items["userRole"] = UserRole.Seller;

            var result = await _controller.UpdateUser(input);

            result.Result.Should().BeOfType<ForbidResult>();
        }

        [Fact]
        public async Task UpdateUser_WithValidInput_ReturnsOkObjectResult()
        {
            var input = new UpdateUserInput { User = new Domain.Models.User { Role = UserRole.Client } };
            var updatedUser = new RetrieveUserByIdOutput { Id = Guid.NewGuid(), Role = UserRole.Client };
            _mediator.Send(input).Returns(updatedUser);

            _controller.ControllerContext.HttpContext.Items["userRole"] = UserRole.Admin;

            var result = await _controller.UpdateUser(input);

            result.Result.Should().BeOfType<OkObjectResult>();
        }
    }
}

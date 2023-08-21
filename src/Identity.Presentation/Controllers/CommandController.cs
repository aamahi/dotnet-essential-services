﻿using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Identity.Command")]
    public class CommandController : ControllerBase
    {
        #region Fields

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public CommandController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        #region Token

        [AuthorizationNotRequired]
        [HttpPost(EndpointRoutes.Action_AuthenticateToken)]
        public async Task<ServiceResponse> AuthenticateToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                return new ServiceResponse().BuildBadRequestResponse("Bad request");

            var command = new AuthenticateCommand()
            {
                Email = httpContext.Request.Form["Email"].ToString(),
                Password = httpContext.Request.Form["Password"].ToString(),
            };

            return await _mediator.Send(command);
        }

        [AuthorizationNotRequired]
        [HttpPost(EndpointRoutes.Action_ValidateToken)]
        public async Task<ServiceResponse> ValidateToken(ValidateTokenCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion

        #region User

        [HttpPost(EndpointRoutes.Action_CreateUser)]
        public async Task<ServiceResponse> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateUser)]
        public async Task<ServiceResponse> UpdateUser(UpdateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateUserPassword)]
        public async Task<ServiceResponse> UpdateUserPassword(UpdateUserPasswordCommand command)
        {
            return await _mediator.Send(command);
        }

        [AuthorizationNotRequired]
        [HttpPost(EndpointRoutes.Action_SubmitUser)]
        public async Task<ServiceResponse> SubmitUser(SubmitUserCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion

        #region Role

        [HttpPost(EndpointRoutes.Action_AddRole)]
        public async Task<ServiceResponse> AddRole(AddRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateRole)]
        public async Task<ServiceResponse> UpdateRole(UpdateRoleCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateUserRoles)]
        public async Task<ServiceResponse> UpdateUserRoles(UpdateUserRolesCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion

        #region AccountActivationRequest

        [HttpPost(EndpointRoutes.Action_SendUserAccountActivationRequest)]
        public async Task<ServiceResponse> SendUserAccountActivationRequest(SendUserAccountActivationRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost(EndpointRoutes.Action_VerifyUserAccountActivationRequest)]
        public async Task<ServiceResponse> VerifyUserAccountActivationRequest(VerifyUserAccountActivationRequestCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion

        #region Claim

        [HttpPost(EndpointRoutes.Action_AddClaimPermission)]
        public async Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion

        #endregion
    }
}

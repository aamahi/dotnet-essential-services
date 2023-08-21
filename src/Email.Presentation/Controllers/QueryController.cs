﻿using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Email.Application.Queries;
using Email.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Email.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Email.Query")]
    public class QueryController : ControllerBase
    {
        #region Fields

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public QueryController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        [HttpGet(EndpointRoutes.Action_GetEmailTemplate)]
        public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate([FromQuery] GetEmailTemplateQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetEmailTemplateByPurpose)]
        public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplateByPurpose([FromQuery] GetEmailTemplateByPurposeQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}

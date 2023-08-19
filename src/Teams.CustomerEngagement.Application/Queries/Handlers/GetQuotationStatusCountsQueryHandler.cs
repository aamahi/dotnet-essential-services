﻿using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.CustomerEngagement.Application.Queries.Validators;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;
using Base.Application.Extensions;
using Teams.CustomerEngagement.Application.DTOs.Responses;

namespace Teams.CustomerEngagement.Application.Queries.Handlers
{
    public class GetQuotationStatusCountsQueryHandler : IRequestHandler<GetQuotationStatusCountsQuery, QueryRecordsResponse<QuotationStatusCount>>
    {

        #region Fields

        private readonly ILogger<GetQuotationStatusCountsQueryHandler> _logger;
        private readonly GetQuotationStatusCountsQueryValidator _validator;
        private readonly IQuotationRepository _QuotationRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctors

        public GetQuotationStatusCountsQueryHandler(
            ILogger<GetQuotationStatusCountsQueryHandler> logger,
            GetQuotationStatusCountsQueryValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            IQuotationRepository QuotationRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _QuotationRepository = QuotationRepository;
        }

        #endregion

        #region Methods

        public async Task<QueryRecordsResponse<QuotationStatusCount>> Handle(GetQuotationStatusCountsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _QuotationRepository.GetQuotationStatusCounts();

                var records = result.Records.Select(x => QuotationStatusCount.Map(x)).ToList();
                records.Add(new QuotationStatusCount() { QuoteStatus = "Total", Count = records.Sum(x => x.Count) });

                return Response.BuildQueryRecordsResponse<QuotationStatusCount>().BuildSuccessResponse(count: result.Count, records: records.ToArray(), authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<QuotationStatusCount>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}

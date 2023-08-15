﻿using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Providers.Interfaces;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using MongoDB.Driver;

namespace IdentityModule.Domain.Repositories.Implementations
{
    public class ClaimPermissionRepository : IClaimPermissionRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContext;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Ctor

        public ClaimPermissionRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContext, IRoleRepository roleRepository)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
            _roleRepository = roleRepository;
        }
        #endregion

        #region Methods

        public async Task<bool> BeAnExistingClaimPermission(string claim)
        {
            var filter = Builders<ClaimPermission>.Filter.Where(x => x.Name.ToLower() == claim.ToLower());

            return await _mongoDbService.Exists(filter);
        }

        public async Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds)
        {
            var filter = Builders<RoleClaimPermissionMap>.Filter.In(x => x.RoleId, roleIds);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<RoleClaimPermissionMap>();
        }

        public async Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var claimPermission = ClaimPermission.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(claimPermission);

            return Response.BuildServiceResponse().BuildSuccessResponse(claimPermission, authCtx?.RequestUri);
        }

        public async Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames)
        {
            var filter = Builders<ClaimPermission>.Filter.In(x => x.Name, claimNames);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<ClaimPermission>();
        }

        public async Task<QueryRecordsResponse<ClaimPermission>> GetClaims(GetClaimsQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<ClaimPermission>.Filter.Empty;

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var clams = await _mongoDbService.GetDocuments(filter: filter);

            return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildSuccessResponse(
               count: count,
               records: clams is not null ? clams.ToArray() : Array.Empty<ClaimPermission>(), requestUri: authCtx?.RequestUri);
        }

        public async Task<ClaimPermission[]> GetUserClaims(string userId)
        {
            // find user roles, then from roles, find claims, then from claims assign in token

            var roles = await _roleRepository.GetUserRoles(userId);

            var roleIds = roles.Select(r => r.RoleId).Distinct().ToArray();

            var roleClaimMaps = await GetClaimsForRoleIds(roleIds);

            var claims = await GetClaimsForClaimNames(roleClaimMaps.Select(x => x.ClaimPermission).Distinct().ToArray());

            return claims;
        }

        #endregion
    }
}

﻿using IdentityCore.Extensions;

namespace IdentityCore
{
    public static class EndpointRoutes
    {
        public const string Action_GetUser = "api/Query/GetUser";
        public const string Action_GetUsers = "api/Query/GetUsers";

        public const string Action_CreateUser = "api/Command/CreateUser";
        public const string Action_AddRole = "api/Command/AddRole";
        public const string Action_UpdateRole = "api/Command/UpdateRole";

        public const string Action_AddClaimPermission = "api/Command/AddClaimPermission";

        public const string Action_Validate = "api/Command/Validate";
        public const string Action_Authenticate = "api/Command/Authenticate";

        public static string[] GetEndpointRoutes()
        {
            var endpoints = ClassExtensions.GetConstants(typeof(EndpointRoutes)).Select(x => x.GetValue(x.Name).ToString().ToLower()).ToArray();

            return endpoints;
        }
    }
}

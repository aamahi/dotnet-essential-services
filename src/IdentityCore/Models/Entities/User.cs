﻿using BaseCore.Models.Entities;
using IdentityCore.Declarations.Commands;
using BaseCore.Extensions;

namespace IdentityCore.Models.Entities
{
    public class User: UserBase
    {
        public static User Initialize(CreateUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                DisplayName = (command.FirstName + " " + command.LastName).Trim(),
                ProfileImageUrl = command.ProfileImageUrl,
                PhoneNumber = command.PhoneNumber,
                Email = command.Email,
                Password = command.Password.Encrypt(),
                Address = command.Address,
                MetaTags = command.MetaTags,
                UserStatus = UserStatus.Inactive,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = command.TenantId,
            };

            return user;
        }

        public static User Initialize(SubmitUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {               
                Email = command.Email,              
                MetaTags = command.MetaTags,
                UserStatus = UserStatus.Inactive,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = command.TenantId,
            };

            return user;
        }
    }
}

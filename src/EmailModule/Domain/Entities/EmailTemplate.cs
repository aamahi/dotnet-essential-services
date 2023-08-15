﻿using BaseModule.Domain.Entities;
using EmailModule.Application.Commands;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;

namespace EmailModule.Domain.Entities
{
    public class EmailTemplate : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailBodyContentType EmailBodyContentType { get; set; } = EmailBodyContentType.Text;

        public string[] Tags { get; set; } = new string[] { };

        public string Purpose { get; set; } = string.Empty;
    }
}

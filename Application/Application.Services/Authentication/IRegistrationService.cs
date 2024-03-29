﻿using Application.Domain;
using Sonya.AspNetCore.Common;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public interface IRegistrationService
    {
        Task<ServiceResponse<ApplicationUser>> RegisterAccount(string email, string firstName, string lastName);
    }
}

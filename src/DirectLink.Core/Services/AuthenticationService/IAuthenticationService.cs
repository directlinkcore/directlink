// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DirectLinkCore
{
    public interface IAuthenticationService
    {
        Task<SignInResult> LoginAsync(HubCallerContext context, string login, string password);

        Task LogoutAsync(HubCallerContext context);

        Task SetPrincipalAsync(string connectionId, ClaimsPrincipal user);

        Task<ClaimsPrincipal> GetPrincipalAsync(string connectionId);
    }
}
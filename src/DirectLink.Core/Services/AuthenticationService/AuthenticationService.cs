// Copyright (c) 2018 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace DirectLinkCore
{
    public class AuthenticationService<TUser, TRole> : IAuthenticationService where TUser : class where TRole : class
    {
        private readonly UserClaimsPrincipalFactory<TUser, TRole> _claimsPrincipalFactory;
        private readonly UserManager<TUser> _userManager;
        private readonly SignInManager<TUser> _signInManager;
        private readonly static ConcurrentDictionary<string, ClaimsPrincipal> _users = new ConcurrentDictionary<string, ClaimsPrincipal>();
        private readonly static ClaimsPrincipal _guest = new ClaimsPrincipal(new ClaimsIdentity());

        public AuthenticationService(IUserClaimsPrincipalFactory<TUser> claimsPrincipalFactory, UserManager<TUser> userManager, SignInManager<TUser> signInManager)
        {
            _claimsPrincipalFactory = claimsPrincipalFactory as UserClaimsPrincipalFactory<TUser, TRole>;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(HubCallerContext context, string login, string password)
        {
            var user = await _userManager.FindByNameAsync(login);
            if (user == null) {
                user = await _userManager.FindByEmailAsync(login);
            }
            if (user == null) {
                return SignInResult.Failed;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (result == SignInResult.Success) {
                var principal = await _claimsPrincipalFactory.CreateAsync(user);
                _users[context.ConnectionId] = principal;
                SetUser(context, principal);
            }
            return result;
        }

        public Task LogoutAsync(HubCallerContext context)
        {
            _users.TryRemove(context.ConnectionId, out var _);
            SetUser(context, _guest);
            return Task.CompletedTask;
        }

        public Task SetPrincipalAsync(string connectionId, ClaimsPrincipal user)
        {
            _users[connectionId] = user;
            return Task.CompletedTask;
        }

        public Task<ClaimsPrincipal> GetPrincipalAsync(string connectionId)
        {
            _users.TryGetValue(connectionId, out var principal);
            return Task.FromResult(principal ?? _guest);
        }

        private static void SetUser(HubCallerContext context, ClaimsPrincipal user)
        {
            var httpContext = context.GetHttpContext();
            var authenticationFeature = httpContext.Features.Get<IHttpAuthenticationFeature>();
            authenticationFeature.User = user;
        }
    }
}
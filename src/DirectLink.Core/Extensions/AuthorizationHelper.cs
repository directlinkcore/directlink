// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DirectLinkCore
{
    internal static class AuthorizationHelper
    {
        public static async Task<bool> AuthorizeAsync(Type type, IPrincipal user, IAuthorizationService service)
        {
            return await AuthorizeAsync(type.GetTypeInfo().GetCustomAttributes<AuthorizeAttribute>(), user, service);
        }

        public static async Task<bool> AuthorizeAsync(MethodInfo methodInfo, IPrincipal user, IAuthorizationService service)
        {
            return await AuthorizeAsync(methodInfo.GetCustomAttributes<AuthorizeAttribute>(), user, service);
        }

        private static async Task<bool> AuthorizeAsync(IEnumerable<AuthorizeAttribute> authorizeAttributes, IPrincipal user, IAuthorizationService service)
        {
            foreach (var attr in authorizeAttributes) {
                if (user == null) {
                    return false;
                }
                if (!string.IsNullOrWhiteSpace(attr.Policy)) {
                    var result = await service.AuthorizeAsync(user as ClaimsPrincipal, null, attr.Policy);
                    if (!result.Succeeded) {
                        return false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(attr.Roles)) {
                    var roles = attr.Roles.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (!roles.Any(user.IsInRole)) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
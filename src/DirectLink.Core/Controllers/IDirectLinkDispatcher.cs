// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectLinkCore
{
    public interface IDirectLinkDispatcher<out T> : IDirectLink<T> where T : ViewModel
    {
        ConcurrentDictionary<string, object> Props { get; }

        ConcurrentDictionary<string, HashSet<string>> Connections { get; }


        Task OnConnectedAsync(string connectionId);

        Task OnDisconnectedAsync(string connectionId);


        Task SetStateAsync(string connectionId, string fullname, object state);

        Task SetStateAsync(string group, object state);

        Task SetStateAsync(object state);


        Task InvokeAsync(string connectionId, string fullname, string method, object[] args = null);

        Task InvokeAsync(string group, string method, object[] args = null);

        Task InvokeAsync(string method, object[] args = null);
    }
}
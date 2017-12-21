// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectLinkCore
{
    public class DirectLinkDispatcher<T> : IDirectLinkDispatcher<T>, IInternalDirectLink<T> where T : ViewModel
    {
        private static ConcurrentDictionary<string, object> _props = new ConcurrentDictionary<string, object>();

        private static ConcurrentDictionary<string, HashSet<string>> _connections = new ConcurrentDictionary<string, HashSet<string>>();


        DirectLinkType IInternalDirectLink<T>.LinkType { get; } = DirectLinkType.Dispatcher;

        Type IInternalDirectLink<T>.ViewModelType { get; } = typeof(T);

        public ConcurrentDictionary<string, object> Props { get; } = _props;

        public ConcurrentDictionary<string, HashSet<string>> Connections { get; } = _connections;


        public virtual Task OnConnectedAsync(string connectionId)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisconnectedAsync(string connectionId)
        {
            return Task.CompletedTask;
        }


        public Task SetStateAsync(string connectionId, string fullname, object state)
        {
            return DirectLinkHub.HubContext
                .Clients.Client(connectionId).InvokeAsync(nameof(IDirectLinkHub.SetState), new object[] { fullname, state });
        }

        public Task SetStateAsync(string group, object state)
        {
            return DirectLinkHub.HubContext
                .Clients.Group(group).InvokeAsync(nameof(IDirectLinkHub.SetState), new object[] { group, state });
        }

        public async Task SetStateAsync(object state)
        {
            foreach (var group in this.Connections.Keys) {
                await DirectLinkHub.HubContext
                    .Clients.Group(group).InvokeAsync(nameof(IDirectLinkHub.SetState), new object[] { group, state });
            }
        }


        public Task InvokeAsync(string connectionId, string fullname, string method, object[] args = null)
        {
            return DirectLinkHub.HubContext
                .Clients.Client(connectionId).InvokeAsync(nameof(IDirectLinkHub.Invoke), new object[] { fullname, method, args });
        }

        public Task InvokeAsync(string group, string method, object[] args = null)
        {
            return DirectLinkHub.HubContext
                .Clients.Group(group).InvokeAsync(nameof(IDirectLinkHub.Invoke), new object[] { group, method, args });
        }

        public async Task InvokeAsync(string method, object[] args = null)
        {
            foreach (var group in this.Connections.Keys) {
                await DirectLinkHub.HubContext
                    .Clients.Group(group).InvokeAsync(nameof(IDirectLinkHub.Invoke), new object[] { group, method, args });
            }
        }
    }
}
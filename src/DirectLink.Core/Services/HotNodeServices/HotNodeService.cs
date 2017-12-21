// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices.HostingModels;

namespace DirectLinkCore
{
    internal class HotNodeService : IHotNodeService
    {
        private readonly INodeInstance _nodeInstance;
        private readonly Process _nodeProcess;

        public HotNodeService(INodeInstance nodeInstance)
        {
            _nodeInstance = nodeInstance;
            _nodeProcess = (Process)typeof(OutOfProcessNodeInstance)
                .GetField("_nodeProcess", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(_nodeInstance);
        }

        public long GetUsedMemory()
        {
            if (_nodeProcess.HasExited) {
                return 0;
            }
            _nodeProcess.Refresh();
            return _nodeProcess.WorkingSet64;
        }

        public Task<T> InvokeExportAsync<T>(CancellationToken cancellationToken, string moduleName, string exportNameOrNull, params object[] args)
        {
            return _nodeInstance.InvokeExportAsync<T>(cancellationToken, moduleName, exportNameOrNull, args);
        }

        public Task<T> InvokeExportAsync<T>(CancellationToken cancellationToken, StringContent payload)
        {
            return _nodeInstance.InvokeExportAsync<T>(cancellationToken, null, null, new object[] { payload });
        }

        public void Dispose()
        {
            _nodeInstance.Dispose();
        }
    }
}
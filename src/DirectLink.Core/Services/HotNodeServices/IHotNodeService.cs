// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DirectLinkCore
{
    public interface IHotNodeService
    {
        long GetUsedMemory();

        Task<T> InvokeExportAsync<T>(CancellationToken cancellationToken, string moduleName, string exportNameOrNull, params object[] args);

        Task<T> InvokeExportAsync<T>(CancellationToken cancellationToken, StringContent payload);

        void Dispose();
    }
}
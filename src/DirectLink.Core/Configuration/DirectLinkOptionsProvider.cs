// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    internal class DirectLinkOptionsProvider
    {
        public DirectLinkOptions Options { get; }

        public DirectLinkOptionsProvider(DirectLinkOptions options)
        {
            this.Options = options;
        }
    }
}
// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace DirectLinkCore
{
    public interface IDirectLinkData
    {
        string App { get; }
        string Title { get; }
        IDictionary<string, object> States { get; }
        IDictionary<string, string> Scripts { get; }
        IDictionary<string, string> Styles { get; }
        IDictionary<string, IList<string>> Methods { get; }
        ICollection<string> Bidirectional { get; }
        string Message { get; }
        bool IsRouted { get; }
        bool IsAuthorized { get; }
    }
}
// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace DirectLinkCore
{
    public class DirectLinkOptions
    {
        public Type EntryType { get; set; }
        public string NotFoundPath { get; set; }
        public string NotAuthPath { get; set; }
        public string ErrorPath { get; set; }

        public string WarmupPath { get; set; }
        public string IndexTemplatePath { get; set; }
        public string AssetsPath { get; set; }
        public Action<string, ConcurrentDictionary<string, string>> AssetsParser { get; set; }

        public Func<Type, string> ComponentNameConverter { get; set; }
        public Func<string, string> FileNameConverter { get; set; }
        public string ScriptExtension { get; set; }
        public string StyleExtension { get; set; }

        public bool HotModuleReplacement { get; set; }

        public NodeServicesOptions NodeServicesOptions { get; set; }
        public int NodeInstanceCount { get; set; }
        public long NodeInstanceMemoryLimit { get; set; }
        public TimeSpan MemoryWatcherInterval { get; set; }
        public TimeSpan FileWatcherDelay { get; set; }
        public TimeSpan NodeServiceDisposingDelay { get; set; }

        public ILogger DirectLinkOutputLogger { get; set; }

        public int RequestsPerSecond { get; set; }
        public int QueuedRequestCount { get; set; }
        public int MaxLinkCount { get; set; }

        internal List<string> PathBases { get; } = new List<string>();
    }
}
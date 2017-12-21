// Copyright (c) .NET Foundation. All rights reserved.
// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.NodeServices.HostingModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DirectLinkCore
{
    /// <summary>
    /// A specialisation of the OutOfProcessNodeInstance base class that uses HTTP to perform RPC invocations.
    ///
    /// The Node child process starts an HTTP listener on an arbitrary available port (except where a nonzero
    /// port number is specified as a constructor parameter), and signals which port was selected using the same
    /// input/output-based mechanism that the base class uses to determine when the child process is ready to
    /// accept RPC invocations.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.NodeServices.HostingModels.OutOfProcessNodeInstance" />
    internal class HttpNodeInstance : OutOfProcessNodeInstance
    {
        private static readonly Regex PortMessageRegex =
            new Regex(@"^\[Microsoft.AspNetCore.NodeServices.HttpNodeHost:Listening on port (\d+)\]$");

        private readonly HttpClient _client;
        private bool _disposed;
        private int _portNumber;

        public HttpNodeInstance(string projectPath, string[] watchFileExtensions, CancellationToken applicationStoppingToken, ILogger nodeOutputLogger,
            IDictionary<string, string> environmentVars, int invocationTimeoutMilliseconds, bool launchWithDebugging,
            int debuggingPort, int port = 0)
            : base(EmbeddedResourceReader.Read(typeof(OutOfProcessNodeInstance), "/Content/Node/entrypoint-http.js"),
                projectPath,
                watchFileExtensions,
                MakeCommandLineOptions(port),
                applicationStoppingToken,
                nodeOutputLogger,
                environmentVars,
                invocationTimeoutMilliseconds,
                launchWithDebugging,
                debuggingPort)
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromMilliseconds(invocationTimeoutMilliseconds + 1000);
        }

        private static string MakeCommandLineOptions(int port)
        {
            return $"--port {port}";
        }

        protected override async Task<T> InvokeExportAsync<T>(NodeInvocationInfo invocationInfo, CancellationToken cancellationToken)
        {
            StringContent payload;
            if (invocationInfo.Args.Length == 1 && invocationInfo.Args[0].GetType() == typeof(StringContent)) {
                payload = (StringContent)invocationInfo.Args[0];
            }
            else {
                var payloadJson = JsonConvert.SerializeObject(new NodeInvocationInfoLower(invocationInfo));
                payload = new StringContent(payloadJson, Encoding.UTF8, "application/json");
            }
            var response = await _client.PostAsync("http://localhost:" + _portNumber, payload, cancellationToken);

            if (!response.IsSuccessStatusCode) {
                // Unfortunately there's no true way to cancel ReadAsStringAsync calls, hence AbandonIfCancelled
                var responseErrorString = await response.Content.ReadAsStringAsync().OrThrowOnCancellation(cancellationToken);
                throw new Exception("Call to Node module failed with error: " + responseErrorString);
            }

            var responseContentType = response.Content.Headers.ContentType;
            switch (responseContentType.MediaType) {
                case "text/plain":
                    // String responses can skip JSON encoding/decoding
                    if (typeof(T) != typeof(string)) {
                        throw new ArgumentException(
                            "Node module responded with non-JSON string. This cannot be converted to the requested generic type: " +
                            typeof(T).FullName);
                    }

                    var responseString = await response.Content.ReadAsStringAsync().OrThrowOnCancellation(cancellationToken);
                    return (T)(object)responseString;

                case "application/json":
                    var responseJson = await response.Content.ReadAsStringAsync().OrThrowOnCancellation(cancellationToken);
                    return JsonConvert.DeserializeObject<T>(responseJson);

                case "application/octet-stream":
                    // Streamed responses have to be received as System.IO.Stream instances
                    if (typeof(T) != typeof(Stream)) {
                        throw new ArgumentException(
                            "Node module responded with binary stream. This cannot be converted to the requested generic type: " +
                            typeof(T).FullName + ". Instead you must use the generic type System.IO.Stream.");
                    }

                    return (T)(object)(await response.Content.ReadAsStreamAsync().OrThrowOnCancellation(cancellationToken));

                default:
                    throw new InvalidOperationException("Unexpected response content type: " + responseContentType.MediaType);
            }
        }

        protected override void OnOutputDataReceived(string outputData)
        {
            // Watch for "port selected" messages, and when observed, store the port number
            // so we can use it when making HTTP requests. The child process will always send
            // one of these messages before it sends a "ready for connections" message.
            var match = _portNumber != 0 ? null : PortMessageRegex.Match(outputData);
            if (match != null && match.Success) {
                _portNumber = int.Parse(match.Groups[1].Captures[0].Value);
            }
            else {
                base.OnOutputDataReceived(outputData);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!_disposed) {
                if (disposing) {
                    _client.Dispose();
                }

                _disposed = true;
            }
        }
    }

    internal static class TaskExtensions
    {
        public static Task<T> OrThrowOnCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            return task.IsCompleted
                ? task // If the task is already completed, no need to wrap it in a further layer of task
                : task.ContinueWith(
                    t => t.Result, // If the task completes, pass through its result
                    cancellationToken,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Default);
        }
    }

    internal class NodeInvocationInfoLower
    {
        public NodeInvocationInfoLower(NodeInvocationInfo nodeInvocationInfo)
        {
            ModuleName = nodeInvocationInfo.ModuleName;
            ExportedFunctionName = nodeInvocationInfo.ExportedFunctionName;
            Args = nodeInvocationInfo.Args;
        }

        [JsonProperty(PropertyName = "moduleName")]
        public string ModuleName { get; set; }

        [JsonProperty(PropertyName = "exportedFunctionName")]
        public string ExportedFunctionName { get; set; }

        [JsonProperty(PropertyName = "args")]
        public object[] Args { get; set; }
    }
}
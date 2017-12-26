// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DirectLinkCore
{
    public class Route
    {
        private static readonly Regex ParamsRegex = new Regex("{.+?}", RegexOptions.Compiled);


        public int Priority { get; private set; }

        public string Name { get; }

        public Type Type { get; }

        public string Template { get; }

        public List<RouteParameter> Parameters { get; private set; }

        public object[] Args { get; private set; }

        public Dictionary<string, object> State { get; private set; }

        public Regex Regex { get; private set; }

        public bool Default { get; }


        public Route(string name, string template, object args, bool @default)
        {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(template)) {
                throw new ArgumentNullException(nameof(template));
            }

            this.Name = name;
            this.Type = null;
            this.Template = template;
            this.Default = @default;

            ParseArgs(args);
            ParseTemplateParameters(template);
            SetState(args);
        }

        public Route(Type type, string template, object args, bool @default)
        {
            if (!typeof(IDirectLink<ViewModel>).IsAssignableFrom(type)) {
                throw new ArgumentException("Incorrect type", nameof(type));
            }

            if (string.IsNullOrWhiteSpace(template)) {
                throw new ArgumentNullException(nameof(template));
            }

            this.Name = type.Name;
            this.Type = type;
            this.Template = template;
            this.Default = @default;

            ParseArgs(args);
            ParseTemplateParameters(template);
        }

        private void ParseArgs(object args)
        {
            this.Args = args?.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => p.GetValue(args, null)).ToArray();
        }

        private void ParseTemplateParameters(string template)
        {
            var matches = ParamsRegex.Matches(template);
            if (matches.Count > 0) {
                this.Parameters = new List<RouteParameter>();
                var regex = template;

                foreach (Match match in matches) {
                    var routeParameter = new RouteParameter(match.Value);
                    this.Priority = this.Priority * 10 + routeParameter.Type.GetPriority();
                    this.Parameters.Add(routeParameter);
                    switch (routeParameter.Type) {
                        case RouteTypesHelper.Rest:
                            regex = ParamsRegex.Replace(regex, "(.*)", 1);
                            break;
                        case RouteTypesHelper.String:
                            regex = ParamsRegex.Replace(regex, "([^/]*)", 1);
                            break;
                        default:
                            regex = ParamsRegex.Replace(regex, "([^/]+)", 1);
                            break;
                    }
                }

                var rest = this.Parameters.Count(p => p.Type == RouteTypesHelper.Rest);
                if (rest > 1) {
                    throw new ArgumentException("Incorrect usage of rest", nameof(template));
                }
                if (rest == 1 && this.Parameters.Last().Type != "rest") {
                    throw new ArgumentException("Incorrect usage of rest", nameof(template));
                }

                this.Regex = new Regex("^" + regex, RegexOptions.Compiled);
            }
        }

        private void SetState(object args)
        {
            if (args != null) {
                var values = args.GetType()
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(p => p.Name, p => p.GetValue(args, null));
                this.State = new Dictionary<string, object> { { "args", values } };
            }
        }
    }
}
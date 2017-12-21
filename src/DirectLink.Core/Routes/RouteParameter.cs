// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace DirectLinkCore
{
    public class RouteParameter
    {
        public string Name { get; }

        public string Type { get; }

        public string Format { get; }

        public object Value { get; }

        public string String { get; }

        public string Match { get; }

        public RouteParameter(string match)
        {
            this.Match = match;
            this.String = match.Substring(1, match.Length - 2);
            var firstColon = this.String.IndexOf(':');
            if (firstColon < 0) {
                this.Name = this.String;
                this.Type = RouteTypesHelper.String;
            }
            else {
                this.Name = this.String.Substring(0, firstColon++);
                var secondColon = this.String.IndexOf(':', firstColon);
                if (secondColon < 0) {
                    this.Type = this.String.Substring(firstColon);
                }
                else {
                    this.Type = this.String.Substring(firstColon, secondColon++ - firstColon);
                    this.Format = this.String.Substring(secondColon);
                }
            }
        }

        public RouteParameter(string name, string type, string format, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Format = format;
            this.Value = value;
            this.String = name;
            if (type != null) {
                this.String += $":{type}";
            }
            if (format != null) {
                this.String += $":{format}";
            }
            this.Match = $"{{{this.String}}}";
        }

        public override string ToString() => String;
    }
}
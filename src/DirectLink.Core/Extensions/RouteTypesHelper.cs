// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DirectLinkCore
{
    internal static class RouteTypesHelper
    {
        public const string Rest = "rest";
        public const string String = "string";
        public const string Long = "long";
        public const string Int = "int";
        public const string Guid = "guid";
        public const string DateTime = "datetime";

        public static Dictionary<Type, string> TypeNames = new Dictionary<Type, string> {
            { typeof(string), "string" },
            { typeof(long), "long" },
            { typeof(int), "int" },
            { typeof(Guid), "guid" },
            { typeof(DateTime), "datetime" }
        };

        public static int GetPriority(this string type)
        {
            switch (type) {
                case Rest: return 1;
                case String: return 2;
                case Long: return 3;
                case Int: return 4;
                case Guid: return 5;
                case DateTime: return 6;
                default: return 0;
            }
        }

        public static bool TryGetArgs(List<RouteParameter> parameters, string[] values, ref object[] args)
        {
            var argsList = new List<object>();
            for (int i = 0; i < parameters.Count; ++i) {
                object arg = null;
                var value = values[i];
                bool success = false;
                switch (parameters[i].Type) {
                    case Rest:
                    case String:
                        success = true;
                        arg = value;
                        break;
                    case Int: {
                        success = int.TryParse(value, out int converted);
                        arg = converted;
                        break;
                    }
                    case Long: {
                        success = long.TryParse(value, out long converted);
                        arg = converted;
                        break;
                    }
                    case Guid: {
                        success = parameters[i].Format == null
                            ? System.Guid.TryParse(value, out Guid converted)
                            : System.Guid.TryParseExact(value, parameters[i].Format, out converted);
                        arg = converted;
                        break;
                    }
                    case DateTime: {
                        success = parameters[i].Format == null
                            ? System.DateTime.TryParse(value, out DateTime converted)
                            : System.DateTime.TryParseExact(value, parameters[i].Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out converted);
                        arg = converted;
                        break;
                    }
                }
                if (!success) {
                    return false;
                }
                argsList.Add(arg);
            }
            args = argsList.ToArray();
            return true;
        }

        public static IEnumerable<RouteParameter> ToRouteParameters(this object arg) => arg.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Select(propertyInfo => new RouteParameter(
                propertyInfo.Name,
                TypeNames[propertyInfo.PropertyType],
                null,
                propertyInfo.GetValue(arg, null)
            ));
    }
}
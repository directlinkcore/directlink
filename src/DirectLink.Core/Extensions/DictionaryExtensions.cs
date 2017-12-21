// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    internal static class DictionaryExtensions
    {
        public static IDictionary<TKey, TValue> AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value, Func<TValue, TValue> update = null)
        {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = update == null ? value : update(dictionary[key]);
            }
            else {
                dictionary.Add(key, value);
            }
            return dictionary;
        }

        public static IDictionary<TKey, TValue> Update<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue, TValue> update)
        {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = update(dictionary[key]);
            }
            else {
                dictionary.Add(key, update(default(TValue)));
            }
            return dictionary;
        }
    }
}
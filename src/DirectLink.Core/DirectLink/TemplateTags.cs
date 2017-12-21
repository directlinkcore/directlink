// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace DirectLinkCore
{
    public class TemplateTags
    {
        public string Title { get; set; }

        public ICollection<string> Meta { get; }

        public IDictionary<string, string> MetaNames { get; }

        public IDictionary<string, string> MetaProperties { get; }

        public ICollection<string> Head { get; }

        public ICollection<string> Content { get; }

        public ICollection<string> Body { get; }

        public ICollection<string> GetMetaTags()
        {
            var metaTags = this.Meta.ToList();
            metaTags.AddRange(this.MetaNames.Select(n => (n.Key, n.Value).ToMetaName()));
            metaTags.AddRange(this.MetaProperties.Select(p => (p.Key, p.Value).ToMetaProperty()));
            return metaTags;
        }

        public TemplateTags()
        {
            this.Meta = new List<string>();
            this.MetaNames = new Dictionary<string, string>();
            this.MetaProperties = new Dictionary<string, string>();
            this.Head = new List<string>();
            this.Content = new List<string>();
            this.Body = new List<string>();
        }

        public TemplateTags(TemplateTags tags)
        {
            this.Title = tags.Title;
            this.Meta = new List<string>(tags.Meta);
            this.MetaNames = new Dictionary<string, string>(tags.MetaNames);
            this.MetaProperties = new Dictionary<string, string>(tags.MetaProperties);
            this.Head = new List<string>(tags.Head);
            this.Content = new List<string>(tags.Content);
            this.Body = new List<string>(tags.Body);
        }
    }
}
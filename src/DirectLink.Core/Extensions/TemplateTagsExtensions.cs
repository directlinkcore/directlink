// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public static class TemplateTagsExtensions
    {
        public static TemplateTags AddDefaultTemplateTags(this TemplateTags tags,
            string title,
            string signalR = null,
            bool addBootstrap = true,
            bool isDevelopment = false,
            bool nocache = false,
            bool hmr = false)
        {
            var min = isDevelopment ? "" : ".min";
            var directlink = $"https://unpkg.com/directlink-react@1.0.1/dist/browser/directlink-react{min}.js";
            var polyfill = $"https://cdn.polyfill.io/v2/polyfill{min}.js";
            var jquery = $"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery{min}.js";
            var reactVersion = "16.2.0";

            if (signalR == null) {
                signalR = $"https://unpkg.com/@aspnet/signalr-client@1.0.0-alpha2-final/dist/browser/signalr-clientES5-1.0.0-alpha2-final{min}.js";
            }

            tags.Title = title;

            if (addBootstrap) {
                tags.Head.AddLink("https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/css/bootstrap.min.css", "sha384-PsH8R72JQ3SOdhVi3uxftmaW6Vc51MKb0q5P2rRUpPvrszuE4W1povHYgTpBfshb", "anonymous");
                tags.Body
                    .AddScript("https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.3/umd/popper.min.js", "sha384-vFJXuSJphROIrBnz7yo7oB41mKfc8JzQZiCq4NCceLEaO4IHwicKwpJf9c9IpFgh", "anonymous")
                    .AddScript("https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0-beta.2/js/bootstrap.min.js", "sha384-alpBpkh1PFOepccYVYDB4do5UnbKysX5WZXm3XxPqe5iKTfUKjNkCk9SaVuEZflJ", "anonymous");
            }
            if (isDevelopment) {
                tags.Head
                    .AddScript($"https://unpkg.com/react@{reactVersion}/umd/react.development.js")
                    .AddScript($"https://unpkg.com/react-dom@{reactVersion}/umd/react-dom.development.js")
                    .AddScript(jquery, "sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE=", "anonymous", condition: addBootstrap);
            }
            else {
                tags.Head
                    .AddScript($"https://unpkg.com/react@{reactVersion}/umd/react.production.min.js")
                    .AddScript($"https://unpkg.com/react-dom@{reactVersion}/umd/react-dom.production.min.js")
                    .AddScript(jquery, "sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4=", "anonymous", condition: addBootstrap);
            }
            tags.Head
                .AddScript(polyfill)
                .AddScript(signalR)
                .AddScript(directlink);
            if (nocache) {
                tags.Head.Add("<script>directlink.nocache = true;</script>");
            }
            if (hmr) {
                tags.Head.Add("<script>directlink.hmr = true;</script>");
            }

            return tags;
        }

        #region (...).To[Link|Script|MetaName|MetaProperty]

        public static string ToLink(this (string href, string integrity, string crossorigin) link)
        {
            link.integrity = !string.IsNullOrWhiteSpace(link.integrity) ? $" integrity=\"{link.integrity}\"" : "";
            link.crossorigin = !string.IsNullOrWhiteSpace(link.crossorigin) ? $" crossorigin=\"{link.crossorigin}\"" : "";
            return $"<link rel=\"stylesheet\" href=\"{link.href}\"{link.integrity}{link.crossorigin}>";
        }

        public static string ToScript(this (string src, string integrity, string crossorigin) script)
        {
            script.integrity = !string.IsNullOrWhiteSpace(script.integrity) ? $" integrity=\"{script.integrity}\"" : "";
            script.crossorigin = !string.IsNullOrWhiteSpace(script.crossorigin) ? $" crossorigin=\"{script.crossorigin}\"" : "";
            return $"<script src=\"{script.src}\"{script.integrity}{script.crossorigin}></script>";
        }

        public static string ToMetaName(this (string name, string content) meta)
        {
            return $"<meta name=\"{meta.name}\" content=\"{meta.content}\">";
        }

        public static string ToMetaProperty(this (string property, string content) meta)
        {
            return $"<meta property=\"{meta.property}\" content=\"{meta.content}\">";
        }

        #endregion

        #region ICollection<string>.Add[Link|Script]

        internal static ICollection<string> AddLink(this ICollection<string> tags, string href, string integrity = null, string crossorigin = null, bool condition = true)
        {
            if (condition) {
                tags.Add((href, integrity, crossorigin).ToLink());
            }
            return tags;
        }

        internal static ICollection<string> AddScript(this ICollection<string> tags, string src, string integrity = null, string crossorigin = null, bool condition = true)
        {
            if (condition) {
                tags.Add((src, integrity, crossorigin).ToScript());
            }
            return tags;
        }

        #endregion

        #region TemplateTags.Add[Meta|Head|Content|Body]Tag

        public static TemplateTags AddMetaTag(this TemplateTags tags, string tag)
        {
            tags.Meta.Add(tag);
            return tags;
        }

        public static TemplateTags AddHeadTag(this TemplateTags tags, string tag)
        {
            tags.Head.Add(tag);
            return tags;
        }

        public static TemplateTags AddContentTag(this TemplateTags tags, string tag)
        {
            tags.Content.Add(tag);
            return tags;
        }

        public static TemplateTags AddBodyTag(this TemplateTags tags, string tag)
        {
            tags.Body.Add(tag);
            return tags;
        }

        #endregion

        #region TemplateTags.Meta[Name|Property]

        public static TemplateTags MetaName(this TemplateTags tags, string name, string content, Func<string, string> update = null)
        {
            tags.MetaNames.AddOrUpdate(name, content, update);
            return tags;
        }

        public static TemplateTags MetaName(this TemplateTags tags, string name, Func<string, string> update)
        {
            tags.MetaNames.Update(name, update);
            return tags;
        }

        public static TemplateTags MetaProperty(this TemplateTags tags, string property, string content, Func<string, string> update = null)
        {
            tags.MetaProperties.AddOrUpdate(property, content, update);
            return tags;
        }

        public static TemplateTags MetaProperty(this TemplateTags tags, string property, Func<string, string> update)
        {
            tags.MetaProperties.Update(property, update);
            return tags;
        }

        #endregion

        #region TemplateTags.Meta[Author|Description|Keywords]

        public static TemplateTags MetaAuthor(this TemplateTags tags, string author, Func<string, string> update = null) => tags.MetaName("author", author, update);

        public static TemplateTags MetaAuthor(this TemplateTags tags, Func<string, string> update) => tags.MetaName("author", update);

        public static TemplateTags MetaDescription(this TemplateTags tags, string description, Func<string, string> update = null) => tags.MetaName("description", description, update);

        public static TemplateTags MetaDescription(this TemplateTags tags, Func<string, string> update) => tags.MetaName("description", update);

        public static TemplateTags MetaKeywords(this TemplateTags tags, string keywords, Func<string, string> update = null) => tags.MetaName("keywords", keywords, update);

        public static TemplateTags MetaKeywords(this TemplateTags tags, Func<string, string> update) => tags.MetaName("keywords", update);

        #endregion
    }
}
// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DirectLinkCore
{
    internal class DirectLinkData : IDirectLinkData
    {
        private readonly IServiceProvider _provider;
        private readonly IAuthorizationService _authorizationService;
        private readonly IComponentsService _componentsService;

        public string App { get; }
        public string Title { get; set; }
        public IDictionary<string, object> States { get; }
        public IDictionary<string, string> Scripts { get; }
        public IDictionary<string, string> Styles { get; }
        public IDictionary<string, IList<string>> Methods { get; }
        public ICollection<string> Bidirectional { get; }
        public string Message { get; set; }
        public bool IsRouted { get; set; }
        public bool IsAuthorized { get; set; }

        public DirectLinkData(
            string app,
            IServiceProvider provider,
            IAuthorizationService authorizationService,
            IComponentsService componentsService
        )
        {
            _provider = provider;
            _authorizationService = authorizationService;
            _componentsService = componentsService;

            this.App = app;
            this.Title = null;
            this.States = new Dictionary<string, object>();
            this.Scripts = new Dictionary<string, string>();
            this.Styles = new Dictionary<string, string>();
            this.Methods = new Dictionary<string, IList<string>>();
            this.Bidirectional = new HashSet<string>();
            this.Message = null;
            this.IsRouted = false;
            this.IsAuthorized = true;
        }

        public async Task<bool> AddStateAsync(string name, string fullname, IViewModel viewModel, IPrincipal user)
        {
            this.States.Add(fullname, viewModel);
            if (!this.Methods.ContainsKey(name)) {
                this.Methods.Add(name, _componentsService.GetMethodNames(name));
            }

            if (viewModel.Components != null) {
                foreach (var component in viewModel.Components) {
                    if (component.Type != null) {
                        var authorized = await AuthorizationHelper.AuthorizeAsync(component.Type, user, _authorizationService);
                        if (!authorized) {
                            return false;
                        }
                    }

                    var childName = component.Type != null
                        ? _componentsService.GetComponentName(component.Type)
                        : component.Name;
                    var childFullName = $"{fullname}.{childName}";
                    if (component.Id != null) {
                        childFullName += $"${component.Id}";
                    }
                    AddAssets(childName);

                    if (component.Type != null) {
                        var link = (IInternalDirectLink<ViewModel>)_provider.GetService(component.Type);
                        var args = component.Args ?? Array.Empty<object>();

                        var childViewModel = CreateViewModel(link, args, childName);

                        var succeeded = await AddStateAsync(childName, childFullName, childViewModel, user);
                        if (!succeeded) {
                            return false;
                        }
                    }
                    else {
                        if (component.State != null) {
                            States.Add(childFullName, component.State);
                        }
                    }
                }
            }
            return true;
        }

        public IViewModel CreateViewModel(IInternalDirectLink<ViewModel> link, object[] args, string name)
        {
            var viewModel = (IViewModel)ActivatorUtilities.CreateInstance(_provider, link.ViewModelType, args);

            if (link.LinkType == DirectLinkType.Dispatcher || link.LinkType == DirectLinkType.Controller) {
                if (!this.Bidirectional.Contains(name)) {
                    this.Bidirectional.Add(name);
                }
            }

            return viewModel;
        }

        public void AddAssets(string name)
        {
            var scriptPath = _componentsService.GetScriptPath(name);
            if (!string.IsNullOrWhiteSpace(scriptPath)) {
                this.Scripts.Add(name, scriptPath);
            }
            var stylePath = _componentsService.GetStylePath(name);
            if (!string.IsNullOrWhiteSpace(stylePath)) {
                this.Styles.Add(name, stylePath);
            }
        }
    }
}
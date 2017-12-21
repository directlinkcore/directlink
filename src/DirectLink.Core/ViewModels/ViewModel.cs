// Copyright (c) 2017 Andrei Molchanov. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace DirectLinkCore
{
    public class ViewModel : IViewModel
    {
        private ICollection<ComponentDescriptor> _components;

        ICollection<ComponentDescriptor> IViewModel.Components => _components;

        public void AddComponents(Action<ICollection<ComponentDescriptor>> setupAction)
        {
            if (_components == null) {
                _components = new List<ComponentDescriptor>();
            }
            setupAction(_components);
        }
    }
}
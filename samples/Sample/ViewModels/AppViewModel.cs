using System.Collections.Generic;
using DirectLinkCore;

namespace Sample
{
    public class AppViewModel : ViewModel
    {
        public IReadOnlyCollection<Message> Messages { get; }

        public AppViewModel(App app) => Messages = app.GetMessages();
    }
}
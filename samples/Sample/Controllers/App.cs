using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using DirectLinkCore;

namespace Sample
{
    public class App : DirectLinkDispatcher<AppViewModel>
    {
        private static ConcurrentQueue<Message> _messages = new ConcurrentQueue<Message>();

        public IReadOnlyCollection<Message> GetMessages() => _messages;

        public async Task AddMessage(string text)
        {
            _messages.Enqueue(new Message(text));
            await SetStateAsync(new { Messages = _messages }); //here we setState of App component
        }
    }
}
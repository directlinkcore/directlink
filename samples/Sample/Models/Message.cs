using System;

namespace Sample
{
    public class Message
    {
        public Guid Id { get; }

        public string Text { get; }

        public Message(string text) => (Id, Text) = (Guid.NewGuid(), text);
    }
}
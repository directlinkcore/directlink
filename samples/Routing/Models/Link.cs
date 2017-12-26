namespace RoutingExample
{
    public class Link
    {
        public string Name { get; }
        public string Href { get; }
        public Link(string name, string href) => (Name, Href) = (name, href);
    }
}
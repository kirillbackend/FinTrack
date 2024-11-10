namespace FinTrack.Services.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, string uiMessage = null, object id = null) : base(message)
        {
            UIMessage = uiMessage;
            Id = id;
        }

        public string UIMessage { get; }

        public object Id { get; }
    }
}

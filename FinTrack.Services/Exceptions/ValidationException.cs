namespace FinTrack.Services.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, string uiMessage = null,
            object validationSource = null)
            : base(message)
        {
            UIMessage = uiMessage;
            ValidationSource = validationSource;
        }

        public string UIMessage { get; }

        public object ValidationSource { get; }
    }
}

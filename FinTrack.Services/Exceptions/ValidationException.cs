namespace FinTrack.Services.Exceptions
{
    public class ValidationException : Exception
    {
        private readonly string? _paramName;

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, string paramName)
            : base(message)
        {
            _paramName = paramName;
        }

        public virtual string? ParamName => _paramName;
    }
}

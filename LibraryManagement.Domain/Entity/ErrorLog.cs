namespace LibraryManagement.Domain.Entity
{
    public class ErrorLog
    {
        public int Id { get; private set; }

        public string Message { get; private set; } = null!;
        public string? StackTrace { get; private set; }
        public string? InnerMessage { get; private set; }
        public string? InnerStackTrace { get; private set; }
        public string? Path { get; private set; }
        public string? Method { get; private set; }

        public DateTime CreatedAt { get; private set; }

        public ErrorLog(
            string message,
            string? stackTrace,
            string? innerMessage,
            string? innerStackTrace,
            string? path,
            string? method)
        {
            Message = message;
            StackTrace = stackTrace;
            InnerMessage = innerMessage;
            InnerStackTrace = innerStackTrace;
            Path = path;
            Method = method;
            CreatedAt = DateTime.UtcNow;
        }
    }

}

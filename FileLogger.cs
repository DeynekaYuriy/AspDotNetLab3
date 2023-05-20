namespace Lab3
{
    public class FileLogger : ILogger, IDisposable
    {
        string filePath;
        static object locker = new object();
        public FileLogger(string filePath)
        {
            this.filePath = filePath;
        }
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {

        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            lock (locker)
            {
                File.AppendAllText(filePath, formatter(state, exception) + Environment.NewLine);
            }
        }
    }
}

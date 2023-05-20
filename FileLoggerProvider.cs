namespace Lab3
{
    public class FileLoggerProvider : ILoggerProvider
    {
        string filePath;
        public FileLoggerProvider(string filePath)
        {
            this.filePath = filePath;
        }
        public ILogger CreateLogger(string categoryName)
        {
            Console.WriteLine("CATEGORY: " + categoryName);
            return new FileLogger(filePath);
        }

        public void Dispose()
        {
        }
    }
}

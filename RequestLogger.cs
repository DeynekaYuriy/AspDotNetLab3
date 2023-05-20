namespace Lab3
{
    public class RequestLogger
    {
        public readonly RequestDelegate next;
        ILoggerFactory loggerFactory;
        ILogger logger;
        public RequestLogger(RequestDelegate next, IConfiguration appConfiguration)
        {
            this.next = next;
            loggerFactory = LoggerFactory.Create(builder => builder.AddFile(appConfiguration["LogFile"]));
            logger = loggerFactory.CreateLogger<Program>();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var ip = context.Request.HttpContext.Connection.RemoteIpAddress;
            logger.LogInformation($"Path: {context.Request.Path} | IP: {ip} | DATETIME: {DateTime.Now}");
            await next.Invoke(context);
        }
    }
}

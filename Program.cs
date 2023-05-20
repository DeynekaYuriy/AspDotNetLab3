using Lab3;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});
builder.Configuration.AddJsonFile("conf.json");
var app = builder.Build();

var loggerFactory = LoggerFactory.Create(builder => builder.AddFile(app.Configuration["LogFile"]));
var requestLogger = loggerFactory.CreateLogger<Program>();
app.UseSession();
app.UseRequestLogger();

app.MapGet("/", async (context) =>
{
    await WriteAsync(context, "/");
});


app.Map("{lang?}/{controller}/{action}/{id:int?}", async (HttpContext context, string? lang, string controller, string action, int? id) =>
{
    await WriteAsync(context, $"Lang: {lang}\nController: {controller}\nAction: {action}\nId: {id}");
});

app.Map("/{controller}/{action}/{id:int?}", async (HttpContext context, string controller, string action, int? id) =>
{
    await WriteAsync(context, $"Controller: {controller}\nAction: {action}\nId: {id}");
});


//-------------------------Cookies Routing-------------------------
app.Map("/Cookie/Add/{name}/{value}", async (HttpContext context, string name, string value) =>
{
    context.Response.Cookies.Append(name, value);
    await WriteAsync(context, $"Cookie {name} with value {value} has been added");
});

app.Map("/Cookie/View/{name}", async (HttpContext context, string name) =>
{
    context.Request.Cookies.TryGetValue(name, out var value);
    if (value != null)
    {
        await WriteAsync(context, $"Cookie {name} with value {value}");
    }
    else
    {
        await WriteAsync(context, $"There is no {name} cookie");
    }
});

//-------------------------Session Routing-------------------------

app.Map("/Session/Add/{name}/{value}", async (HttpContext context, string name, string value) =>
{
    context.Session.SetString(name, value);
    await WriteAsync(context, $"Property {name} with value {value} has been added");
});

app.Map("/Session/View/{name}", async (HttpContext context, string name) =>
{
    if (context.Session.Keys.Contains(name))
    {
        var value = context.Session.GetString(name);
        await WriteAsync(context, $"Property {name} with value {value}");
    }
    else
    {
        await WriteAsync(context, $"There is no {name} property");
    }
});

Task WriteAsync(HttpContext context, string content)
{
    return context.Response.WriteAsync($"{app.Configuration["Title"]}\n\n{content}");
}
app.Run();

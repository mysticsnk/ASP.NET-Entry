using System.Security.Principal;
using System.Text;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

int counter = 0;

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/")
    {
        await context.Response.WriteAsync("/counter - task 2\n" +
                                          "/time - task 3\n" +
                                          "/10items - task 4\n" +
                                          "/headers - task 5\n" +
                                          "/user - task 6");
    }

    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/counter")
    {
        counter++;
        await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(counter.ToString()), 0,
            counter.ToString().Length);
    }
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/time")
    {
        string currentTime = DateTime.Now.ToString();
        await context.Response.WriteAsync(currentTime);

    }
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/10items")
    {
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(@"
<ul>
    <li>1</li>
    <li>2</li>
    <li>3</li>
    <li>4</li>
    <li>5</li>
    <li>6</li>
    <li>7</li>
    <li>8</li>
    <li>9</li>
    <li>10</li>
</ul>  
");
    }

    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/headers")
    {
        await context.Response.WriteAsync(context.Request.Headers.Aggregate("", (current, next) =>
        {
            current += $"{next.Key}={next.Value}\n";
            return current;
        }));
    }

    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "GET" &&
        context.Request.Path == "/user")
    {
        string? name = context.Request.Query["name"];
        string? surname = context.Request.Query["surname"];
        string? email = context.Request.Query["email"];
        string? gender = context.Request.Query["gender"];
        string? position = context.Request.Query["position"];
        
        int? age = null;
        if (Int32.TryParse(context.Request.Query["age"], out int ageValue))
        {
            age = ageValue;
        }

        int? height = null;
        if (Int32.TryParse(context.Request.Query["height"], out int heightValue))
        {
            height = heightValue;
        }

        string profile = $"Profile of {surname ?? ""} {name ?? ""}";
        if (email != null) profile += $"\n{email}";
        if (age != null) profile += $"\n{age} years old";
        if (height != null) profile += $"\n{height} cm";
        if (gender != null) profile += $"\n{gender}";
        if (position != null) profile += $"\nPosition: {position}";
        await context.Response.WriteAsync(profile);
    }

    await next();
});

app.Run();

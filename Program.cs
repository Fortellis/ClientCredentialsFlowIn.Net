using System.Net;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet("/HelloWorld", () =>
{
    return new string[] { "value", "someValue" };
});

app.MapPost("/token", async () =>
{
    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
    var url = new Uri("https://identity-dev.fortellis.io/oauth2/aus1ni5i9n9WkzcYa2p7/v1/token");
    var handler = new HttpClientHandler();
    using var client = new HttpClient();
    client.DefaultRequestHeaders.Add("Authorization", "Basic {yourBase64EncodedAPIKey:APISecret}");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    HttpContent data = new FormUrlEncodedContent(new[] 
    {
        new KeyValuePair<string, string>("scope", "anonymous"),
        new KeyValuePair<string, string>("grant_type", "client_credentials")

    });

    Console.WriteLine($"This is what's in the data: {data.ToString}");

    var res = await client.PostAsync(url, data);

    var content = await res.Content.ReadAsStringAsync();

    Console.WriteLine("This is the content of the response: " + content);
    return content;
});

app.Run();

using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Shop.Api.Gateway.Config;
using Shop.Api.Gateway.MessageHandler;
using Shop.Api.Gateway.RemoteServices;

var builder = WebApplication.CreateBuilder(args);

var routes = "DockerRoutes"; //docker
//var routes = "LocalRoutes"; //local
builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = routes;
});

builder.Services.AddOcelot(builder.Configuration).AddPolly().AddDelegatingHandler<BookHandler>();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();


builder.Services.AddSingleton<IAuthorService, AuthorService>();
builder.Services.AddControllers();
builder.Services.AddHttpClient("Authors", config => config.BaseAddress = new Uri(builder.Configuration["Services:Authors"]));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;

}).UseOcelot().Wait();

app.MapControllers();

app.Run();


//var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("ocelot.books.json", optional: false, reloadOnChange: true);
//builder.Services.AddOcelot();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();
//app.UseOcelot().Wait();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//else
//{
//    app.UseHttpsRedirection();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Cart.Application;
using Shop.Api.Cart.RemoteServices;
using Shop.Api.Cart.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewCart>());

builder.Services.AddDbContext<CartContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
}, ServiceLifetime.Scoped);

builder.Services.AddMediatR(typeof(NewCart.Handler).Assembly);
//builder.Services.AddAutoMapper(typeof(QueryBook.Handler));
builder.Services.AddHttpClient("Books", config => config.BaseAddress = new Uri(builder.Configuration["Services.Books"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
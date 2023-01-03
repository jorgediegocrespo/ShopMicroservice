using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Books.Application;
using Shop.Api.Books.Repository;
using Shop.Messages.Bus.Bus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEventBus, RabbitEventBus>();
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewBook>());


builder.Services.AddDbContext<BookContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
}, ServiceLifetime.Scoped);

builder.Services.AddMediatR(typeof(NewBook.Handler).Assembly);
builder.Services.AddAutoMapper(typeof(QueryBook.Handler));

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
else
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
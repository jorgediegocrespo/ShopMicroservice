using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Authors.Application;
using Shop.Api.Authors.EventBusHandlers;
using Shop.Api.Authors.Repository;
using Shop.Messages.Bus.Bus;
using Shop.Messages.Bus.EventQueues;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IEventBus, RabbitEventBus>(x => new RabbitEventBus(x.GetService<IMediator>(), x.GetRequiredService<IServiceScopeFactory>()));
builder.Services.AddTransient<EmailEventHandler>();
builder.Services.AddTransient<IEventHandler<EmailMessageEventQueue>, EmailEventHandler>();
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<NewAuthor>());
    
builder.Services.AddDbContext<AuthorContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddMediatR(typeof(NewAuthor.Handler).Assembly);
builder.Services.AddAutoMapper(typeof(QueryAuthor.Handler));
    
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

app.Services.GetRequiredService<IEventBus>().Subscribe<EmailMessageEventQueue, EmailEventHandler>();

app.Run();
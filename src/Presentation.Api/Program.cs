using Application;
using Domain.EventSourcing;
using Infrastructure.EventSourcing;
using Infrastructure.MongoDb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped(typeof(IPersistence<>), typeof(Persistence<>));
builder.Services.AddScoped(typeof(IStateStore<>), typeof(MongoStateStore<>));
builder.Services.AddScoped<IEventStore, MongoEventStore>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationMarker).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
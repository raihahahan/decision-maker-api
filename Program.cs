using DecisionMakerApi.Features.RandomDecision.Models;
using DecisionMakerApi.Features.ConditionalDecision.Models;
using DecisionMakerApi.Features.WeightedDecision.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register database context
builder.Services.AddDbContext<RandomDecisionContext>(opt =>
    opt.UseInMemoryDatabase("RandomDecisionList"));
builder.Services.AddDbContext<ConditionalDecisionContext>(opt =>
    opt.UseInMemoryDatabase("ConditionalDecisionList"));
builder.Services.AddDbContext<WeightedDecisionContext>(opt =>
    opt.UseInMemoryDatabase("WeightedDecisionList"));

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

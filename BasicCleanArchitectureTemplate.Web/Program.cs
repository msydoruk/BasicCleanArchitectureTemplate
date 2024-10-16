using AutoMapper;
using BasicCleanArchitectureTemplate.Core.Factories;
using BasicCleanArchitectureTemplate.Core.Interfaces;
using BasicCleanArchitectureTemplate.Core.Models;
using BasicCleanArchitectureTemplate.Core.Services;
using BasicCleanArchitectureTemplate.Infrastructure.Data;
using BasicCleanArchitectureTemplate.Infrastructure.Repositories;
using BasicCleanArchitectureTemplate.Web.Logging;
using BasicCleanArchitectureTemplate.Web.Mapping;
using BasicCleanArchitectureTemplate.Web.Middleware;
using BasicCleanArchitectureTemplate.Web.ServiceExtension;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddLogger();

var mapperConfig = new MapperConfiguration(mapperConfiguration => { mapperConfiguration.AddProfile(new MappingProfile()); });
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddCustomValidators();
builder.Services.RegisterCustomValidators();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventOccurrenceStrategyFactory, EventOccurrenceStrategyFactory>();
builder.Services.AddScoped<IEventService, EventService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseExceptionHandling();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}");

app.Run();

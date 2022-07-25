using System.Reflection;
using Comics.ApplicationCore;
using Comics.ApplicationCore.Data;
using Comics.ApplicationCore.Features.Registration;
using Comics.ApplicationCore.Middleware;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(MediatorAssemblyMarker).Assembly);
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Comics.ApplicationCore")));
builder.Services.AddRegistrationServices();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
builder.Services.AddDbContext<ComicsDbContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddScoped<ErrorHandlingMiddleware>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program{}

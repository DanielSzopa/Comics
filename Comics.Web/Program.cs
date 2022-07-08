using System.Reflection;
using Comics.ApplicationCore;
using Comics.ApplicationCore.Features.Registration;
using FluentValidation.AspNetCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(MediatorAssemblyMarker).Assembly);
builder.Services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("Comics.ApplicationCore")));
builder.Services.AddRegistrationServices();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

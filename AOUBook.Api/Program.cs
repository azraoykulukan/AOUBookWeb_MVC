using AOUBook.DataAccess.Repository.IRepository;
using AOUBook.DataAccess.Repository;
using AOUBook.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using AOUBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using System;
using AOUBook.Models;
using AOUBook.Api.Validatior;
using AOUBook.Models.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//builder.Services.AddScoped<IValidator<Category>, CategoryValidator>();
//builder.Services.AddScoped<IValidator<ProductVM>, ProductValidatior> ();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());





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

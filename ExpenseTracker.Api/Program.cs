using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Api.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//регистрируем AppDbContext.cs как сервис приложения
builder.Services.AddDbContext<AppDbContext>     
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))); //база работает через SQL Server + строка подключения

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

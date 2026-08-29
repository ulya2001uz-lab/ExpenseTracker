using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Core;

namespace ExpenseTracker.Api.Data
{
    public class AppDbContext:DbContext //базовый класс EF Core дает всю функциональность работы с БД
    {
        //конструктор ниже - это "настройки подключения", которые EF передаст при запуске (адрес базы и тд)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        }

        public DbSet<Expense> Expenses { get; set; }        //объявляем таблицу Expenses; DbSet<Expense> - это как список, но в базе
    }
}

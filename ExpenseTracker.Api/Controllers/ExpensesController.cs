using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Core;
using ExpenseTracker.Api.Data;
using System.Runtime.InteropServices;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]             //метка/атрибут говорит "это контроллер веб-API"
    [Route("expenses")]         //задает адрес, контроллер отвечает по пути/expenses
    public class ExpensesController : ControllerBase      //ControllerBase - дает готовые возможности отвечать на запросы
    {
        private readonly AppDbContext _context;     //поле, где контроллер будет хранить доступ к базе

        public ExpensesController(AppDbContext dbContext)
        {
            _context = dbContext;
        }                                               //сохраняем полученную базу в поле, чтобы пользоваться ей во всех методах

        [HttpGet]                                       //метка над методом GetAll(): "этот метод отвечает на GET-запрос" (отдать данные)
        public List<Expense> GetAll()                   //возвращает список расходов
        {
            return _context.Expenses.ToList();
        }
        [HttpPost]                                      //метка "этот метод отвечает на POST-запрос" (принять данные), в отличие от [HttpsGet] 
        public IActionResult Add(Expense expense)             //метод добавления расхода
        {
            if(expense == null)
            {
                return BadRequest("Нет данных для заполнения. Введите данные!");
            }
            expense.Date = DateOnly.FromDateTime(DateTime.Now);
            _context.Expenses.Add(expense);
            _context.SaveChanges();

            return Ok(new {message = "Данные добавлены", expense});
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)                      //метод удаления одного расхода по его ID
        {
            Expense foundExpense = _context.Expenses.Find(id);
            if (foundExpense == null)
            {
                return NotFound("Расход не найден");
            }
            _context.Remove(foundExpense);
            _context.SaveChanges();

            return Ok(new { message = "Расход удален", expense = foundExpense});
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] Expense updated)          //метод для изменения расхода
        {                                                                             //FromRoute/FromBody - указывают asp.net откуда брать параметры            
            Expense foundExpense = _context.Expenses.Find(id);
            if (foundExpense == null)
            {
                return NotFound("Расход не найден");
            }

            foundExpense.Amount = updated.Amount;
            foundExpense.Category = updated.Category;
            foundExpense.Date = DateOnly.FromDateTime(DateTime.Now);
            _context.SaveChanges();

            return Ok(new {message = "Расход изменен", expense = foundExpense});
        }
    }
}
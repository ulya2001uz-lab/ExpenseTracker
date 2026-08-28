using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Core;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]             //метка/атрибут говорит "это контроллер веб-API"
    [Route("expenses")]         //задает адрес, контроллер отвечает по пути/expenses
    public class ExpensesController : ControllerBase      //ControllerBase - дает готовые возможности отвечать на запросы
    {
        private static List<Expense> expenses = new();
        private static int nextId = 1;

        [HttpGet]                                       //метка над методом GetAll(): "этот метод отвечает на GET-запрос" (отдать данные)
        public List<Expense> GetAll()                   //возвращает список расходов
        {
            return expenses;
        }
        [HttpPost]                                      //метка "этот метод отвечает на POST-запрос" (принять данные), в отличие от [HttpsGet] 
        public Expense Add(Expense expense)             //метод добавления расхода
        {
            expense.Id = nextId;
            nextId++;
            expense.Date = DateTime.Now;
            expenses.Add(expense);
            return expense;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)                      //метод удаления одного расхода по его ID
        {
            expenses.RemoveAll(x => x.Id == id);
        }
    }
}

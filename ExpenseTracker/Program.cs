using ExpenseTracker.Core;

List<Expense> expenses = new();

bool running = true;
while (running)
{
    Console.Write("Возможные действия:" +'\n'+ "1 - Добавить расход" + '\n' + "2 - Показать все расходы" 
                    + '\n'+ "3 - Показать итого расходов" + '\n' + "4 - Расходы по категориям:" + '\n'+ "5 - Выход" + '\n' + "Выберите действие: ");
    string choiceExit = Console.ReadLine();
    if (choiceExit == "1")
    {
        AddExpense(expenses);     
    }
    else if (choiceExit == "2")
    {
        ShowExpenses(expenses);
    }
    else if (choiceExit == "3")
    {
       ShowTotal(expenses);
    }
    else if (choiceExit == "4")
    {
       ShowByCategory(expenses);
    }                                                       
    else if (choiceExit == "5")                             
    {
        running = false;
        Console.WriteLine("До свидания!");
    }
    else
    {
        Console.WriteLine("Неверный код действия! Попробуйте снова!");
    }
}

void AddExpense(List<Expense> expenses)
{
    Expense object1 = new();
    Console.Write("Введите сумму трат: ");
    decimal newAmount = Convert.ToDecimal(Console.ReadLine());
    Console.Write("Введите категорию трат: ");
    string categoryExpenses = Console.ReadLine();
    object1.Amount = newAmount;
    object1.Category = categoryExpenses;
    object1.Date = DateTime.Now;
    expenses.Add(object1);
}

void ShowExpenses(List<Expense> expenses)
{
    if (expenses.Count != 0)
    {
        Console.WriteLine("Все расходы:");
        foreach (Expense expense in expenses)
        {
            Console.WriteLine(expense.Amount + " - " + expense.Category + " - " + expense.Date);
        }
    }
    else
    {
        Console.WriteLine("Данных о расходах нет!");
    }
}

void ShowTotal(List<Expense> expenses)
{
    decimal totalExpenses = 0;
    foreach (Expense expense in expenses)
    {
        totalExpenses += expense.Amount;
    }
    if (totalExpenses > 0)
    {
        Console.WriteLine("Итого: " + totalExpenses);
    }
    else
    {
        Console.WriteLine("Данных о расходах нет!");
    }
}

void ShowByCategory(List<Expense> expenses)
{
    //Dictionary хранит пары "ключ->значение", как словарь, где можно найти перевод слова.
    //Он нужен, чтобы связать категорию и накопленную в ней сумму. Ключ всегда уникален,
    //что позволяет быстро найти и обновить сумму в той или иной категории.
    Dictionary<string, decimal> allCategories = new();
    foreach (Expense expense in expenses)
    {
        if (allCategories.ContainsKey(expense.Category))
        {
            allCategories[expense.Category] += expense.Amount;  //суммируем траты по конкретной категории
        }
        else
        {
            allCategories[expense.Category] = expense.Amount;   //запись новой категории, если ее нет
        }
    }
    foreach (var pairs in allCategories)
    {
        Console.WriteLine(pairs.Key + " : " + pairs.Value); 
    }
}
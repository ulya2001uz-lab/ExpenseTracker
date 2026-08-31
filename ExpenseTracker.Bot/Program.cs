using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Microsoft.Extensions.Configuration;

using System.Net.Http.Json;


//читаем конфигурацию, включая User Secrets, чтобы не "палить" токен бота
var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

//достаем токен
var tokenBot = configuration["BotToken"];

var httpClient = new HttpClient();                    //для отправки запросов к API, "почтальон"
var botClient = new TelegramBotClient(tokenBot);      //пульт управления моим ботом

using CancellationTokenSource cts = new();      //остановка бота, когда программа завершается, "выключатель"

var receiverOptions = new ReceiverOptions       //настройка "прослушки": что именно слушать
{
    AllowedUpdates = Array.Empty<UpdateType>()  //слушаем ВСЕ типы событий по умолчанию
};

//"прослушка" для бота, проверка на новые сообщения в ТГ
botClient.StartReceiving(                   //включаем "прослушку", чтобы ловить сообщения
    updateHandler: HandleUpdateAsync,       //когда придет сообщение будет вызван метод HandleUpdateAsync
    errorHandler: HandleErrorAsync,         //если будет ошибка, будет вызван метод HandleErrorAsync
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMe();
Console.WriteLine($"Бот @{me.Username} запущен! Нажмите Enter для остановки.");
Console.ReadLine();     //без этой строки программа просто закроется, а пока программа "висит", бот в фоне слушает и отвечает
cts.Cancel();

//обработчик входящих сообщений
async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
{
    ///проверка, что пришло именно сообщение
    if (update.Message is not { } message)
        return;

    //проверка, что в сообщении есть текст
    if (message.Text is not { } messageText)
        return;

    Console.WriteLine($"Получено сообщение: {messageText}");

    //разрезаем сообщение по пробелу
    string[] parts = messageText.Split(' ');

    //проверяем, что частей ровно 2 (категория и сумма)
    if (parts.Length != 2)
    {
        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: "Напиши в формате: категория сумма (например: кофе 500)",
            cancellationToken: token
        );
        return;
    }

    string category = parts[0];           //первая часть - категория
    string amountText = parts[1];         //вторая часть - сумма (пока текст)

    //пробуем превратить сумму в число
    if (!decimal.TryParse(amountText, out decimal amount))
    {
        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: "Сумма должна быть числом. Например: кофе 500",
            cancellationToken: token
        );
        return;
    }

    //собираем данные расхода для отправки
    var newExpense = new
    {
        amount = amount,
        category = category
    };

    try
    {       
        //ответ от API, полученный после того, как наш "почтальон" превратил объект в данные JSON,
        //где есть два аргумента: куда и что отправить (адрес и объект)
        var response = await httpClient.PostAsJsonAsync("http://localhost:5176/expenses", newExpense);

        if (response.IsSuccessStatusCode)
        {
            await bot.SendMessage(
                chatId: message.Chat.Id,
                text: $"Записал: {category} - {amount}",
                cancellationToken: token
                );
        }
        else
        {
            await bot.SendMessage(
                chatId: message.Chat.Id,
                text: "Не удалось сохранить расход! Попробуйте позже.",
                cancellationToken: token
                );
        }
    }
    catch
    {
        await bot.SendMessage(
            chatId: message.Chat.Id,
            text: "Ошибка связи с сервером. Проверьте, запущен ли API!",
            cancellationToken: token
            );
    }
}

//обработчик ошибок
Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
{
    Console.WriteLine($"Ошибка: {exception.Message}");
    return Task.CompletedTask;
}
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

using Microsoft.Data.SqlClient;
using System.Text;

internal class Program
{
    static string connectionString = "" + //подсоединение к бд
    "Data Source=(localdb)\\MSSQLLocalDB;" +
    "Initial Catalog=test;" +
    "Integrated Security=True;" +
    "Connect Timeout=30;";

    private static string token { get; set; } = "1974124251:AAHIxZTHqPU5p84ADtokqx5nZfLnuWstR6E"; //токен моего тг бта
    private static TelegramBotClient client;
    static void Main(string[] args)
    {
        client = new TelegramBotClient(token); //создание клиента для работы
        client.StartReceiving(); //запуск бота
        client.OnMessage += OnMessageHandler; //работа
        Console.ReadLine(); //выполнять до ввода в консоль
        client.StopReceiving(); //окончание работы бота
    }

    private static async void OnMessageHandler(object? sender, MessageEventArgs e) //реакция на сообщение
    {
        var msg = e.Message;
        if (msg.Text == "Send Database")
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = connectionString;
                sqlConnection.Open();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();
                sqlCommand.CommandText = "SELECT * FROM web_db";
                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        object[] objects = new object[reader.FieldCount];
                        reader.GetValues(objects);
                        StringBuilder text = new StringBuilder();
                        foreach (object obj in objects)
                            text.Append(obj.ToString() + "\t");
                        await client.SendTextMessageAsync(msg.Chat.Id, text.ToString(), replyMarkup: GetButtons());
                    }
                    sqlConnection.Close();
                    sqlCommand.CommandText = "TRUNCATE TABLE web_db";
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
                else
                    await client.SendTextMessageAsync(msg.Chat.Id, "Database is empty", replyMarkup: GetButtons());
            }
        }
        else
            await client.SendTextMessageAsync(msg.Chat.Id, "Incorrect Message", replyMarkup: GetButtons());
    }
    private static IReplyMarkup GetButtons() //клавиатура для ответа
    {
        return new ReplyKeyboardMarkup
        {
            Keyboard = new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton { Text = "Send Database" } }
            },
            ResizeKeyboard = true,
        };
    }
}
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models
{
    public class Text
    {
        public static string connectionString = "" + //подсоединение к бд
            "Data Source=(localdb)\\MSSQLLocalDB;" +
            "Initial Catalog=test;" +
            "Integrated Security=True;";

        [Required(ErrorMessage = "Необходимо ввести текст")]
        public string Message { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web;

namespace WebApplication1.Models
{
    public static class GlobalVariables
    {
        // UserName, read-write variable
        public static int UserId
        {
            get {
                object userId = HttpContext.Current.Application["UserId"];
                return userId == null ? 0 : (int) userId;
            }
            set { HttpContext.Current.Application["UserId"] = value; }
        }

        // UserName, read-write variable
        public static string UserName
        {
            get { return HttpContext.Current.Application["UserName"] as string; }
            set { HttpContext.Current.Application["UserName"] = value; }
        }

        // IsAdmin, read-write variable
        public static bool IsAdmin
        {
            get { return (HttpContext.Current.Application["isAdmin"] as string) == "True"; }
            set { HttpContext.Current.Application["isAdmin"] = (value ? "True" : "False"); }
        }
    }

    // Контекст для запроса данных о пользователе, при авторизации
    public class UserContext : DbContext
    {
        public UserContext() : base("masterEntities")
        {
        }

        public DbSet<TableUser> Users { get; set; }
    }

    // Модель для страницы авторизации
    public class UserLogin
    {
        [Required]
        [Display(Name = "Логин")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
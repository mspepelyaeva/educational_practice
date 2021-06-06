using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        // Обработка нажатия на кнопку "Вход" (+авторизация)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                TableUser user = null;
                using (UserContext db = new UserContext())
                {
                    user = db.Users.FirstOrDefault(u => u.login == model.Name && u.password == model.Password);
                }
                if (user != null)
                {
                    // Добавление данных об авторизованном пользователе в "куки"
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    // Админ или простой пользователь
                    bool isAdmin = (model.Name.IndexOf("admin") != -1);
                    GlobalVariables.IsAdmin = isAdmin;
                    GlobalVariables.UserId = user.Id;
                    GlobalVariables.UserName = model.Name;
                    // Переход на Главную страницу
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }
            return View(model);
        }

        // Обработка нажатия на кнопку "Выход"
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //------------------------------------------------------
        // Формирование отчёта о сделках покупки
        public ActionResult ReportBue()
        {
            DataTable dataTable = ControlDataBase.PortfolioSelect(true);
            OutInExcel("Отчёт о сделках покупки", dataTable);
            return RedirectToAction("Index", "Home");
        }

        // Формирование отчёта о сделках продажи
        public ActionResult ReportSell()
        {
            DataTable dataTable = ControlDataBase.PortfolioSelect(false);
            OutInExcel("Отчёт о сделках продажи", dataTable);
            return RedirectToAction("Index", "Home");
        }

        // Формирование отчёта о выплаченных дивидендах
        public ActionResult ReportDividendHistory()
        {
            DataTable dataTable = ControlDataBase.DividendSelectHistory();
            OutInExcel("Отчёт о выплаченных дивидендах", dataTable);
            return RedirectToAction("Index", "Home");
        }

        // Формирование отчёта об ожидаемых дивидендах
        public ActionResult ReportDividendNext()
        {
            DataTable dataTable = ControlDataBase.DividendSelectNext();
            OutInExcel("Отчёт об ожидаемых дивидендах", dataTable);
            return RedirectToAction("Index", "Home");
        }

        public DataTable TablePortfolioFilter(int userId)
        {
            using (var db = new masterEntities())
            {
                //IQueryable<TablePortfolio> query = from p in db.TablePortfolio
                //                                   //join u in db.TableUser on p.userId equals u.Id
                //                                   where p.priceSell != 0
                //                                   where p.userId == userId
                //                                   orderby p.ticker
                //                                   select p;
                //return PortfolioUtils.ToDataTable<TablePortfolio>(query.ToList());

                var tablePortfolio = db.TablePortfolio
                    .Where(x => x.userId == userId);
                return PortfolioUtils.ToDataTable<TablePortfolio>(tablePortfolio.ToList());
            }
        }

        // Сохранение данных с построением графика в MS Excel
        private void OutInExcel(string title, DataTable dataTable)
        {
            // Создаём объект - экземпляр нашего приложения
            Excel.Application excelApp = new Excel.Application();
            // Создаём экземпляр рабочей книги Excel
            Excel.Workbook workBook;
            // Создаём экземпляр листа Excel
            Excel.Worksheet workSheet;
            workBook = excelApp.Workbooks.Add();
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);

            // Количество столбцов и строк
            int colCount = dataTable.Columns.Count;
            int rowCount = dataTable.Rows.Count;
            int colIndex = 0;

            // Вывод Названия отчета
            var cell = workSheet.Range["A2"];
            cell.Formula = title;
            cell.Style.Font.Size = 14;
            cell.Font.Bold = true;

            // Перенос данных на страницу Excel
            for (int col = 0; col < colCount; col++)
            {
                string columnType = dataTable.Columns[col].DataType.FullName;
                if (!columnType.StartsWith("System."))
                {
                    continue;
                }
                colIndex++;
                string columnName = dataTable.Columns[col].ColumnName;
                workSheet.Cells[4, colIndex] = columnName;
                for (int row = 0; row < rowCount; row++)
                {
                    Object test = dataTable.Rows[row].Field<Object>(columnName);
                    //TableStock tt = (TableStock) test;
                    workSheet.Cells[row + 5, colIndex] = dataTable.Rows[row].Field<Object>(columnName);
                }
            }

            var rangeTitle = workSheet.Range[workSheet.Cells[2, 1], workSheet.Cells[2, colIndex]];
            rangeTitle.Merge();

            // Оформление данных в таблицу
            var rangeTable = workSheet.Range[workSheet.Cells[4, 1], workSheet.Cells[rowCount + 4, colIndex]];
            rangeTable.Borders.LineStyle = 1;
            rangeTable.Style.Font.Size = 12;

            // Подгонка ширины столбцов "по содержимому"
            workSheet.Columns.AutoFit();

            // Открываем созданный excel-файл
            excelApp.Visible = true;
            excelApp.UserControl = true;
        }
    }
}

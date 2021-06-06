using System.Data.SqlClient;
using System.Data;
using System;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ControlDataBase
    {
        //создаем подключение
        static SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework");
        //static SqlConnection sqlConnection = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master; Integrated Security=True; Connect Timeout=30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private static DataTable sqlRun(string query)
        {
            // Открытие содинения
            sqlConnection.Open();
            // Создание SQL-адаптера для получения данных
            SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);
            // Закрытие соединения
            sqlConnection.Close();
            DataSet ds = new DataSet();
            // Преобразование данных
            adapter.Fill(ds);
            return ds.Tables[0];
        }

        // Запрос на удаление данных о секторе
        public static DataTable PortfolioSelect(Boolean isAllStock)
        {
            int userId = GlobalVariables.UserId;
            // Запрос на выборку данных
            string query = "SELECT tp.Id AS 'ИД записи'," +
                " tu.nikname AS 'Пользователь'," +
                " tp.ticker AS 'Тикер'," +
                " ts.name AS 'Название'," +
                " tp.count AS 'Количество'," +
                " tp.dateBue AS 'Дата покупки'," +
                " tp.priceBue AS 'Цена покупки'," +
                " tp.count * priceBue AS 'Сумма покупки'," +
                " tp.dateSell AS 'Дата продажи'," +
                " tp.priceSell AS 'Цена продажи'," +
                " CASE tp.priceSell WHEN 0 THEN 0 ELSE (tp.priceSell - tp.priceBue) * tp.count END AS 'Профит'" +
                " FROM TablePortfolio tp" +
                " JOIN TableUser tu ON tu.Id = tp.userId" +
                " JOIN TableStock ts ON ts.ticker = tp.ticker" +
                " WHERE tp.userId = " + userId;
            if (!isAllStock)
            {
                query += " AND tp.priceSell is not null";
            }
            return sqlRun(query);
        }

        // Формирование отчёта о выплаченных дивидендов 
        public static DataTable DividendSelectHistory()
        {
            int userId = GlobalVariables.UserId;
            // Запрос на выборку данных о дивидендах, по бумагам из портфеля (соответствие дат владения и выплат дивов)
            string query = "SELECT tp.ticker AS 'Тикер'," +
                " ts.name AS 'Название'," +
                " td.dateCutoff AS 'Дата отсечки'," +
                " td.dateRegisterClosing AS 'Дата закрытия реестра'," +
                " td.datePay AS 'Дата выплаты'," +
                " td.amount AS 'Сумма на 1 акцию'," +
                " tp.count AS 'Кол-во акций'," +
                " td.amount * tp.count AS 'Сумма'," +
                " td.profitability AS 'Доходность'" +
                " FROM TablePortfolio tp " +
                " JOIN TableStock ts ON ts.ticker = tp.ticker" +
                " JOIN TableDividends td ON td.ticker = tp.ticker AND td.dateCutoff < SYSDATETIME()" +
                " AND(tp.dateSell is null AND td.dateCutoff between tp.dateBue AND DATEADD(year, 1, SYSDATETIME())" +
                "  OR td.dateCutoff between tp.dateBue AND tp.dateSell)" +
                " WHERE tp.userId = " + userId +
                " ORDER BY dateCutoff DESC";
            return sqlRun(query);
        }

        // Формирование отчёта об ожидаемых дивидендов
        public static DataTable DividendSelectNext()
        {
            int userId = GlobalVariables.UserId;
            // Запрос на выборку данных о дивидендах, по бумагам из портфеля (соответствие дат владения и выплат дивов)
            string query = "SELECT tp.ticker AS 'Тикер'," +
                " ts.name AS 'Название'," +
                " td.dateCutoff AS 'Дата отсечки'," +
                " td.dateRegisterClosing AS 'Дата закрытия реестра'," +
                " td.datePay AS 'Дата выплаты'," +
                " td.amount AS 'Сумма на 1 акцию'," +
                " tp.count AS 'Кол-во акций'," +
                " td.amount * tp.count AS 'Сумма'," +
                " td.profitability AS 'Доходность' " +
                " FROM TablePortfolio tp " +
                " JOIN TableStock ts ON ts.ticker = tp.ticker" +
                " JOIN TableDividends td ON td.ticker = tp.ticker AND td.dateCutoff > SYSDATETIME()" +
                " WHERE tp.userId = " + userId +
                " AND tp.priceSell is null" +
                " ORDER BY dateCutoff DESC";
            return sqlRun(query);
        }

        //------------------------------------------------------------------------------
        // Запрос на добавление или обновление данных о секторе
        public static void DividendInsertOrUpdate(string ticker, DateTime dateCutoff, DateTime dateRegisterClosing, DateTime datePay, float amount, float profitability)
        {
            SqlCommand command = sqlConnection.CreateCommand();
            command.CommandText = @"IF EXISTS(SELECT * FROM TableDividends WHERE ticker = @ticker AND dateCutoff = @dateCutoff)
                        UPDATE TableDividends
                        SET dateRegisterClosing = @dateRegisterClosing, datePay = @datePay, amount = @amount, profitability = @profitability
                        WHERE ticker = @ticker AND dateCutoff = @dateCutoff
                    ELSE
                        INSERT INTO TableDividends(ticker, dateCutoff, dateRegisterClosing, datePay, amount, profitability) VALUES(@ticker, @dateCutoff, @dateRegisterClosing, @datePay, @amount, @profitability);";

            command.Parameters.Add("ticker", SqlDbType.NVarChar).Value = ticker;
            command.Parameters.Add("dateCutoff", SqlDbType.Date).Value = dateCutoff;
            command.Parameters.Add("dateRegisterClosing", SqlDbType.Date).Value = dateRegisterClosing;
            command.Parameters.Add("datePay", SqlDbType.Date).Value = datePay;
            command.Parameters.Add("amount", SqlDbType.Float).Value = amount;
            command.Parameters.Add("profitability", SqlDbType.Float).Value = profitability;
            sqlConnection.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                sqlConnection.Close();
            }
        }
    }
}
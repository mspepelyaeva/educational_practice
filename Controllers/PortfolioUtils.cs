using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public static class PortfolioUtils
    {
        public static async Task getContent(string ticker)
        {
            // Клиент для запроса данных с сервера
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.nasdaq.com/api/quote/{ticker}/dividends?assetclass=stocks";
                // Конфигурация клиента
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Выполнение запроса
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                // Если запрос выполнился успешно
                if (response.IsSuccessStatusCode)
                {
                    // Преобразование контента (строки) с JSON (объект)
                    var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    JObject json = JObject.Parse(str);

                    // Количество записей
                    int rowCount = json.SelectToken("data.dividends.rows").Count();
                    for (int index = 0; index < rowCount; index++)
                    {
                        Object objAnnualizedDividend = json.SelectToken("data.annualizedDividend");
                        Object objAmount = json.SelectToken("data.dividends.rows[" + index + "].amount");
                        Object objExOrEffDate = json.SelectToken("data.dividends.rows[" + index + "].exOrEffDate");
                        Object objRecordDate = json.SelectToken("data.dividends.rows[" + index + "].recordDate");
                        Object objPaymentDate = json.SelectToken("data.dividends.rows[" + index + "].paymentDate");
                        // 
                        float amount = 0;
                        float profitability = 0;
                        DateTime exOrEffDate = DateTime.MinValue;
                        DateTime recordDate = DateTime.MinValue;
                        DateTime paymentDate = DateTime.MinValue;
                        if (objAnnualizedDividend != null)
                        {
                            profitability = float.Parse(objAnnualizedDividend.ToString().Replace(".", ","));
                        }
                        if (objAmount != null)
                        {
                            amount = float.Parse(objAmount.ToString().Replace("$", "").Replace(".", ","));
                        }
                        if (objExOrEffDate != null)
                        {
                            exOrEffDate = DateTime.ParseExact(objExOrEffDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (objRecordDate != null)
                        {
                            recordDate = DateTime.ParseExact(objRecordDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        if (objPaymentDate != null)
                        {
                            paymentDate = DateTime.ParseExact(objPaymentDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        }
                        // Собираем данные не ранее начала 2020 года
                        if (exOrEffDate.CompareTo(DateTime.Parse("01/01/2020")) > 0)
                        {
                            ControlDataBase.DividendInsertOrUpdate(ticker, exOrEffDate, recordDate, paymentDate, amount, profitability);
                        }
                    }
                }
            }
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
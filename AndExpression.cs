using System.Data;

namespace LoadCustomeCSVFile
{
    public class AndExperesion
    {
        public DataTable Import<T>(string csvFile, string first, string second)
        {
            List<T> list = new List<T>();
            var lines = System.IO.File.ReadAllLines(csvFile, System.Text.Encoding.UTF8);
            var headerLine = lines.First();
            int i = lines.Count();
            DataTable table = new DataTable();

            var columns = headerLine.Split(',').ToList().Select((v, i) => new { Position = i, Name = v });
            foreach (var item in columns)
            {
                table.Columns.Add(item.Name);
            }
            var dataLines = lines.Skip(1).ToList();
            foreach (var item in dataLines)
            {
                var line = item.Split(',');
                table.Rows.Add(line);

            }

            var firstExpression = table;

            string expression;
            string sortOrder;

            expression = $"{first} and {second}";
            sortOrder = "id DESC";

            DataRow[] rows = firstExpression.Select(expression, sortOrder);

            firstExpression = null; // for testing
            firstExpression = new DataTable(); // for testing
            var columns2 = headerLine.Split(',').ToList().Select((v, i) => new { Position = i, Name = v });
            foreach (var item in columns2)
            {
                firstExpression.Columns.Add(item.Name);
            }

            foreach (DataRow row in rows)
            {
                string tempRowData = "";
                foreach (string item in row.ItemArray)
                {
                    tempRowData += item + ",";

                }

                var cleanedRow = tempRowData.Remove(tempRowData.ToString().LastIndexOf(','), 1);
                var SplittedRow = cleanedRow.Split(',');
                firstExpression.Rows.Add(SplittedRow);
            }
            return firstExpression;
        }

        public List<T> DataTableToList<T>(System.Data.DataTable table) where T : new()
        {
            List<T> list = new List<T>();
            var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
            {
                PropertyInfo = propertyInfo,
                Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
            }).ToList();

            foreach (var row in table.Rows.Cast<DataRow>())
            {
                T obj = new T();
                foreach (var typeProperty in typeProperties)
                {
                    object value = row[typeProperty.PropertyInfo.Name];
                    object safeValue = value == null || DBNull.Value.Equals(value)
                        ? null
                        : Convert.ChangeType(value, typeProperty.Type);

                    typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
                }
                list.Add(obj);
            }
            return list;
        }
    }


}
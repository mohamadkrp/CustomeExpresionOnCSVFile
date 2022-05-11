using System.Data;
using LoadCustomeCSVFile;

string file = @"C:\MyRepo\LoadCustomeCSVFile\LoadCustomeCSVFile\inputData.csv";
var dataEntry = new AndExperesion();

var equal = Eq("gender", "1");
var greaterThan = Gd("height", "160");
var data = dataEntry.Import<string>(file, equal, greaterThan);

var tempHeaderData = "";
foreach (DataColumn dataColumn in data.Columns)
{
    tempHeaderData += dataColumn.ColumnName + ",";
}
var headerData = tempHeaderData.Remove(tempHeaderData.ToString().LastIndexOf(','), 1);
var allRows = new List<string>();
foreach (DataRow dataRow in data.Rows)
{
    var stringData = "";
    foreach (var item in dataRow.ItemArray)
    {
        stringData += item + ",";
    }
    var rowData = stringData.Remove(stringData.ToString().LastIndexOf(','), 1);
    allRows.Add(rowData);
}

var allHeaders = headerData.Split(',');
var outPut = "";
for(int i = 0; i < allRows.Count();i++)
{
    var row = allRows[i].Split(',');
    outPut += "{ ";
    for(int j=0;j<allHeaders.Count();j++)
    {
        outPut += $"{allHeaders[j]} : {row[j]} ";
        if(j != (allHeaders.Count() -1))
        outPut += " , " ;
    }
    outPut += " }";
    if(i != (allRows.Count() -1))
        outPut += " , " ;
}

Console.WriteLine(outPut);

string Eq(string key, string value)
{
    return key + " = " + $"'{value}'";
}

string Gd(string key, string value)
{
    return key + " > " + $"'{value}'";
}

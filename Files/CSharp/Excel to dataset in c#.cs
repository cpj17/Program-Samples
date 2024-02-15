using System;
using System.Data;
using System.Data.OleDb;

namespace ExcelToDataSet
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //this excel have 2 sheets main one is sheet
                string stringPath = @"C:\Users\GSIAD-031\Desktop\Portal Data required v1.xlsx";

                string filePath = stringPath;
                string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1;\"";
                // The "HDR=YES" option indicates that the first row of the worksheet contains column headers.
                // The "IMEX=1" option tells the provider to always read intermixed data types as text.

                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();

                DataTable schemaTable = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();
                sheetName = schemaTable.Rows[1]["TABLE_NAME"].ToString(); //here index 1 is sheet1
                //sheetName = "Sheet1$";
                // This gets the name of the first worksheet in the workbook.

                OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", connection);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                connection.Close();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

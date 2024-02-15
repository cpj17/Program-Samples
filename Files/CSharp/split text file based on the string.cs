using System;
using System.IO;
using System.Data;
using System.Linq;

namespace Test002
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataTable objDataTable = new DataTable();
            objDataTable.Columns.Add("Time", typeof(string));
            objDataTable.Columns.Add("msCallingApp", typeof(string));
            objDataTable.Columns.Add("wsPatientID", typeof(string));
            Console.Title = "Made By CPJ";
            Console.WriteLine("Enter Path");
            string filePath = Console.ReadLine();
            string[] allLines = File.ReadAllLines(filePath);
            int n = allLines.Length + 1;
            int loopcount = n / 10;

            string[] arr = allLines.Take(10).ToArray();

            DataRow objRow = objDataTable.NewRow();
            objRow["Time"] = arr[1];
            objRow["msCallingApp"] = arr[7].Replace("<msCallingApp>", "").Replace("</msCallingApp>", "").Trim();
            objRow["wsPatientID"] = arr[8].Replace("<wsPatientID>", "").Replace("</wsPatientID>", "").Trim();
            objDataTable.Rows.Add(objRow);

            for (int i = 1; i < loopcount; i++)
            {
                DataRow row = objDataTable.NewRow();
                string[] arr2 = allLines.Skip(i * 10).Take(10).ToArray();

                row["Time"] = arr2[1];
                row["msCallingApp"] = arr2[7].Replace("<msCallingApp>", "").Replace("</msCallingApp>", "").Trim();
                row["wsPatientID"] = arr2[8].Replace("<wsPatientID>", "").Replace("</wsPatientID>", "").Trim();
                objDataTable.Rows.Add(row);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(objDataTable);
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ds.WriteXml(documentsPath + "/result.xml");
            Console.WriteLine("export successfully");
            Console.WriteLine("press enter to exit");
            Console.ReadKey();
        }
    }
}

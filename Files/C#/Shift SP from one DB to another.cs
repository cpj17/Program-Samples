using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string[] objList = File.ReadAllLines("D:/objList.txt");

                foreach (string obj in objList)
                {
                    CreateSpAndView(obj);
                    //CreateTvp(obj);
                }

                Console.WriteLine("Done!!!");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateSpAndView(string stringObj)
        {
            SqlConnection sqlConnectionHRData = new SqlConnection("Server=192.168.2.138;Database=HRData;User Id=sa;Password=Nabfins@123;");
            SqlConnection sqlConnectionHR = new SqlConnection("Server=192.168.2.138;Database=HR;User Id=sa;Password=Nabfins@123;");
            try
            {
                string stringQuery1 = $"EXEC sp_helptext '{stringObj}' ";
                SqlCommand sqlCommand = new SqlCommand(stringQuery1, sqlConnectionHRData);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlConnectionHRData.Open();

                StringBuilder objText = new StringBuilder();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                        objText.Append(reader.GetString(0));
                }

                string stringQuery = objText.ToString();
                sqlConnectionHRData.Close();

                sqlCommand = new SqlCommand(stringQuery, sqlConnectionHR);
                sqlConnectionHR.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnectionHR.Close();

                Console.WriteLine($"{stringObj} is copied");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateTvp(string stringObj)
        {
            SqlConnection sqlConnectionHRData = new SqlConnection("Server=192.168.2.138;Database=HRData;User Id=sa;Password=Nabfins@123;");
            SqlConnection sqlConnectionHR = new SqlConnection("Server=192.168.2.138;Database=HR;User Id=sa;Password=Nabfins@123;");
            try
            {
                string stringQuery1 = $@"
                            DECLARE @TableTypeName NVARCHAR(128) = '{stringObj}'; -- Replace with your table type name
                            DECLARE @SQL NVARCHAR(MAX) = '';

                            -- Start the CREATE TYPE statement
                            SET @SQL = 'CREATE TYPE ' + @TableTypeName + ' AS TABLE (';

                            -- Add column definitions
                            SELECT @SQL = @SQL + CHAR(13) + CHAR(10) + 
                                    '    ' + c.name + ' ' + t.name + 
                                    CASE 
                                        WHEN t.name IN ('varchar', 'char', 'nvarchar', 'nchar') THEN 
                                            '(' + 
                                                CASE 
                                                    WHEN c.max_length = -1 THEN 'MAX'
                                                    ELSE CAST(c.max_length AS VARCHAR(5))
                                                END		
                                            + ')'
                                        WHEN t.name IN ('decimal', 'numeric') THEN 
                                            '(' + CAST(c.precision AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'
                                        ELSE ''
                                    END +
                                    CASE 
                                        WHEN c.is_nullable = 1 THEN ' NULL'
                                        ELSE ' NOT NULL'
                                    END + ',' 
                            FROM sys.table_types AS tt
                            INNER JOIN sys.columns AS c ON tt.type_table_object_id = c.object_id
                            INNER JOIN sys.types AS t ON c.user_type_id = t.user_type_id
                            WHERE tt.name = @TableTypeName
                            ORDER BY c.column_id;

                            -- Remove the trailing comma and add closing parenthesis
                            SET @SQL = LEFT(@SQL, LEN(@SQL) - 1) + ');';

                            -- Output the generated CREATE TYPE statement
                            SELECT @SQL;

                ";
                SqlCommand sqlCommand = new SqlCommand(stringQuery1, sqlConnectionHRData);
                sqlCommand.CommandType = System.Data.CommandType.Text;
                sqlConnectionHRData.Open();

                StringBuilder objText = new StringBuilder();

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                        objText.Append(reader.GetString(0));
                }

                string stringQuery = objText.ToString();
                sqlConnectionHRData.Close();

                sqlCommand = new SqlCommand(stringQuery, sqlConnectionHR);
                sqlConnectionHR.Open();
                sqlCommand.ExecuteNonQuery();
                sqlConnectionHR.Close();

                Console.WriteLine($"{stringObj} is copied");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

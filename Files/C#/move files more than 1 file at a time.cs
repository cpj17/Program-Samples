using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    // Replace these paths with your actual paths
        //    string firstPath = @"C:\Users\Praveen\Desktop\a";
        //    string secondPath = @"C:\Users\Praveen\Desktop\list.txt";
        //    string newFolderName = "MovedFiles";

        //    // Create the new folder in the first path
        //    string newFolderPath = Path.Combine(firstPath, newFolderName);
        //    Directory.CreateDirectory(newFolderPath);

        //    // Read all lines from the text file
        //    string[] filesToFind = File.ReadAllLines(secondPath);

        //    // Search for each file in the first path and its subfolders
        //    foreach (string fileToFind in filesToFind)
        //    {
        //        string[] foundFiles = Directory.GetFiles(firstPath, fileToFind, SearchOption.AllDirectories);

        //        foreach (string foundFile in foundFiles)
        //        {
        //            string fileName = Path.GetFileName(foundFile);
        //            string destinationPath = Path.Combine(newFolderPath, fileName);

        //            // Move the file to the new folder
        //            File.Move(foundFile, destinationPath);
        //            Console.WriteLine($"Moved: {foundFile} to {destinationPath}");
        //        }
        //    }

        //    Console.WriteLine("Operation completed.");
        //    Console.ReadKey();
        //}

        //static void Main(string[] args)
        //{
        //    // Replace these paths with your actual paths
        //    string firstPath = @"C:\Users\Praveen\Desktop\a";
        //    string secondPath = @"C:\Users\Praveen\Desktop\list.txt";
        //    string newFolderName = "MovedFiles";

        //    // Create the new folder in the first path
        //    string newFolderPath = Path.Combine(firstPath, newFolderName);
        //    Directory.CreateDirectory(newFolderPath);

        //    // Read all lines from the text file
        //    string[] filesToFind = File.ReadAllLines(secondPath);

        //    // Get all files in the first path and its subfolders
        //    var allFiles = Directory.GetFiles(firstPath, "*.*", SearchOption.AllDirectories);

        //    // Search for each file in the first path and its subfolders
        //    foreach (string fileToFind in filesToFind)
        //    {
        //        // Find files where the name (without extension) matches
        //        var matchingFiles = allFiles
        //            .Where(file => Path.GetFileNameWithoutExtension(file)
        //                          .Equals(fileToFind, StringComparison.OrdinalIgnoreCase))
        //            .ToList();

        //        foreach (string foundFile in matchingFiles)
        //        {
        //            string fileName = Path.GetFileName(foundFile);
        //            string destinationPath = Path.Combine(newFolderPath, fileName);

        //            // Move the file to the new folder
        //            File.Move(foundFile, destinationPath);
        //            Console.WriteLine($"Moved: {fileName}");
        //        }
        //    }

        //    Console.WriteLine("Operation completed.");
        //}

        static void Main(string[] args)
        {
            // Replace these paths with your actual paths
            string firstPath = @"C:\Users\Praveen\Desktop\a";
            string secondPath = @"C:\Users\Praveen\Desktop\list.txt";
            string newFolderName = "MovedFiles";

            // Create the new folder in the first path
            string newFolderPath = Path.Combine(firstPath, newFolderName);
            Directory.CreateDirectory(newFolderPath);

            // Read all lines from the text file
            string[] filesToFind = File.ReadAllLines(secondPath);

            // Get all files in the first path and its subfolders
            var allFiles = Directory.GetFiles(firstPath, "*.*", SearchOption.AllDirectories);

            // Search for each file in the first path and its subfolders
            foreach (string fileToFind in filesToFind)
            {
                // Find files where the name (without extension) matches
                var matchingFiles = allFiles
                    .Where(file => Path.GetFileNameWithoutExtension(file)
                                  .Equals(fileToFind, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (matchingFiles.Any())
                {
                    foreach (string foundFile in matchingFiles)
                    {
                        string fileName = Path.GetFileName(foundFile);
                        string destinationPath = Path.Combine(newFolderPath, fileName);

                        // Move the file to the new folder
                        File.Move(foundFile, destinationPath);
                        Console.WriteLine($"Moved: {fileName}");
                    }
                }
                else
                {
                    // Log a message if the file is not found
                    //Console.WriteLine($"File '{fileToFind}' not found in '{firstPath}'.");
                    Console.WriteLine($"File '{Path.GetFileName(fileToFind)}' not found in '{firstPath}'.");
                }
            }

            Console.WriteLine("Operation completed.");
        }
    }
}

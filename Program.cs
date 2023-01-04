using System;
using System.IO;
using System.IO.Compression;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Enter the path of the file to compress
            Console.WriteLine("Enter file to compress path:");
            string fileToCompress = Console.ReadLine();
            Console.WriteLine("File to compress path: " + fileToCompress);
            
            // Just create a list with the path of all files to compress
            List<string> filesToCompress = new List<string> { fileToCompress };

            // Enter the path of the destination zip file
            Console.WriteLine("Enter destination zip file path:");
            string zipFilePath = Console.ReadLine();
            Console.WriteLine("Destination zip file path: " + zipFilePath);


            string errorMessage;
            bool status = CompressFiles(filesToCompress, zipFilePath, out errorMessage);

            if (status)
            {
                Console.WriteLine("\n\nCompression complete with sucess.");
            }
            else
            {
                Console.WriteLine("\n\nCompression complete with errors.");
                Console.WriteLine(errorMessage);
            }
        }

        /// <summary>
        /// Creates a zip file with all files in the list
        /// </summary>
        /// <param name="filesToCompressPaths">A list with the path of all files we want to compress</param>
        /// <param name="zipFileToCreatePath">A path to were we want the zip file to be saved</param>
        /// <returns>A boolean indicating the status of the compression</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static bool CompressFiles(List<string> filesToCompressPaths, string zipFileToCreatePath, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (filesToCompressPaths is null) 
            { 
                throw new System.ArgumentNullException(nameof(filesToCompressPaths));
            }

            if (zipFileToCreatePath is null)
            {
                throw new System.ArgumentNullException(nameof(zipFileToCreatePath));
            }

            if (filesToCompressPaths.Count == 0)
            {
                errorMessage = "Provided an empty list of files to compress";
                return false;
            }

            if (Path.GetExtension(zipFileToCreatePath) != ".zip")
            {
                errorMessage = "Provided an valid path for the zip to create";
                return false;
            }

            try
            {
                // Create a zip archive object
                using (ZipArchive zipArchive = ZipFile.Open(zipFileToCreatePath, ZipArchiveMode.Create))
                {
                    // iterate over the list of files to add to this zip archive
                    foreach (string file in filesToCompressPaths)
                    {
                        using (FileStream originalFileStream1 = File.OpenRead(file))
                        {
                            ZipArchiveEntry zipEntry1 = zipArchive.CreateEntry(Path.GetFileName(file));
                            using (Stream zipEntryStream1 = zipEntry1.Open())
                            {
                                originalFileStream1.CopyTo(zipEntryStream1);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return false;
            }

            return true;
        }
    }
}
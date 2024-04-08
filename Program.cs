namespace console_split_file;

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Splitting a large log file:");

        if (args.Length < 1)
        {
            Console.WriteLine("Specify the path to the large text file.");
            return;
        }

        string filePath = args[0];
        int linesPerFile = 50000; 

        if (args.Length >= 2 && !int.TryParse(args[1], out linesPerFile))
        {
            Console.WriteLine("Invalid number of lines per file. Using default of 50000.");
        }
        try
        {
            SplitFile(filePath, linesPerFile);
            Console.WriteLine("Splitting completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static void SplitFile(string filePath, int linesPerFile)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("The specified file was not found.", filePath);
        }

        string directory = Path.GetDirectoryName(filePath);
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
        string extension = Path.GetExtension(filePath);

        using (var reader = new StreamReader(filePath))
        {
            int filePartNumber = 0;
            while (!reader.EndOfStream)
            {
                filePartNumber++;
                string partFileName = Path.Combine(directory, $"{fileNameWithoutExtension}_{filePartNumber}{extension}");
                using (var writer = new StreamWriter(partFileName))
                {
                    for (int i = 0; i < linesPerFile; i++)
                    {
                        if (reader.EndOfStream)
                            break;

                        string line = reader.ReadLine();
                        writer.WriteLine(line);

                        UpdateProgressBar(i + 1, linesPerFile);
                    }
                }
                Console.WriteLine();
            }
        }
    }
    static void UpdateProgressBar(int currentLine, int totalLines)
    {
        int barWidth = 40;
        
        double progressFraction = (double)currentLine / totalLines;
        
        int progressBlocks = (int)(progressFraction * barWidth);

        string progressBar = "[" + new string('=', progressBlocks) + new string(' ', barWidth - progressBlocks) + "]";

        int percentage = (int)(progressFraction * 100);

        Console.Write($"\r{progressBar} {percentage}%");
    }
}

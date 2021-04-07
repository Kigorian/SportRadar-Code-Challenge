using Newtonsoft.Json.Linq;
using NHL_API.Resources.Enums;
using System;
using System.IO;

namespace NHL_API.Services
{
    public class ConsoleService
    {
        #region Get From User

        /// <summary>
        /// Asks the user for the pipeline type using the console.
        /// </summary>
        /// <returns></returns>
        public static PipelineType GetPipelineType()
        {
            while (true)
            {
                WriteParagraph("Would you like to query for Team or Player results?");
                var pipelineTypeChoice = Console.ReadLine();

                switch (pipelineTypeChoice.ToLower().Trim().TrimEnd('s'))
                {
                    case "team":
                        return PipelineType.Team;

                    case "player":
                        return PipelineType.Player;

                    default:
                        WriteParagraphInColor(
                            $"\"{pipelineTypeChoice}\" is not a valid option.",
                            ConsoleColor.Red
                        );
                        break;
                }
            }
        }

        /// <summary>
        /// Prompts the user for the ID of the <see cref="Model.Team"/> or <see cref="Model.Player"/>
        /// that they wish to search.
        /// </summary>
        /// <param name="pipelineTypeDescription"></param>
        /// <returns></returns>
        public static int GetEntityId(string pipelineTypeDescription)
        {
            int id;

            while (true)
            {
                WriteParagraph(
                    $"Please enter the ID for the {pipelineTypeDescription} " +
                    $"you would like to view."
                );
                var userInput = Console.ReadLine();

                if (int.TryParse(userInput, out id) && id > 0)
                {
                    break;
                }
                else
                {
                    WriteParagraphInColor(
                        $"\"{userInput}\" is not a valid option. " +
                        $"Please enter the ID as a positive integer.",
                        ConsoleColor.Red
                    );
                }
            }

            return id;
        }

        /// <summary>
        /// Prompts the user for a year; this also hits the API to find the current season,
        /// and won't allow year selections past the current season.
        /// </summary>
        /// <returns></returns>
        public static int GetYear()
        {
            // Set a minimum boundary.
            var minYear = 1900;

            // Find the year of the current season, and use it as the maximum boundary.
            var currentSeason = ApiService.GetSeasonData();
            var currentSeasonStartYear = currentSeason.StartYear;

            int year;
            while (true)
            {
                WriteParagraph("Which year would you like to view?");
                var userInput = Console.ReadLine();

                if (
                    int.TryParse(userInput, out year)
                    && year >= minYear
                    && year <= currentSeasonStartYear
                )
                {
                    break;
                }
                else
                {
                    WriteParagraphInColor(
                        $"\"{userInput}\" is not a valid option. " +
                        $"Please enter a valid year between {minYear} and {currentSeasonStartYear}",
                        ConsoleColor.Red
                    );
                }
            }

            return year;
        }

        /// <summary>
        /// Prompts the user for a directory path, and checks to see if there is a valid
        /// directory at that path.
        /// </summary>
        /// <returns></returns>
        public static string GetDirectoryPath()
        {
            string path;

            while (true)
            {
                WriteParagraph("Please specify the directory to save the file to.");
                path = Console.ReadLine();

                if (Directory.Exists(path))
                {
                    break;
                }
                else
                {
                    WriteParagraphInColor(
                        $"No directory found at \"{path}\". " +
                        $"Please enter the path to an existing directory.",
                        ConsoleColor.Red
                    );
                }
            }

            return path;
        }

        /// <summary>
        /// Prompts the user for a file name, and checks to see if there is an existing CSV file
        /// with that name.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static string GetCsvFileName(string directoryPath)
        {
            string name;

            while (true)
            {
                WriteParagraph("Choose a name for your file (without the extension).");
                name = Console.ReadLine();

                if (File.Exists($"{directoryPath}/{name}.csv"))
                {
                    WriteParagraph(
                        "A file with that name already exists. " +
                        "Do you wish to overwrite it? (Y/N)"
                    );
                    var isOverwrite = Console.ReadLine();

                    if (isOverwrite.ToLower() == "y" || isOverwrite.ToLower() == "yes")
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
            }

            return $"{name}.csv";
        }

        /// <summary>
        /// Asks the user if they would like to run the program again from the beginning.
        /// </summary>
        /// <returns></returns>
        public static bool GetTryAgainChoice(bool success = false)
        {
            var tryAgainMessage = success
                ? "File saved successfully. Import another file? (Y/N)"
                : "Try again from the beginning? (Y/N)";

            WriteParagraph(tryAgainMessage);

            var again = Console.ReadLine();

            return again.ToLower() == "y" || again.ToLower() == "yes";
        }

        #endregion Get From User

        #region Console Writes

        /// <summary>
        /// Prints a new empty line, then a new line with the given value.
        /// </summary>
        public static void WriteParagraph(string value)
        {
            Console.WriteLine();
            Console.WriteLine(value);
        }

        /// <summary>
        /// Calls Console.WriteLine(), but prints in the specified color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
        public static void WriteLineInColor(string value, ConsoleColor color)
        {
            // Keep track of original color so we can set it again.
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = originalColor;
        }

        /// <summary>
        /// Prints a new empty line, then calls WriteLineInColor().
        /// </summary>
        /// <param name="color"></param>
        /// <param name="message"></param>
        public static void WriteParagraphInColor(string value, ConsoleColor color)
        {
            Console.WriteLine();
            WriteLineInColor(value, color);
        }

        public static void PrintException(Exception e, string customMessage = null)
        {
            if (!string.IsNullOrWhiteSpace(customMessage))
            {
                WriteParagraphInColor(
                    customMessage,
                    ConsoleColor.Red
                );
            }
            WriteLineInColor(
                e.Message,
                ConsoleColor.Red
            );
        }

        #endregion Console Writes
    }
}

using NHL_API.Model;
using NHL_API.Resources.Attributes;
using NHL_API.Resources.Enums;
using NHL_API.Services;
using System;
using System.Collections.Generic;
using System.IO;

namespace NHL_API
{
    class UserInputToCsvLoop
    {
        public static bool RunProgramLoop()
        {
            // Get the user's pipeline type choice.
            var pipelineType = ConsoleService.GetPipelineType();
            var pipelineTypeDescription = AttributeHelper.GetDescription(pipelineType);

            // Get the ID and year from the user.
            var entityId = ConsoleService.GetEntityId(pipelineTypeDescription);
            var year = ConsoleService.GetYear();

            // Query the API for the requested information, and serialize the results into a CSV string.
            string csv;
            switch (pipelineType)
            {
                case PipelineType.Team:
                    var teamResult = ApiService.TryGetTeamData(entityId, year);
                    if (teamResult.HasError)
                    {
                        ConsoleService.PrintException(
                            teamResult.Exception,
                            "Failed to get Team data. Error details:"
                        );

                        // Prompt to start the program loop over.
                        return ConsoleService.GetTryAgainChoice();
                    }
                    else
                    {
                        // If there was no error, generate the CSV.
                        csv = CsvService.ToCsv(new List<Team>() { (Team)teamResult.EntityResult });
                        break;
                    }

                case PipelineType.Player:
                    var playerResult = ApiService.TryGetPlayerData(entityId, year);
                    if (playerResult.HasError)
                    {
                        ConsoleService.PrintException(
                            playerResult.Exception,
                            "Failed to get Player data. Error details:"
                        );

                        // Start the program loop over.
                        return ConsoleService.GetTryAgainChoice();
                    }
                    else
                    {
                        // If there was no error, generate the CSV.
                        csv = CsvService.ToCsv(new List<Player>() { (Player)playerResult.EntityResult });
                        break;
                    }

                default:
                    ConsoleService.WriteParagraphInColor(
                        "Error getting pipeline. Please try again.",
                        ConsoleColor.Red
                    );

                    return ConsoleService.GetTryAgainChoice();
            }

            // Get the file path info from the user.
            var directoryPath = ConsoleService.GetDirectoryPath();
            var fileName = ConsoleService.GetCsvFileName(directoryPath);
            var filePath = $"{directoryPath}/{fileName}";

            // Save the CSV file.
            try
            {
                File.WriteAllText(filePath, csv);
            }
            catch(Exception e)
            {
                ConsoleService.PrintException(e, "Error saving file. Details:");
                return ConsoleService.GetTryAgainChoice();
            }

            // If all went well, print a success message and ask the user if they'd like
            // to generate another file.
            return ConsoleService.GetTryAgainChoice(success: true);
        }
    }
}

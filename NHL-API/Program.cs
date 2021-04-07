using NHL_API.Model;
using NHL_API.Resources.Attributes;
using NHL_API.Resources.Enums;
using NHL_API.Services;
//using NHL_API.Resources.JsonConverters;
using System.Collections.Generic;
using System.IO;

namespace NHL_API
{
    class Program
    {
        static void Main(string[] args)
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
                    var team = ApiService.GetTeamData(entityId, year);
                    csv = CsvService.ToCsv(new List<Team>() { team });
                    break;

                case PipelineType.Player:
                    var player = ApiService.GetPlayerData(entityId, year);
                    csv = CsvService.ToCsv(new List<Player>() { player });
                    break;

                default:
                    return;
            }

            // Get the file path info from the user.
            var directoryPath = ConsoleService.GetDirectoryPath();
            var fileName = ConsoleService.GetCsvFileName(directoryPath);
            var filePath = $"{directoryPath}/{fileName}";
            
            // Save the CSV file.
            File.WriteAllText(filePath, csv);
        }
    }
}

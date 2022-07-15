using CsvHelper;
using PetroineosChallenage.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    public class CSVDataWriter: IDataWriter
    {
        private readonly OutputSettings _outputSettings;
        private readonly ILogger _logger;

        public CSVDataWriter(ILogger<Worker> logger, OutputSettings outputSettings)
        {
            _outputSettings = outputSettings;
            _logger = logger;
            _logger.LogInformation($"Current output settings {_outputSettings.ToJsonString()}");
        }
        
        public async Task WriteAsync<T>(IEnumerable<T> records)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmm");
            var outputFilePath = Path.Combine(_outputSettings.Location, $"PowerPosition_{timestamp}.csv");

            _logger.LogInformation($"Start writing {outputFilePath}");
            using (var writer = new StreamWriter(outputFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await csv.WriteRecordsAsync<T>(records);
            }
            _logger.LogInformation($"Finished writing {outputFilePath}");
        }
    }
}

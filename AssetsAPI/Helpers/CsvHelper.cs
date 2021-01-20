using AssetsAPI.Mappers;
using AssetsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace AssetsAPI.Helpers
{
    public class CsvHelper
    {
        public static List<AssetCsvResponseModel> ReadCsv(string csvName)
        {
            try
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                var csvParser = new CsvParser<AssetCsvResponseModel>(csvParserOptions, new CsvAssetMapping());

                return csvParser.ReadFromFile(csvName, Encoding.UTF8)
                                .Select(x => x.Result)
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}

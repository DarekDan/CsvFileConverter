using System.Dynamic;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace Fiserv;

internal class Util
{
    protected internal static async Task ProcessStreams(StreamReader reader, StreamWriter writer, ConversionConfig config)
    {
        using var csvReader = new CsvReader(reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
                { Delimiter = config.InputDelimiter, HasHeaderRecord = config.HasHeader });
        await using var csvWriter = new CsvWriter(writer,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = config.OutputDelimiter, HasHeaderRecord = config.HasHeader,
                NewLine = config.IncludeCrInLineEnding ? "\r\n" : "\n"
            });
        if (config.RemoveNonPrintableCharacters)
        {
            foreach (dynamic record in csvReader.GetRecords<dynamic>())
            {
                IDictionary<string, object> propValues = record;
                var expando = new ExpandoObject();
                var dictionary = expando as IDictionary<string, object>;
                foreach (var prop in propValues.Keys)
                {
                    dictionary.Add(prop, ((string)propValues[prop]).Cleanse());
                }

                csvWriter.WriteRecord(expando);
            }
        }
        else
        {
            await csvWriter.WriteRecordsAsync(csvReader.GetRecords<dynamic>());
        }
    }
}
using System.CommandLine;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace Fiserv;

static class Program
{
    static async Task<int> Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var inputFileOption = new Option<FileInfo?>(aliases: new []{"-i","--inputFile"}, description: "The file to read from");
        var inputEncodingOption = new Option<string>(aliases: new []{"-ie", "--inputEncoding"}, getDefaultValue: () => "utf-8");
        var inputDelimiterOption = new Option<string>(aliases: new []{"-id", "--inputDelimiter"}, getDefaultValue: () => "\t");
        var outputFileOption = new Option<FileInfo?>(aliases: new []{"-o","--outputFile"}, description: "The file to write to");
        var outputEncodingOption = new Option<string>(aliases: new []{"-oe","--outputEncoding"}, getDefaultValue: () => "Windows-1252");
        var outputDelimiterOption = new Option<string>(aliases: new []{"-od","--outputDelimiter"}, getDefaultValue: () => ",");
        var useHeaderRecordsOption = new Option<bool>(aliases: new []{"-hdr","--hasHeaderRow"}, getDefaultValue: () => true);
        var rootCommand = new RootCommand("FileConverter Utility")
        {
            inputFileOption,
            inputEncodingOption,
            inputDelimiterOption,
            outputFileOption,
            outputEncodingOption,
            outputDelimiterOption,
            useHeaderRecordsOption
        };
        rootCommand.SetHandler(async (inFile, outFile, inEnc, outEnc, inDeli, outDeli, hasHeader) =>
            {
                var inputEncoding = Encoding.GetEncoding(inEnc);
                var outputEncoding = Encoding.GetEncoding(outEnc);
                using var reader = new StreamReader(inFile.FullName, inputEncoding, true);
                using var csvReader = new CsvReader(reader,
                    new CsvConfiguration(CultureInfo.InvariantCulture)
                        { Delimiter = inDeli, HasHeaderRecord = hasHeader });
                await using var writer = new StreamWriter(outFile.FullName, false, outputEncoding);
                await using var csvWriter = new CsvWriter(writer,
                    new CsvConfiguration(CultureInfo.InvariantCulture)
                        { Delimiter = outDeli, HasHeaderRecord = hasHeader });
                await csvWriter.WriteRecordsAsync(csvReader.GetRecords<dynamic>());
            }, inputFileOption,
            outputFileOption, inputEncodingOption, outputEncodingOption, inputDelimiterOption, outputDelimiterOption,
            useHeaderRecordsOption);
        return await rootCommand.InvokeAsync(args);
    }
}
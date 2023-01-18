using System.CommandLine;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("CsvFileConverter.Tests")]

namespace Fiserv;

static class Program
{
    static async Task<int> Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var inputFileOption =
            new Option<FileInfo?>(aliases: new[] { "-i", "--inputFile" }, description: "The file to read from")
                { IsRequired = true };
        var inputEncodingOption =
            new Option<string>(aliases: new[] { "-ie", "--inputEncoding" }, getDefaultValue: () => "utf-8");
        var inputDelimiterOption =
            new Option<string>(aliases: new[] { "-id", "--inputDelimiter" }, getDefaultValue: () => "\t",
                description: "The default input delimiter is TAB, 0x09");
        var outputFileOption =
            new Option<FileInfo?>(aliases: new[] { "-o", "--outputFile" }, description: "The file to write to")
                { IsRequired = true };
        var outputEncodingOption = new Option<string>(aliases: new[] { "-oe", "--outputEncoding" },
            getDefaultValue: () => "Windows-1252");
        var outputDelimiterOption =
            new Option<string>(aliases: new[] { "-od", "--outputDelimiter" }, getDefaultValue: () => ",");
        var useHeaderRecordsOption =
            new Option<bool>(aliases: new[] { "-hdr", "--hasHeaderRow" }, getDefaultValue: () => true);
        var removeNonPrintableCharactersOption = new Option<bool>(aliases: new[] { "-rnp", "--removeNonPrintable" },
            getDefaultValue: () => false);
        var includeCrOption = new Option<bool>(aliases: new[] { "-cr", "--includeCarriageReturn" }, getDefaultValue:
            () => false, description: "Should records be separated by LF (default) or CRLF");
        var rootCommand = new RootCommand("CsvFileConverter Utility")
        {
            inputFileOption,
            inputEncodingOption,
            inputDelimiterOption,
            outputFileOption,
            outputEncodingOption,
            outputDelimiterOption,
            useHeaderRecordsOption,
            removeNonPrintableCharactersOption,
            includeCrOption
        };
        rootCommand.SetHandler(async ctx =>
        {
            var inputEncoding = Encoding.GetEncoding(ctx.ParseResult.GetValueForOption(inputEncodingOption));
            var inFile = ctx.ParseResult.GetValueForOption(inputFileOption);
            var outputEncoding = Encoding.GetEncoding(ctx.ParseResult.GetValueForOption(outputEncodingOption));
            var outFile = ctx.ParseResult.GetValueForOption(outputFileOption);
            var conversionConfig = new ConversionConfig
            {
                InputDelimiter = ctx.ParseResult.GetValueForOption(inputDelimiterOption).ConvertWhenHex(),
                OutputDelimiter = ctx.ParseResult.GetValueForOption(outputDelimiterOption).ConvertWhenHex(),
                HasHeader = ctx.ParseResult.GetValueForOption(useHeaderRecordsOption),
                RemoveNonPrintableCharacters = ctx.ParseResult.GetValueForOption(removeNonPrintableCharactersOption),
                IncludeCrInLineEnding = ctx.ParseResult.GetValueForOption(includeCrOption)
            };
            using var reader = new StreamReader(inFile.FullName, inputEncoding, true);
            await using var writer = new StreamWriter(outFile.FullName, false, outputEncoding);
            await Util.ProcessStreams(reader, writer, conversionConfig);
        });
        return await rootCommand.InvokeAsync(args);
    }
}
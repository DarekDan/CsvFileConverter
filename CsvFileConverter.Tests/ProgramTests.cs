using System.Text;
using Fiserv;

namespace CsvFileConverter.Tests;

public class ProgramTests
{
    private StringBuilder _original = new StringBuilder();

    public ProgramTests()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var writer = new StringWriter(_original);
        writer.WriteLine("C1\tC2\tC3");
        writer.WriteLine("C1\t\"C\n2\"\t3.0");
    }

    [Fact]
    public async void ConversionMustSucceed()
    {
        var memory = new MemoryStream();
        using var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(_original.ToString())));
        await using var writer = new StreamWriter(memory);
        await Util.ProcessStreams(reader, writer, new ConversionConfig
        {
            HasHeader = true, RemoveNonPrintableCharacters = true, InputDelimiter = "\t", OutputDelimiter = ",",
            IncludeCrInLineEnding = false
        });
        var output = Encoding.UTF8.GetString(memory.ToArray());
        Assert.Equal("C1,C2,C3\nC1,C 2,3.0", output);
    }
}
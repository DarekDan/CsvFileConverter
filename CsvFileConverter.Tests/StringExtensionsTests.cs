using Fiserv;

namespace CsvFileConverter.Tests;

public class StringExtensionsTests
{
    [Fact]
    public void StringMustBeFreeOFNPC()
    {
        string s = "A\tB\nC\rD";
        Assert.Equal("A B C D", s.Cleanse());
    }

    [Fact]
    public void StringRepresentationMustConvertSuccessfully()
    {
        Assert.Equal("\t", "0x09".ConvertWhenHex());
        Assert.Equal(",", ",".ConvertWhenHex());
    }
}
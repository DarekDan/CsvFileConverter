namespace Fiserv;

sealed internal class ConversionConfig
{
    public string InputDelimiter { get; set; }
    public string OutputDelimiter { get; set; }
    public bool HasHeader { get; set; }
    public bool RemoveNonPrintableCharacters { get; set; }
    public bool IncludeCrInLineEnding { get; set; }
}
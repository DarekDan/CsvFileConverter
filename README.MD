# CsvFileConverter

A context-aware character-separated-values (CSV) file, capable of transcoding and change
in delimiters, while preserving (or not) special characters within each column.

```shell
Description:
  CsvFileConverter Utility

Usage:
  CsvFileConverter [options]

Options:
  -i, --inputFile <inputFile> (REQUIRED)    The file to read from
  -ie, --inputEncoding <inputEncoding>      [default: utf-8]
  -id, --inputDelimiter <inputDelimiter>    The default input delimiter is TAB, 0x09 []
  -o, --outputFile <outputFile> (REQUIRED)  The file to write to
  -oe, --outputEncoding <outputEncoding>    [default: Windows-1252]
  -od, --outputDelimiter <outputDelimiter>  [default: ,]
  -hdr, --hasHeaderRow                      [default: True]
  -rnp, --removeNonPrintable                [default: False]
  -cr, --includeCarriageReturn              Should records be separated by LF (default) or CRLF [default: False]
  --version                                 Show version information
  -?, -h, --help                            Show help and usage information

```

## Compilation for Linux 64-bit

```shell
dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:IncludeNativeLibrariesForSelfExtract=true -p:UseAppHost=true --self-contained true
```

[dotnet publish RIDs](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)
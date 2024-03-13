using Core.Dal;
using Core.Encodings;
using Microsoft.EntityFrameworkCore;
using Core.Encodings.Builders.ShannonFano;
using System.Text;
using SharedKernel.Errors;
using System.IO.Compression;
using Core.Encodings.Builders.VariableLengthCode;
using Core.Encodings.Builders.Huffman;

namespace Core.Commands.Base;

internal abstract class BaseEncoding
{
    internal static EncodingTableElements EnglishTableElements => new EncodingTableElements()
    {
         EncodingTableElement.Init(' ', "11").Value,
         EncodingTableElement.Init('t', "1001").Value,
         EncodingTableElement.Init('n', "10000").Value,
         EncodingTableElement.Init('s', "0101").Value,
         EncodingTableElement.Init('r', "01000").Value,
         EncodingTableElement.Init('d', "00101").Value,
         EncodingTableElement.Init('!', "001000").Value,
         EncodingTableElement.Init('c', "000101").Value,
         EncodingTableElement.Init('m', "000011").Value,
         EncodingTableElement.Init('g', "0000100").Value,
         EncodingTableElement.Init('b', "0000010").Value,
         EncodingTableElement.Init('v', "00000001").Value,
         EncodingTableElement.Init('k', "0000000001").Value,
         EncodingTableElement.Init('q', "000000000001").Value,
         EncodingTableElement.Init('e', "101").Value,
         EncodingTableElement.Init('o', "10001").Value,
         EncodingTableElement.Init('a', "011").Value,
         EncodingTableElement.Init('i', "01001").Value,
         EncodingTableElement.Init('h', "0011").Value,
         EncodingTableElement.Init('l', "001001").Value,
         EncodingTableElement.Init('u', "00011").Value,
         EncodingTableElement.Init('f', "000100").Value,
         EncodingTableElement.Init('p', "0000101").Value,
         EncodingTableElement.Init('w', "0000011").Value,
         EncodingTableElement.Init('y', "0000001").Value,
         EncodingTableElement.Init('j', "000000001").Value,
         EncodingTableElement.Init('x', "00000000001").Value,
         EncodingTableElement.Init('z', "000000000000").Value
    };

    internal static EncodingTableElements UkrainianTableElements => new EncodingTableElements()
    {
         EncodingTableElement.Init(' ', "00000").Value,
         EncodingTableElement.Init('!', "00001").Value,
         EncodingTableElement.Init('\'', "00010").Value,
         EncodingTableElement.Init('а', "00011").Value,
         EncodingTableElement.Init('б', "00100").Value,
         EncodingTableElement.Init('в', "00101").Value,
         EncodingTableElement.Init('г', "00110").Value,
         EncodingTableElement.Init('ґ', "00111").Value,
         EncodingTableElement.Init('д', "010000").Value,
         EncodingTableElement.Init('е', "010001").Value,
         EncodingTableElement.Init('є', "010010").Value,
         EncodingTableElement.Init('ж', "010011").Value,
         EncodingTableElement.Init('з', "010100").Value,
         EncodingTableElement.Init('и', "010101").Value,
         EncodingTableElement.Init('і', "010110").Value,
         EncodingTableElement.Init('ї', "010111").Value,
         EncodingTableElement.Init('й', "011000").Value,
         EncodingTableElement.Init('к', "011001").Value,
         EncodingTableElement.Init('л', "011010").Value,
         EncodingTableElement.Init('м', "011011").Value,
         EncodingTableElement.Init('н', "011100").Value,
         EncodingTableElement.Init('о', "011101").Value,
         EncodingTableElement.Init('п', "011110").Value,
         EncodingTableElement.Init('р', "011111").Value,
         EncodingTableElement.Init('с', "100000").Value,
         EncodingTableElement.Init('т', "100001").Value,
         EncodingTableElement.Init('у', "100010").Value,
         EncodingTableElement.Init('ф', "100011").Value,
         EncodingTableElement.Init('х', "100100").Value,
         EncodingTableElement.Init('ц', "100101").Value,
         EncodingTableElement.Init('ч', "100110").Value,
         EncodingTableElement.Init('ш', "100111").Value,
         EncodingTableElement.Init('щ', "101000").Value,
         EncodingTableElement.Init('ь', "101001").Value,
         EncodingTableElement.Init('ю', "101010").Value,
         EncodingTableElement.Init('я', "101011").Value
    };

    protected readonly ArchivEaseContext _context;
    private readonly EncodingAnalyzer _analyzer;

    public BaseEncoding(ArchivEaseContext context, EncodingAnalyzer analyzer) => (_context, _analyzer) = (context, analyzer);

    protected async Task<(List<EncodingFile>, List<EncodingTable>)> GetEncodingRelatedDataAsync(List<string> fileNames, CancellationToken cancellationToken = default)
    {
        List<EncodingTable> _existingTables = [];

        List<EncodingFile> existingFiles = await _context
            .EncodingFiles
            .AsNoTracking()
            .Where(x => fileNames.Contains(x.FileName))
            .ToListAsync(cancellationToken);

        List<Guid>? existingFileTableIds = existingFiles.Select(x => x.EncodingTableId).ToList();

        if (existingFiles.Any() && existingFileTableIds.Any())
        {
            _existingTables = await _context
                .EncodingTables
                    .AsNoTracking()
                    .Where(x => existingFileTableIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);
        }

        return (existingFiles, _existingTables);
    }

    protected async Task<(byte[], EncodingTableElements?, Guid, Guid)> EncodeFileAsync
    (
        Stream stream
    )
    {
        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
            string content = await reader.ReadToEndAsync();

            (string algorithmName, string languageName) = _analyzer.PredictAlgorithmAndLanguage(content);

            return algorithmName switch
            {
                "variable_length_code" => VariableLengthEncode(content, languageName),
                "shannon_fano" => ShannonFanoEncode(content, languageName),
                "huffman" => HuffmanEncode(content, languageName),
                _ => ShannonFanoEncode(content, languageName)
            };
        }

        (byte[], EncodingTableElements, Guid, Guid) VariableLengthEncode(string content, string languageName)
        {
            EncodingTableElements tableElements = languageName == EncodingLanguage.Ukrainian.Name ? UkrainianTableElements 
                                                                                                  : EnglishTableElements;

            using (var encoder = new VariableLengthCodeEncodeBuilder())
            {
                return encoder
                    .WithContent(content)
                    .WithTableElements(tableElements)
                    .WithLanguage(languageName)
                    .PrepareContent()
                    .Build();
            }
        }

        (byte[], EncodingTableElements, Guid, Guid) ShannonFanoEncode(string content, string languageName)
        {
            using (var encoder = new ShannonFanoEncodeBuilder())
            {
                return encoder
                    .WithContent(content)
                    .WithTableElements(null)
                    .WithLanguage(languageName)
                    .PrepareContent()
                    .Build();
            }
        }

        (byte[], EncodingTableElements, Guid, Guid) HuffmanEncode(string content, string languageName)
        {
            using (var encoder = new HuffmanEncodeBuilder())
            {
                return encoder
                    .WithContent(content)
                    .WithTableElements(null)
                    .WithLanguage(languageName)
                    .PrepareContent()
                    .Build();
            }
        }
    }

    protected string DecodeFile
    (
        byte[] encodedBytes,
        EncodingTableElements encodingTableElements,
        EncodingAlgorithm? algorithm,
        EncodingLanguage? language
    )
    {
        return algorithm?.Name switch
        {
            "variable_length_code" => VariableLengthCodeDecode(),
            "shannon_fano" => ShannonFanoDecode(),
            "huffman" => HuffmanDecode(),
            _ => string.Empty
        };

        string VariableLengthCodeDecode()
        {
            using (var decoder = new VariableLengthCodeDecodeBuilder())
            {
                return decoder
                    .WithContent(encodedBytes)
                    .WithTableElements(encodingTableElements)
                    .WithLanguage(language!.Name)
                    .PrepareContent()
                    .Build();
            }
        }

        string ShannonFanoDecode()
        {
            using (var decoder = new ShannonFanoDecodeBuilder())
            {
                return decoder
                    .WithContent(encodedBytes)
                    .WithTableElements(encodingTableElements)
                    .WithLanguage(language!.Name)
                    .PrepareContent()
                    .Build();
            }
        }

        string HuffmanDecode()
        {
            using (var decoder = new HuffmanDecodeBuilder())
            {
                return decoder
                    .WithContent(encodedBytes)
                    .WithTableElements(encodingTableElements)
                    .WithLanguage(language!.Name)
                    .PrepareContent()
                    .Build();
            }
        }
    }

    protected async Task< Result<byte[]>> ZipResult(Dictionary<string, byte[]> encodedContent)
    {
        using (var zipMemoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(zipMemoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var (key, value) in encodedContent)
                {
                    var entry = zipArchive.CreateEntry(key);

                    using (var entryStream = entry.Open())
                    {
                        await entryStream.WriteAsync(value, 0, value.Length);
                    }
                }
            }

            zipMemoryStream.Seek(0, SeekOrigin.Begin);

            var zipBytes = zipMemoryStream.ToArray();

            return Result.Success(zipBytes);
        }
    }
}

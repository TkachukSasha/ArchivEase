using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.ShannonFano;

public sealed class ShannonFanoEncodeBuilder
    :
    BaseEncodeBuilder<ShannonFanoEncodeBuilder, string, EncodingTableElements>,
    IDisposable,
    IEncodeBuilder
    <
        ShannonFanoEncodeBuilder,
        EncodingTableElements,
        string
    >
{
    private Dictionary<char, int> _symbolFrequency = new Dictionary<char, int>();
    private Dictionary<char, SymbolStatistic> _elementsStatistic = new Dictionary<char, SymbolStatistic>();

    public ShannonFanoEncodeBuilder WithContent(string content)
    {
        Content = content;
        return this;
    }

    public ShannonFanoEncodeBuilder WithTableElements(EncodingTableElements? encodingTableElements)
    {
        EncodingTableElements = encodingTableElements is null ? new EncodingTableElements() : encodingTableElements;
        return this;
    }

    public ShannonFanoEncodeBuilder WithLanguage(string language)
    {
        Language = language;
        return this;
    }

    public ShannonFanoEncodeBuilder PrepareContent()
    {
        if (!IsContentNotNullOrWhiteSpace())
            return this;

        FillSymbolFrequency();

        FillElementsStatistic();

        FillEncodingElements();

        return this;
    }

    public (byte[], EncodingTableElements, Guid, Guid) Build()
    {
        var codes = EncodingTableElements!.ToDictionary(element => element.Symbol, element => element.Code);

        var response = new StringBuilder(Content!.Length);

        foreach (var c in Content.AsSpan())
            response.Append(codes[c]);

        byte[] encodedBytes = ConvertChunksToByteArray(response.ToString());

        return (encodedBytes.ToArray()!, EncodingTableElements!, EncodingLanguage.FromName(Language)!.Value, EncodingAlgorithm.ShannonFanoAlgorithm.Value);
    }

    #region local
    private void FillSymbolFrequency()
    {
        foreach (var c in Content!.AsSpan())
            _symbolFrequency[c] = _symbolFrequency.GetValueOrDefault(c) + 1;
    }

    private void FillElementsStatistic()
    {
        var codes = _symbolFrequency.Select(kv => new SymbolStatistic
        {
            Symbol = kv.Key,
            Frequency = kv.Value
        })
          .OrderByDescending(code => code.Frequency)
          .ThenBy(code => code.Symbol)
          .ToList();

        AssignCodes(codes);

        foreach (var code in codes)
            _elementsStatistic[code.Symbol] = code;
    }

    private void FillEncodingElements()
    {
        foreach (var kvp in _elementsStatistic)
        {
            string byteStr = Convert.ToString(kvp.Value.Bits, 2).PadLeft(kvp.Value.Size, '0');

            EncodingTableElements?.Add(EncodingTableElement.Init(kvp.Key, byteStr).Value);
        }
    }

    private void AssignCodes(List<SymbolStatistic> codes)
    {
        if (codes.Count < 2) return;

        int totalFrequency = codes.Sum(code => code.Frequency);
        int leftFrequency = 0;

        // Find the best split position
        int bestPosition = FindBestSplitPosition(codes, totalFrequency, ref leftFrequency);

        // Assign codes based on the split position
        for (int i = 0; i < codes.Count; i++)
        {
            // Increase code size for each symbol
            codes[i].Size++;

            // Assign the bit based on the split position
            if (i < bestPosition)
                codes[i].Bits <<= 1; // Move the bit to the left
            else
            {
                codes[i].Bits <<= 1;
                codes[i].Bits |= 1; // Set the least significant bit to 1
            }
        }

        // Recursively assign codes to the left and right partitions
        AssignCodes(codes.GetRange(0, bestPosition));
        AssignCodes(codes.GetRange(bestPosition, codes.Count - bestPosition));
    }

    private int FindBestSplitPosition(List<SymbolStatistic> codes, int totalFrequency, ref int leftFrequency)
    {
        int bestPosition = 0;
        int previousDiff = int.MaxValue;

        for (int i = 0; i < codes.Count - 1; i++)
        {
            int rightFrequency = totalFrequency - leftFrequency;
            int diff = Math.Abs(rightFrequency - leftFrequency);

            if (diff >= previousDiff)
                break;

            previousDiff = diff;
            bestPosition = i + 1;
            leftFrequency += codes[i].Frequency;
        }

        return bestPosition;
    }
    #endregion
}
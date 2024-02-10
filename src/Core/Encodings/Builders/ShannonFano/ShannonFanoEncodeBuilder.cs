using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.ShannonFano;

public sealed class ShannonFanoEncodeBuilder
    :
    BaseEncodeBuilder<ShannonFanoEncodeBuilder, string, EncodingTableElements>,
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

    public ShannonFanoEncodeBuilder PrepareContent()
    {
        if (!IsContentNotNullOrWhiteSpace())
            return this;

        FillSymbolFrequency();

        FillElementsStatistic();

        FillEncodingElements();

        return this;
    }

    public (byte[], EncodingTableElements) Build()
    {
        var codes = EncodingTableElements!.ToDictionary(element => element.Symbol, element => element.Code);

        var response = new StringBuilder(Content!.Length);

        foreach (var c in Content.AsSpan())
            response.Append(codes[c]);

        byte[] encodedBytes = ConvertChunksToByteArray(response.ToString());

        return (encodedBytes.ToArray()!, EncodingTableElements!);
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

        int divider = BestPosition();

        for (int i = 0; i < codes.Count; i++)
        {
            codes[i].Bits <<= 1;
            codes[i].Size++;

            if (i >= divider)
                codes[i].Bits |= 1;
        }

        AssignCodes(codes.GetRange(0, divider));
        AssignCodes(codes.GetRange(divider, codes.Count - divider));

        int BestPosition()
        {
            int total = codes.Sum(code => code.Frequency);
            int left = codes[0].Frequency;

            int bestPosition = 0;
            int previousDiff = int.MaxValue;

            for (int i = 0; i < codes.Count - 1; i++)
            {
                int right = total - left;

                int diff = Math.Abs(right - left);

                if (diff >= previousDiff)
                    break;

                previousDiff = diff;
                bestPosition = i + 1;
            }

            return bestPosition;
        }
    }
    #endregion
}
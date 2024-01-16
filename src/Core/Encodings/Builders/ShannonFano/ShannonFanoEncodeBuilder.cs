using SharedKernel.Builders.Base;
using SharedKernel.Builders;

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

    public (string, EncodingTableElements) Build()
    {
        Dictionary<char, string> codes = new Dictionary<char, string>();

        foreach (var element in EncodingTableElements!)
            codes[element.Symbol] = element.Code;

        string response = "";

        foreach (char c in Content!)
            response += codes[c];

        return (response, EncodingTableElements);
    }

    #region local
    private void FillSymbolFrequency()
    {
        foreach (char c in Content!.AsSpan())
        {
            if (!_symbolFrequency.ContainsKey(c))
                _symbolFrequency.TryAdd(c, 1);

            _symbolFrequency[c]++;
        }
    }

    private void FillElementsStatistic()
    {
        List<SymbolStatistic> codes = _symbolFrequency.Select(kv => new SymbolStatistic
        {
            Symbol = kv.Key,
            Frequency = kv.Value
        }).ToList();

        codes = codes.OrderByDescending(code => code.Frequency)
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
            string byteStr = Convert.ToString(kvp.Value.Bits, 2);

            if (byteStr.Length < kvp.Value.Size)
            {
                byteStr = byteStr.PadLeft(kvp.Value.Size, '0');
            }

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
            {
                codes[i].Bits |= 1;
            }
        }

        AssignCodes(codes.GetRange(0, divider));
        AssignCodes(codes.GetRange(divider, codes.Count - divider));

        int BestPosition()
        {
            int total = 0;
            int bestPosition = 0;
            int previousDiff = int.MaxValue;

            foreach (var code in codes)
                total += code.Frequency;

            int left = codes[0].Frequency;

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
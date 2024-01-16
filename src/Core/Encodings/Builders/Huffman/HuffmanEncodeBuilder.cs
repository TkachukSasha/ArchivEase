using SharedKernel.Builders.Base;
using SharedKernel.Builders;

namespace Core.Encodings.Builders.Huffman;

public sealed class HuffmanEncodeBuilder
    :
    BaseEncodeBuilder<HuffmanEncodeBuilder, string, EncodingTableElements>,
    IEncodeBuilder
    <
        HuffmanEncodeBuilder,
        EncodingTableElements,
        string
    >
{
    private HuffmanNode _root = new();
    private Dictionary<char, int> _symbolFrequency = new Dictionary<char, int>();

    public HuffmanEncodeBuilder WithContent(string content)
    {
        Content = content;
        return this;
    }

    public HuffmanEncodeBuilder WithTableElements(EncodingTableElements? encodingTableElements)
    {
        EncodingTableElements = encodingTableElements is null ? new EncodingTableElements() : encodingTableElements;
        return this;
    }

    public HuffmanEncodeBuilder PrepareContent()
    {
        if (string.IsNullOrWhiteSpace(Content))
            return this;

        FillSymbolFrequency();

        FillEncodingElements(string.Empty);

        return this;
    }

    public (string, EncodingTableElements) Build()
    {
        Dictionary<char, string> codes = new Dictionary<char, string>();

        foreach (var element in EncodingTableElements!)
            codes[element.Symbol] = element.Code;

        string response = "";

        foreach (char c in Content!.AsSpan())
        {
            if (codes.ContainsKey(c))
                response += codes[c];
            else
                throw new ArgumentException($"Symbol {c} not found in the encoding table.");
        }

        return (response, EncodingTableElements!);
    }

    #region local
    private void FillSymbolFrequency()
        => _symbolFrequency = Content!.GroupBy(c => c)
            .ToDictionary(gr => gr.Key, gr => gr.Count());

    private void FillEncodingElements(string code)
    {
        var nodes = _symbolFrequency.Select(pair => new HuffmanNode { Symbol = pair.Key, Frequency = pair.Value }).ToList();

        while (nodes.Count > 1)
        {
            nodes = nodes.OrderBy(node => node.Frequency).ToList();

            var left = nodes[0];
            var right = nodes[1];

            var parent = new HuffmanNode
            {
                Symbol = '\0',
                Frequency = left.Frequency + right.Frequency,
                Left = left,
                Right = right
            };

            nodes.Remove(left);
            nodes.Remove(right);
            nodes.Add(parent);
        }

        _root = nodes.Single();

        GenerateEncodingElements(_root, "");
    }

    private void GenerateEncodingElements(HuffmanNode? node, string code)
    {
        if (node == null)
            return;

        if (node.Symbol != '\0')
        {
            EncodingTableElements!.Add(EncodingTableElement.Init(node.Symbol, code).Value);
            return;
        }

        GenerateEncodingElements(node.Left, code + '0');
        GenerateEncodingElements(node.Right, code + '1');
    }
    #endregion
}
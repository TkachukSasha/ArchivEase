using SharedKernel.Builders.Base;
using SharedKernel.Builders;

namespace Core.Encodings.Builders.Huffman;

public sealed class HuffmanDecodeBuilder
    :
    BaseDecodeBuilder<HuffmanDecodeBuilder, string, EncodingTableElements>,
    IDecodeBuilder
    <
        HuffmanDecodeBuilder,
        EncodingTableElements,
        string,
        string
    >
{
    private DecodingTreeNode? _decodingTree;
    private string _content = string.Empty;

    public HuffmanDecodeBuilder WithContent(string content)
    {
        EncodedContent = content;
        return this;
    }

    public HuffmanDecodeBuilder WithTableElements(EncodingTableElements encodingTableElements)
    {
        EncodingTableElements = encodingTableElements;
        _decodingTree = new DecodingTreeNode().Get(EncodingTableElements);
        return this;
    }

    public HuffmanDecodeBuilder PrepareContent()
    {
        if (!IsContentNotNullOrWhiteSpace())
            return this;

        if (!IsEncodingTableElementsProvided())
            return this;

        _content = _decodingTree!.Decode(EncodedContent!);

        return this;
    }

    public string Build()
        => _content;
}
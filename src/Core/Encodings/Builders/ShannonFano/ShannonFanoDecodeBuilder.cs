using SharedKernel.Builders.Base;
using SharedKernel.Builders;

namespace Core.Encodings.Builders.ShannonFano;

public sealed class ShannonFanoDecodeBuilder
    :
    BaseDecodeBuilder<ShannonFanoDecodeBuilder, string, EncodingTableElements>,
    IDecodeBuilder
    <
        ShannonFanoDecodeBuilder,
        EncodingTableElements,
        string,
        string
    >
{
    private DecodingTreeNode? _decodingTree;
    private string _content = string.Empty;

    public ShannonFanoDecodeBuilder WithContent(string content)
    {
        EncodedContent = content;
        return this;
    }

    public ShannonFanoDecodeBuilder WithTableElements(EncodingTableElements encodingTableElements)
    {
        EncodingTableElements = encodingTableElements;
        _decodingTree = new DecodingTreeNode().Get(EncodingTableElements);
        return this;
    }

    public ShannonFanoDecodeBuilder PrepareContent()
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
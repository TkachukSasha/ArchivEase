using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.Huffman;

public sealed class HuffmanDecodeBuilder
    :
    BaseDecodeBuilder<HuffmanDecodeBuilder, byte[], EncodingTableElements>,
    IDecodeBuilder
    <
        HuffmanDecodeBuilder,
        EncodingTableElements,
        byte[],
        string
    >
{
    private DecodingTreeNode? _decodingTree;
    private string _content = string.Empty;

    public HuffmanDecodeBuilder WithContent(byte[] content)
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

        var response = new StringBuilder();

        foreach (byte code in EncodedContent!)
        {
            ConvertChunkToString(code, response);
        }

        _content = _decodingTree!.Decode(response.ToString()!).TrimEnd();

        return this;
    }

    public string Build()
        => _content;
}
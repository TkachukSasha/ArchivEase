using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.ShannonFano;

public sealed class ShannonFanoDecodeBuilder
    :
    BaseDecodeBuilder<ShannonFanoDecodeBuilder, byte[], EncodingTableElements>,
    IDecodeBuilder
    <
        ShannonFanoDecodeBuilder,
        EncodingTableElements,
        byte[],
        string
    >
{
    private DecodingTreeNode? _decodingTree;
    private string _content = string.Empty;

    public ShannonFanoDecodeBuilder WithContent(byte[] content)
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

        var response = new StringBuilder(EncodedContent!.Length);

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
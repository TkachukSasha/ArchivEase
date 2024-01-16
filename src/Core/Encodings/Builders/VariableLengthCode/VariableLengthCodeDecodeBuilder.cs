using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.VariableLengthCode;

public sealed class VariableLengthCodeDecodeBuilder
    :
    BaseDecodeBuilder<VariableLengthCodeDecodeBuilder, byte[], EncodingTableElements>,
    IDecodeBuilder
    <
        VariableLengthCodeDecodeBuilder,
        EncodingTableElements,
        byte[],
        string
    >
{
    private DecodingTreeNode? _decodingTree;
    private string? _content;

    public VariableLengthCodeDecodeBuilder WithContent(byte[] encodedContent)
    {
        EncodedContent = encodedContent;
        return this;
    }

    public VariableLengthCodeDecodeBuilder WithTableElements(EncodingTableElements encodingTableElements)
    {
        EncodingTableElements = encodingTableElements;
        _decodingTree = new DecodingTreeNode().Get(EncodingTableElements);
        return this;
    }

    public VariableLengthCodeDecodeBuilder PrepareContent()
    {
        TransformText();
        return this;
    }

    public string Build()
        => _content!;

    #region local
    private void TransformText()
    {
        var response = new StringBuilder();

        foreach (byte code in EncodedContent!)
        {
            var binChunk = ConvertBytesToString(code);
            response.Append(binChunk);
        }

        var decodedText = _decodingTree?.Decode(response.ToString());

        ExportText(decodedText!);
    }

    private void ExportText(string content)
    {
        var response = new StringBuilder();

        bool isCapital = false;

        foreach (char symbol in content)
        {
            if (isCapital)
            {
                response.Append(char.ToUpper(symbol));
                isCapital = false;
                continue;
            }

            if (symbol == '!') isCapital = true;
            else response.Append(symbol);
        }

        _content = response.ToString().TrimEnd();
    }

    private string ConvertBytesToString(byte code)
        => Convert.ToString(code, 2).PadLeft(8, '0');
    #endregion
}
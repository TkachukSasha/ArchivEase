using SharedKernel.Builders.Base;
using SharedKernel.Builders;
using System.Text;

namespace Core.Encodings.Builders.VariableLengthCode;

public sealed class VariableLengthCodeEncodeBuilder
    :
    BaseEncodeBuilder<VariableLengthCodeEncodeBuilder, string, EncodingTableElements>,
    IEncodeBuilder
    <
        VariableLengthCodeEncodeBuilder,
        EncodingTableElements,
        string
    >
{
    public VariableLengthCodeEncodeBuilder WithContent(string content)
    {
        Content = content;
        return this;
    }

    public VariableLengthCodeEncodeBuilder WithTableElements(EncodingTableElements? encodingTableElements)
    {
        EncodingTableElements = encodingTableElements is null ? new EncodingTableElements() : encodingTableElements;
        return this;
    }

    public VariableLengthCodeEncodeBuilder PrepareContent()
    {
        if (!IsContentNotNullOrWhiteSpace())
            return this;

        TranformContent();

        return this;
    }

    public byte[] Build()
        => IsContentNotNullOrWhiteSpace() && IsEncodingTableElementsProvided() ? Encode(8)
                                                                               : Array.Empty<byte>();

    #region local
    private void TranformContent()
    {
        var response = new StringBuilder(Content!.Length);

        foreach (char symbol in Content!.AsSpan())
        {
            if (char.IsUpper(symbol))
            {
                response.Append("!");
                response.Append(char.ToLower(symbol));
                continue;
            }

            response.Append(symbol);
        }

        Content = response.ToString();
    }

    private byte[] Encode(int chunkSize)
    {
        var response = new StringBuilder(Content!.Length);

        foreach (char symbol in Content.AsSpan())
        {
            if (EncodingTableElements!.FirstOrDefault(x => x.Symbol == symbol) is null)
                throw new ArgumentNullException(symbol.ToString());

            response.Append(EncodingTableElements!.FirstOrDefault(x => x.Symbol == symbol)!.Code);
        }

        return ConvertChunksToByteArray(response.ToString());
    }
    #endregion
}
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

        BinaryChunks chunks = SplitByChunks(response.ToString(), chunkSize);

        Span<byte> encodedBytes = new byte[chunks.Count];

        int index = 0;

        foreach (var binaryChunk in chunks)
            encodedBytes[index++] = binaryChunk.Byte();

        return encodedBytes.ToArray();
    }

    private BinaryChunks SplitByChunks
    (
        string content,
        int chunkSize
    )
    {
        int contentLength = content.Length;

        int chunksCount = contentLength / chunkSize;

#pragma warning disable S1854 // Unused assignments should be removed
        if (contentLength % chunkSize != 0) chunksCount++;
#pragma warning restore S1854 // Unused assignments should be removed

        BinaryChunks result = new BinaryChunks();

        StringBuilder buffer = new StringBuilder(contentLength);

        for (int i = 0; i < contentLength; i++)
        {
            buffer.Append(content[i]);

            if ((i + 1) % chunkSize == 0)
            {
                result.Add(new BinaryChunk(Convert.ToByte(buffer.ToString(), 2)));
                buffer.Clear();
            }
        }

        if (buffer.Length != 0)
        {
            string lastChunk = buffer.ToString();
            lastChunk += new string('0', chunkSize - lastChunk.Length);

            result.Add(new BinaryChunk(Convert.ToByte(lastChunk, 2)));
        }

        return result;
    }
    #endregion
}
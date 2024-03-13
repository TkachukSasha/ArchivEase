using System.Text;

namespace SharedKernel.Builders.Base;

public abstract class BaseEncodeBuilder<TEncoder, TContent, TTableElements> : IDisposable
    where TTableElements : IEnumerable<object>
{
    private bool disposed = false;

    protected TContent? Content { get; set; }

    protected TTableElements? EncodingTableElements { get; set; }

    protected string Language { get; set; } = string.Empty;

    public static TEncoder Init()
        => Activator.CreateInstance<TEncoder>();

    ~BaseEncodeBuilder()
    {
        Dispose(true);
    }

    #region validations
    protected bool IsContentNotNullOrWhiteSpace()
    {
        if (typeof(TContent) == typeof(string))
            return !string.IsNullOrWhiteSpace(Content as string);

        if (typeof(TContent) == typeof(byte[]))
            return (Content as byte[])?.Length > 0;

        return false;
    }

    protected bool IsEncodingTableElementsProvided()
        => EncodingTableElements != null && EncodingTableElements.Any();
    #endregion

    #region chunks logics
    protected BinaryChunks SplitByChunks
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

    protected byte[] ConvertChunksToByteArray
    (
        string content,
        int chunkSize = 8
    )
    {
        BinaryChunks chunks = SplitByChunks(content.ToString(), chunkSize);

        Span<byte> encodedBytes = new byte[chunks.Count];

        int index = 0;

        foreach (var binaryChunk in chunks)
            encodedBytes[index++] = binaryChunk.Byte();

        return encodedBytes.ToArray();
    }
    #endregion

    #region dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            Content = default;
            EncodingTableElements = default;

            disposed = true;
        }
    }
    #endregion
}
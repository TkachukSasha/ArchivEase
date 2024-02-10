using System.Text;

namespace SharedKernel.Builders.Base;

public abstract class BaseDecodeBuilder<TDecoder, TContent, TTableElements>
    where TTableElements : IEnumerable<object>
{
    protected TContent? EncodedContent { get; set; }

    protected TTableElements? EncodingTableElements { get; set; }

    public static TDecoder Init()
       => Activator.CreateInstance<TDecoder>();

    protected bool IsContentNotNullOrWhiteSpace()
    {
        if (typeof(TContent) == typeof(string))
            return !string.IsNullOrWhiteSpace(EncodedContent as string);

        if (typeof(TContent) == typeof(byte[]))
            return (EncodedContent as byte[])?.Length > 0;

        return false;
    }

    protected bool IsEncodingTableElementsProvided()
        => EncodingTableElements != null && EncodingTableElements!.Any();

    protected void ConvertChunkToString
    (
        byte code,
        StringBuilder response
    )
    {
        var span = new Span<char>(new char[8]);
        var binChunk = Convert.ToString(code, 2).PadLeft(8, '0').AsSpan();
        binChunk.CopyTo(span);
        response.Append(span);
    }
}
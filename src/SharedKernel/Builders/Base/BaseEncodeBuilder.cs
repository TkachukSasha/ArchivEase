namespace SharedKernel.Builders.Base;

public abstract class BaseEncodeBuilder<TEncoder, TContent, TTableElements>
    where TTableElements : IEnumerable<object>
{
    protected TContent? Content { get; set; }

    protected TTableElements? EncodingTableElements { get; set; }

    public static TEncoder Init()
        => Activator.CreateInstance<TEncoder>();

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
}
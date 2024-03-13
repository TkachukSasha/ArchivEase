namespace SharedKernel.Builders;

public interface IDecodeBuilder<TDecoder, TTableElements, TContent, TResponse>
{
    TDecoder WithContent(TContent content);

    TDecoder WithTableElements(TTableElements encodingTableElements);

    TDecoder WithLanguage(string language);

    TDecoder PrepareContent();

    TResponse Build();
}
namespace SharedKernel.Builders;

public interface IEncodeBuilder<TEncoder, TTableElements, TContent>
{
    TEncoder WithContent(TContent content);

    TEncoder WithTableElements(TTableElements? tableElements);

    TEncoder WithLanguage(string language);

    TEncoder PrepareContent();
}
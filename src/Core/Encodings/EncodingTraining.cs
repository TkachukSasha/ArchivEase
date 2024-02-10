using SharedKernel.Abstractions;
using SharedKernel.Errors;
using System.Text;

namespace Core.Encodings;

public sealed class EncodingTraining : Entity<EncodingTrainingId>
{
    internal static EncodingTableElements EnglishTableElements => new EncodingTableElements()
    {
         EncodingTableElement.Init(' ', "11").Value,
         EncodingTableElement.Init('t', "1001").Value,
         EncodingTableElement.Init('n', "10000").Value,
         EncodingTableElement.Init('s', "0101").Value,
         EncodingTableElement.Init('r', "01000").Value,
         EncodingTableElement.Init('d', "00101").Value,
         EncodingTableElement.Init('!', "001000").Value,
         EncodingTableElement.Init('c', "000101").Value,
         EncodingTableElement.Init('m', "000011").Value,
         EncodingTableElement.Init('g', "0000100").Value,
         EncodingTableElement.Init('b', "0000010").Value,
         EncodingTableElement.Init('v', "00000001").Value,
         EncodingTableElement.Init('k', "0000000001").Value,
         EncodingTableElement.Init('q', "000000000001").Value,
         EncodingTableElement.Init('e', "101").Value,
         EncodingTableElement.Init('o', "10001").Value,
         EncodingTableElement.Init('a', "011").Value,
         EncodingTableElement.Init('i', "01001").Value,
         EncodingTableElement.Init('h', "0011").Value,
         EncodingTableElement.Init('l', "001001").Value,
         EncodingTableElement.Init('u', "00011").Value,
         EncodingTableElement.Init('f', "000100").Value,
         EncodingTableElement.Init('p', "0000101").Value,
         EncodingTableElement.Init('w', "0000011").Value,
         EncodingTableElement.Init('y', "0000001").Value,
         EncodingTableElement.Init('j', "000000001").Value,
         EncodingTableElement.Init('x', "00000000001").Value,
         EncodingTableElement.Init('z', "000000000000").Value
    };

    internal static EncodingTableElements UkrainianTableElements => new EncodingTableElements()
    {
         EncodingTableElement.Init(' ', "00000").Value,
         EncodingTableElement.Init('!', "00001").Value,
         EncodingTableElement.Init('\'', "00010").Value,
         EncodingTableElement.Init('а', "00011").Value,
         EncodingTableElement.Init('б', "00100").Value,
         EncodingTableElement.Init('в', "00101").Value,
         EncodingTableElement.Init('г', "00110").Value,
         EncodingTableElement.Init('ґ', "00111").Value,
         EncodingTableElement.Init('д', "010000").Value,
         EncodingTableElement.Init('е', "010001").Value,
         EncodingTableElement.Init('є', "010010").Value,
         EncodingTableElement.Init('ж', "010011").Value,
         EncodingTableElement.Init('з', "010100").Value,
         EncodingTableElement.Init('и', "010101").Value,
         EncodingTableElement.Init('і', "010110").Value,
         EncodingTableElement.Init('ї', "010111").Value,
         EncodingTableElement.Init('й', "011000").Value,
         EncodingTableElement.Init('к', "011001").Value,
         EncodingTableElement.Init('л', "011010").Value,
         EncodingTableElement.Init('м', "011011").Value,
         EncodingTableElement.Init('н', "011100").Value,
         EncodingTableElement.Init('о', "011101").Value,
         EncodingTableElement.Init('п', "011110").Value,
         EncodingTableElement.Init('р', "011111").Value,
         EncodingTableElement.Init('с', "100000").Value,
         EncodingTableElement.Init('т', "100001").Value,
         EncodingTableElement.Init('у', "100010").Value,
         EncodingTableElement.Init('ф', "100011").Value,
         EncodingTableElement.Init('х', "100100").Value,
         EncodingTableElement.Init('ц', "100101").Value,
         EncodingTableElement.Init('ч', "100110").Value,
         EncodingTableElement.Init('ш', "100111").Value,
         EncodingTableElement.Init('щ', "101000").Value,
         EncodingTableElement.Init('ь', "101001").Value,
         EncodingTableElement.Init('ю', "101010").Value,
         EncodingTableElement.Init('я', "101011").Value
    };

    private EncodingTraining() { }

    private EncodingTraining
    (
        Guid id,
        string content,
        string language,
        string algorithm
    ) : base (id)
    {
        Content = content;
        Language = language;
        Algorithm = algorithm;
    }

    public string Content { get; }

    public string Language { get; }

    public string Algorithm { get; }

    public static Result<EncodingTraining> Init(
        string content,
        string language,
        string algorithm
    ) =>
        Result.Ensure(
            (content, language, algorithm),
            (_ => !string.IsNullOrWhiteSpace(content), EncodingTrainingErrors.EncodingTrainingContentMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(language) || !EncodingLanguage.GetValues().Any(x => x.Name == language), EncodingTrainingErrors.EncodingTrainingLanguageMustBeProvide),
            (_ => !string.IsNullOrWhiteSpace(algorithm) || !EncodingAlgorithm.GetValues().Any(x => x.Name == algorithm), EncodingTrainingErrors.EncodingTrainingAlgorithmNameMustBeProvide)
        )
        .Map(_ => new EncodingTraining(new EncodingTrainingId(), content, language, algorithm));

    public static IEnumerable<EncodingTraining> GenerateTextFilesContentVariableLengthEncoding
    (
        string language,
        string algorithm,
        int count
    )
    {
        Random random = new Random();

        EncodingTableElements encodingTableElements = language == EncodingLanguage.Ukrainian.Name ? UkrainianTableElements
                                                                                                  : EnglishTableElements;                                        

        for (int i = 1; i <= count; i++)
            yield return Init
            (
                GenerateRandomText(encodingTableElements, random, count * 10),
                language,
                algorithm
            ).Value;
    }

    private static string GenerateRandomText(EncodingTableElements encodingTable, Random random, int length)
    {
        StringBuilder text = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            EncodingTableElement randomElement = encodingTable[random.Next(encodingTable.Count)];
            text.Append(randomElement.Symbol);
        }

        return text.ToString();
    }
}

public sealed class EncodingTrainingId : TypeId
{
    public EncodingTrainingId()
        : this(Guid.NewGuid()) { }

    public EncodingTrainingId(Guid value) : base(value) { }

    public static implicit operator EncodingTrainingId(Guid id) => new(id);

    public static implicit operator Guid(EncodingTrainingId id) => id.Value;
    public override string ToString() => Value.ToString();
}
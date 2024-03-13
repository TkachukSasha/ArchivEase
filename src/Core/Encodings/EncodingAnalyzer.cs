using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.ML.Data;
using Microsoft.ML;
using System.Globalization;

namespace Core.Encodings;

public sealed class EncodingAnalyzer
{
    private record EncodingTrainingPredict(string Algorithm, string Content, string Language);

    private readonly List<EncodingTrainingPredict> _trainingData;
    private readonly MLContext _mlContext;
    private readonly PredictionEngine<EncodingData, EncodingPrediction> _predictionEngine;

    private readonly Dictionary<uint, string> _algorithmMappingValues = new Dictionary<uint, string>()
    {
         { 1, EncodingAlgorithm.VariableLengthCodeAlgorithm.Name },
         { 2, EncodingAlgorithm.ShannonFanoAlgorithm.Name },
         { 3, EncodingAlgorithm.HuffmanAlgorithm.Name }
    };

    private readonly Dictionary<uint, string> _algorithmLanguageMappingValues = new Dictionary<uint, string>()
    {
         { 1, EncodingLanguage.Ukrainian.Name },
         { 2, EncodingLanguage.English.Name }
    };

    public EncodingAnalyzer()
    {
        _trainingData = LoadTrainingDataFromCsv("D:\\Projects\\ArchivEase\\src\\encoding_trainings_202403091535.csv");
        _mlContext = new MLContext();

        var trainingDataView = _mlContext.Data.LoadFromEnumerable(_trainingData.Select(x => new EncodingData { Content = x.Content, Algorithm = x.Algorithm, Language = x.Language }));

        var dataProcessPipeline = _mlContext.Transforms.Conversion.MapValueToKey("Label", nameof(EncodingData.Algorithm))
              .Append(_mlContext.Transforms.Conversion.MapValueToKey("LanguageLabel", nameof(EncodingData.Language)))
              .Append(_mlContext.Transforms.Text.FeaturizeText("Features", nameof(EncodingData.Content)));

        var trainer = _mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy();

        var trainingPipeline = dataProcessPipeline.Append(trainer)
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedAlgorithm", "PredictedLabel"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLanguage", "LanguageLabel"));

        var trainedModel = trainingPipeline.Fit(trainingDataView);

        _predictionEngine = _mlContext.Model.CreatePredictionEngine<EncodingData, EncodingPrediction>(trainedModel);
    }

    private List<EncodingTrainingPredict> LoadTrainingDataFromCsv(string filePath)
    {
        var trainingData = new List<EncodingTrainingPredict>();

        using var reader = new StreamReader(filePath);

        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            var algorithm = csv.GetField<string>("Algorithm");
            var language = csv.GetField<string>("Language");
            var content = csv.GetField<string>("Content");

            trainingData.Add(new EncodingTrainingPredict(algorithm, content, language));
        }

        return trainingData;
    }

    public (string Algorithm, string Language) PredictAlgorithmAndLanguage(string fileContent)
    {
        string checkLanguage = fileContent.Contains('я') ? EncodingLanguage.Ukrainian.Name
                                                         : EncodingLanguage.English.Name;

        var prediction = _predictionEngine.Predict(new EncodingData { Content = fileContent });

        _ = _algorithmMappingValues.TryGetValue(prediction.PredictedAlgorithm, out string? algorithmName);

        return (algorithmName ?? string.Empty, string.IsNullOrWhiteSpace(prediction.PredictedLanguage) ? checkLanguage
                                                                                                       : prediction.PredictedLanguage);
    }
}

public class EncodingData
{
    [LoadColumn(0)]
    public string Algorithm { get; set; } = string.Empty;

    [LoadColumn(1)]
    public string Content { get; set; } = string.Empty;

    [LoadColumn(2)]
    public string Language { get; set; } = string.Empty;
}

public class EncodingPrediction
{
    [ColumnName("PredictedLabel")]
    public uint PredictedAlgorithm { get; set; }

    [ColumnName("PredictedLanguage")]
    public string PredictedLanguage { get; set; } = string.Empty;
}
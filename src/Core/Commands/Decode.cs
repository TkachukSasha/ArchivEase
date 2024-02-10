using Core.Encodings.Builders.VariableLengthCode;
using Core.Encodings;
using SharedKernel.Commands;
using SharedKernel.Errors;

namespace Core.Commands;

public sealed record DecodeCommand
(
    string? Text,
    byte[] Data,
    EncodingTableElements EncodingTableElements,
    string Algorithm
) : ICommand<Result>;

internal sealed class DecodeCommandHandler : ICommandHandler<DecodeCommand, Result>
{
    public async Task<Result> HandleAsync(DecodeCommand command, CancellationToken cancellationToken = default)
    {
        string variableLength = EncodingAlgorithm.VariableLengthCodeAlgorithm.Name;

        if (command.Algorithm == variableLength)
        {
            return Result.Success(VariableLengthEncodeAlgorithm(command.Data, command.EncodingTableElements));
        }

        return Result.Success();
    }

    private string VariableLengthEncodeAlgorithm(byte[] data, EncodingTableElements tableElements)
    {
        return VariableLengthCodeDecodeBuilder
            .Init()
            .WithContent(data)
            .WithTableElements(tableElements)
            .PrepareContent()
            .Build();
    }
}

namespace Core.Encodings.Builders.ShannonFano;

internal sealed class SymbolStatistic
{
    internal char Symbol { get; set; }

    internal int Frequency { get; set; }

    internal uint Bits { get; set; }

    internal int Size { get; set; }
}
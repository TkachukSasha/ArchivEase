namespace Core.Encodings.Builders.Huffman;

internal class HuffmanNode
{
    internal char Symbol { get; set; }
    internal int Frequency { get; set; }
    internal HuffmanNode? Left { get; set; }
    internal HuffmanNode? Right { get; set; }
}
using SharedKernel.Abstractions;

namespace Core.Encodings;

public class EncodingAlgorithm : Enumeration<EncodingAlgorithm>
{
    public static readonly EncodingAlgorithm VariableLengthCodeAlgorithm = new EncodingVariableLengthCodeAlgorithm();
    public static readonly EncodingAlgorithm ShannonFanoAlgorithm = new EncodingShannonFanoAlgorithm();
    public static readonly EncodingAlgorithm HuffmanAlgorithm = new EncodingHuffmanAlgorithm();
    public static readonly EncodingAlgorithm GzipAlgorithm = new GZipAlgorithm();

    public EncodingAlgorithm
    (
        Guid value,
        string name
    ) : base(value, name)
    {
    }

    private sealed class EncodingVariableLengthCodeAlgorithm : EncodingAlgorithm
    {
        public EncodingVariableLengthCodeAlgorithm()
            : base(Guid.NewGuid(), "variable_length_code") { }
    }

    private sealed class EncodingShannonFanoAlgorithm : EncodingAlgorithm
    {
        public EncodingShannonFanoAlgorithm()
            : base(Guid.NewGuid(), "shannon_fano") { }
    }

    private sealed class EncodingHuffmanAlgorithm : EncodingAlgorithm
    {
        public EncodingHuffmanAlgorithm()
            : base(Guid.NewGuid(), "huffman") { }
    }

    private sealed class GZipAlgorithm : EncodingAlgorithm
    {
        public GZipAlgorithm()
           : base(Guid.NewGuid(), "gzip") { }
    }
}
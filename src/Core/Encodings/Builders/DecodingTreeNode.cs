using System.Text;

namespace Core.Encodings.Builders;

public class DecodingTreeNode
{
    public char Value { get; set; }
    public DecodingTreeNode? Zero { get; set; }
    public DecodingTreeNode? One { get; set; }

    public DecodingTreeNode Get(IEnumerable<EncodingTableElement> encodingTableElements)
    {
        var root = new DecodingTreeNode();

        foreach (var encoding in encodingTableElements)
            Add(encoding.Code, encoding.Symbol, root);

        return root;
    }

    public string Decode(string content)
    {
        var response = new StringBuilder();

        DecodingTreeNode currentNode = this;

        foreach (char symbol in content)
        {
            switch (symbol)
            {
                case '0':
                    if (currentNode.Zero != null) currentNode = currentNode.Zero;
                    else throw new ArgumentException("Invalid binary code: " + content);
                    break;
                case '1':
                    if (currentNode.One != null) currentNode = currentNode.One;
                    else throw new ArgumentException("Invalid binary code: " + content);
                    break;
                default:
                    throw new ArgumentException("Invalid binary code: " + symbol);
            }

            if (currentNode.Value != '\0')
            {
                response.Append(currentNode.Value);
                currentNode = this;
            }
        }

        return response.ToString();
    }

    private static void Add
    (
        string code,
        char value,
        DecodingTreeNode root
    )
    {
        var currentNode = root;

        foreach (char bit in code)
        {
            switch (bit)
            {
                case '0':
                    if (currentNode.Zero == null) currentNode.Zero = new DecodingTreeNode();
                    currentNode = currentNode.Zero;
                    break;
                case '1':
                    if (currentNode.One == null) currentNode.One = new DecodingTreeNode();
                    currentNode = currentNode.One;
                    break;
                default:
                    throw new ArgumentException("Invalid binary code: " + code);
            }
        }

        currentNode.Value = value;
    }
}
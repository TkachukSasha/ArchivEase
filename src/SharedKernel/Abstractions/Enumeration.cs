using System.Reflection;

namespace SharedKernel.Abstractions;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<Guid, TEnum> Enumerations = CreateEnumerations();

    protected Enumeration(Guid value, string name)
    {
        Value = value;
        Name = name;
    }

    public Guid Value { get; protected init; }

    public string Name { get; protected init; }

    #region methods
    public static TEnum? FromValue(Guid value)
        => Enumerations.TryGetValue(
            value,
            out TEnum? enumeration) ? enumeration
                                    : default;

    public static TEnum? FromName(string name)
        => Enumerations.Values.SingleOrDefault(x => x.Name == name);

    public static IEnumerable<TEnum> GetValues()
        => Enumerations.Values;
    #endregion

    #region overrides
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;

        return GetType() == other.GetType() && Value == other.Value;
    }

    public override bool Equals(object? obj)
        => obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();
    #endregion

    private static Dictionary<Guid, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        return enumerationType.GetFields(
            BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy)
                .Where(fieldInfo =>
                    enumerationType.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo =>
                    (TEnum)fieldInfo.GetValue(default)!)
                .ToDictionary(x => x.Value);
    }
}
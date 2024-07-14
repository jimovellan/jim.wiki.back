namespace jim.wiki.core.Extensions;

public static class TypeExtensions
{
    public static bool ContainsProperty(this Type type, string field)
    {
        return type.GetProperties().Where(wh => wh.Name.Trim().ToLowerInvariant() == field.Trim().ToLowerInvariant()).Any();
    }
}

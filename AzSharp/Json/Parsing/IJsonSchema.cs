namespace AzSharp.Json.Parsing;

public abstract class IJsonSchema
{
    public virtual JsonNode Sanitize(JsonNode node)
    {
        if (!SanitizeNode(node))
        {
            return DefaultNode();
        }
        return node;
    }
    public abstract JsonNode DefaultNode();
    public abstract bool SanitizeNode(JsonNode node);
}

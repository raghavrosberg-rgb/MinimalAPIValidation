namespace MinimalAPIValidationDemo.Utility;

public static class TokenCounter
{
    public static int Count(string input)
    {
        // Simple token count: count words separated by whitespace
        if (string.IsNullOrWhiteSpace(input))
            return 0;
        return input.Split((char[]?)null, System.StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
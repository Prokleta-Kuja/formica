namespace formica;

public static class C
{
    public static string Data => Path.Combine(Environment.CurrentDirectory, "data");
    public static string DataFor(Guid id) => Path.Combine(Data, $"{id}.json");
}
using System.Text.Json;
using System.Text.Json.Serialization;

namespace formica;

public static class C
{
    public static JsonSerializerOptions JsonOpt { get; } = new() { WriteIndented = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault };
    public static Settings Settings { get; set; } = new("host", 587, "user", "pass", "Subject", "Sender <sender@example.com>", "First Last <first.last@example.com>");
    public static string Config => Path.Combine(Environment.CurrentDirectory, "config");
    public static string ConfigFor(string filename) => Path.Combine(Config, filename);
    public static string Data => Path.Combine(Config, "data");
    public static string DataFor(Guid id) => Path.Combine(Data, $"{id}.json");
    public static string DataFor(string filename) => Path.Combine(Data, filename);
}
public class Settings
{
    public Settings(string host, int port, string username, string password, string subject, string from, string to)
    {
        Host = host;
        Port = port;
        Username = username;
        Password = password;
        Subject = subject;
        From = from;
        To = to;
    }

    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Subject { get; set; }
    public string From { get; set; }
    public string To { get; set; }
}
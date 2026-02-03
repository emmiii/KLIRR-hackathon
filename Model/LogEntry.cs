namespace Model;
public class LogEntry
{
    public DateTime Tid { get; set; }
    public string Titel { get; set; } = string.Empty;
    public string Avdelning { get; set; } = string.Empty;
    public string Beskrivning { get; set; } = string.Empty;
    public string Utfall { get; set; } = string.Empty;
    public List<string> Taggar { get; set; } = [];

    public string TaggarDisplay => Taggar.Count > 0 ? $"#{string.Join(" #", Taggar)}" : string.Empty;
}


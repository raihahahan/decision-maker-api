namespace DecisionMakerApi.Common.Domains;

public class Choice
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public long DecisionId { get; set; }
    public string? RefId { get; set; }
}
using CsvHelper.Configuration.Attributes;

public class GeneAbundanceOutput
{
    [Name("sample")]
    public string Sample { get; set; } = "";

    [Name("gene")]
    public string Gene { get; set; } = "";

    [Name("sum")]
    public float Sum { get; set; }
}
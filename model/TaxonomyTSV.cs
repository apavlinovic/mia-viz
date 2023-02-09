using CsvHelper.Configuration.Attributes;

// 							
public class TaxonomyTSV
{
    [Name("Feature ID")]
    public int FeatureId { get; set; }

    [Name("Taxon")]
    public string Taxon { get; set; } = "";

    [Name("Consensus")]
    public float Consensus { get; set; }
}
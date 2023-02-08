using CsvHelper.Configuration.Attributes;

// 							
public class Taxonomy
{
    [Name("Feature ID")]
    public int FeatureId { get; set; }

    [Name("Taxon_Domena")]
    public string Domena { get; set; } = "";

    [Name("Taxon_Phylum")]
    public string Phylum { get; set; } = "";

    [Name("Taxon_class")]
    public string Class { get; set; } = "";

    [Name("Taxon_order")]
    public string Order { get; set; } = "";

    [Name("Taxon_Family")]
    public string Family { get; set; } = "";

    [Name("Taxon_genus")]
    public string Genus { get; set; } = "";

    [Name("Consensus")]
    public float Consensus { get; set; }
}
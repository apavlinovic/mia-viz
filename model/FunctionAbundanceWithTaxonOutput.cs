using CsvHelper.Configuration.Attributes;

// sample	function	taxon	taxon_abun	taxon_rel_abun	genome_function_count	taxon_function_abun	taxon_rel_function_abun	norm_taxon_function_contrib
public class FunctionAbundanceWithTaxonOutput
{
    [Name("sample")]
    public string Sample { get; set; } = "";

    [Name("function")]
    public string Function { get; set; } = "";

    [Name("taxon")]
    public string Taxon { get; set; } = "";

    [Name("sum")]
    public float TaxonSum { get; set; }

    [Name("taxon_contribution")]
    public float TaxonContribution { get; set; }
}
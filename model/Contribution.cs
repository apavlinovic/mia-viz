using CsvHelper.Configuration.Attributes;

// sample	function	taxon	taxon_abun	taxon_rel_abun	genome_function_count	taxon_function_abun	taxon_rel_function_abun	norm_taxon_function_contrib
public class Contribution
{
    [Name("sample")]
    public string Sample { get; set; } = "";

    [Name("function")]
    public string Function { get; set; } = "";

    [Name("taxon")]
    public int Taxon { get; set; }

    [Name("taxon_abun")]
    public float TaxonAbundance { get; set; }

    [Name("taxon_rel_abun")]
    public float TaxonRelativeAbundance { get; set; }

    [Name("genome_function_count")]
    public int GenomeFunctionCount { get; set; }

    [Name("taxon_function_abun")]
    public float TaxonFunctionAbundance { get; set; }

    [Name("taxon_rel_function_abun")]
    public float TaxonRelativeFunctionAbundance { get; set; }

    [Name("norm_taxon_function_contrib")]
    public float NormalisedTaxonFunctionContribution { get; set; }
}
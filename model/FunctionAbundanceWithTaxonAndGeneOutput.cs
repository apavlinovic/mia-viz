using CsvHelper.Configuration.Attributes;

// sample	function	taxon	taxon_abun	taxon_rel_abun	genome_function_count	taxon_function_abun	taxon_rel_function_abun	norm_taxon_function_contrib
public class FunctionAbundanceWithTaxonAndGeneOutput
{
    [Name("sample")]
    public string Sample { get; set; } = "";

    [Name("function")]
    public string Function { get; set; } = "";

    [Name("taxon")]
    public string Taxon { get; set; } = "";

    [Name("gene")]
    public string Gene { get; set; } = "";

    [Name("taxon_sum")]
    public float TaxonSum { get; set; }

    [Name("gene_sum")]
    public float GeneSum { get; set; }

    [Name("gene_contribution_in_taxon")]
    public float GeneContributionInTaxon { get; set; }

    [Name("gene_contribution_in_function")]
    public float GeneContributionInFunction { get; set; }

    [Name("taxon_contribution")]
    public float TaxonContribution { get; set; }
}
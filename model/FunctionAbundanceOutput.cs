using CsvHelper.Configuration.Attributes;

// sample	function	taxon	taxon_abun	taxon_rel_abun	genome_function_count	taxon_function_abun	taxon_rel_function_abun	norm_taxon_function_contrib
public class FunctionAbundanceOutput
{
    [Name("sample")]
    public string Sample { get; set; } = "";

    [Name("function")]
    public string Function { get; set; } = "";

    [Name("sum")]
    public float Sum { get; set; }
}
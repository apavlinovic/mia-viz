using CsvHelper.Configuration.Attributes;

// Function Gene	GeneFullName	GeneVariance	GeneVarianceClass 							
public class Filter
{
    [Name("Function")]
    public string Function { get; set; } = "";

    [Name("Gene")]
    public string Gene { get; set; } = "";

    [Name("GeneFullName")]
    public string GeneFullName { get; set; } = "";

    [Name("GeneVariance")]
    public string GeneVariance { get; set; } = "";

    [Name("GeneVarianceClass")]
    public string GeneVarianceClass { get; set; } = "";
}
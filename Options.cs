using CommandLine;

[Verb("process", HelpText = "Preprocess metagenome prediction files. Filters and decorates with taxon and function names.")]
public class PreProcessOptions
{
    [Option('t', "taxonomy", Required = true, HelpText = "Path to taxonomy.tsv file")]
    public string TaxonomyPath { get; set; } = "";

    [Option('c', "contrib", Required = true, HelpText = "Path to pred_metagenome_contrib.tsv.gz file")]
    public string ContributionsPath { get; set; } = "";

    [Option('d', "description", Required = true, HelpText = "Path to pred_metagenome_unstrat_descrip.tsv.gz file")]
    public string UnstratDescriptionPath { get; set; } = "";
}

[Verb("generate", HelpText = "Generate output files. You need to run process before running this verb")]
public class GenerateOptions
{

}

using System.Diagnostics;
var timer = new Stopwatch();
timer.Start();

// Read contributions
Console.WriteLine("Reading contributions");
var contributions = CSVHelper.ReadCompressedCSV<Contribution>("./input/pred_metagenome_contrib.tsv.gz");
Console.WriteLine("{0} seconds needed to load: \tpred_metagenome_contrib", timer.ElapsedMilliseconds / 1000);

// Read supporting files
timer.Restart();
var unstratDescriptions = CSVHelper.ReadCSV<UnstratDescription>("./input/pred_metagenome_unstrat_descrip.tsv");
var taxonomies = CSVHelper.ReadCSV<Taxonomy>("./input/taxonomy.tsv");
var filters = CSVHelper.ReadCSV<Filter>("./input/filter.tsv");
Console.WriteLine("{0} seconds needed to load: \tpred_metagenome_unstrat_descrip, filters, taxonomy", timer.ElapsedMilliseconds / 1000);

// Extract functions that we want in the output TSV
timer.Restart();
var functionsInFilter = filters.Select(f => f.Function).Distinct().ToDictionary(x => x, x => x);
Console.WriteLine("{0} seconds needed to create filtering dictionary", timer.ElapsedMilliseconds / 1000);

// Filter contributions TSV and report before and after line counts
timer.Restart();
Console.WriteLine("Starting to filter contributions");
Console.WriteLine("Contributions before filtering: \t{0}", contributions.Count());
var filteredContributions = contributions.Where(c => functionsInFilter.ContainsKey(c.Function));
Console.WriteLine("Contributions after filtering: \t{0}", filteredContributions.Count());
Console.WriteLine("{0} seconds needed to filter contributions", timer.ElapsedMilliseconds / 1000);

// Save filtered data
CSVHelper.SaveAsCSV<Contribution>("./output/filteredContributions.tsv", filteredContributions.ToList());

// Inflate data with supporting data
timer.Restart();
Console.WriteLine("Starting to decorate contribution with taxon and function names");
var taxonIdNameDictionary = taxonomies.ToDictionary(x => x.FeatureId, x => x.Genus);
var functionIdNameDictionary = filters.ToDictionary(x => x.Function, x => x.GeneVarianceClass);

var decoratedContributions = filteredContributions.Select(f => new DecoratedContribution()
{
    Sample = f.Sample,
    // Overriden
    Function = functionIdNameDictionary.GetValueOrDefault(f.Function, "NOT_FOUND").Trim(),
    Taxon = taxonIdNameDictionary.GetValueOrDefault(f.Taxon, "NOT_FOUND").Trim(),

    TaxonId = f.Taxon,
    FunctionId = f.Function,

    GenomeFunctionCount = f.GenomeFunctionCount,
    NormalisedTaxonFunctionContribution = f.NormalisedTaxonFunctionContribution,
    TaxonAbundance = f.TaxonAbundance,
    TaxonFunctionAbundance = f.TaxonFunctionAbundance,
    TaxonRelativeAbundance = f.TaxonRelativeAbundance,
    TaxonRelativeFunctionAbundance = f.TaxonRelativeFunctionAbundance,
});

CSVHelper.SaveAsCSV<DecoratedContribution>("./output/decoratedContributions.tsv", decoratedContributions.ToList());
Console.WriteLine("{0} seconds needed to decorate contributions", timer.ElapsedMilliseconds / 1000);

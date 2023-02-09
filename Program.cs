using System.Diagnostics;
using CommandLine;

namespace MiaViz
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Create output directory if it doesn't exist
            System.IO.Directory.CreateDirectory("./output");

            // Map Command Line options to their handler
            CommandLine.Parser.Default.ParseArguments<PreProcessOptions, GenerateOptions>(args).MapResult(
                (PreProcessOptions opts) => ProcessContributions(opts),
                (GenerateOptions opts) => GenerateOutputs(opts),
                errs => 1
            );
        }

        static int ProcessContributions(PreProcessOptions options)
        {
            try
            {
                var timer = new Stopwatch();
                timer.Start();

                // Read contributions
                Console.WriteLine("Reading contributions");
                var filters = CSVHelper.ReadCSV<ObservedFunctionsFilter>("./observedFunctions.tsv").DistinctBy(f => f.Function);
                var contributions = CSVHelper.ReadCompressedCSV<Contribution>(options.ContributionsPath);
                var unstratDescriptions = CSVHelper.ReadCompressedCSV<UnstratDescription>(options.UnstratDescriptionPath);
                var taxonomiesTSV = CSVHelper.ReadCSV<TaxonomyTSV>(options.TaxonomyPath);
                var taxonomies = taxonomiesTSV.Select(t =>
                {
                    var taxonTree = t.Taxon.Split("; ");

                    return new Taxonomy()
                    {
                        FeatureId = t.FeatureId,
                        Domena = taxonTree.FirstOrDefault(s => s.StartsWith("d__"), "").Replace("d__", ""),
                        Phylum = taxonTree.FirstOrDefault(s => s.StartsWith("p__"), "").Replace("p__", ""),
                        Class = taxonTree.FirstOrDefault(s => s.StartsWith("c__"), "").Replace("c__", ""),
                        Order = taxonTree.FirstOrDefault(s => s.StartsWith("o__"), "").Replace("o__", ""),
                        Family = taxonTree.FirstOrDefault(s => s.StartsWith("f__"), "").Replace("f__", ""),
                        Genus = taxonTree.FirstOrDefault(s => s.StartsWith("g__"), "").Replace("g__", ""),
                        Consensus = t.Consensus
                    };
                });

                Console.WriteLine("{0} seconds needed to load: \ttpred_metagenome_contrib, pred_metagenome_unstrat_descrip, observedFunctionsFilter, and taxonomy", timer.ElapsedMilliseconds / 1000);

                // Extract functions that we want in the output TSV
                var functionToGeneVarianceDictionary = filters.ToDictionary(x => x.Function, x => x.GeneVariance);
                var functionToGeneDictionary = filters.ToDictionary(x => x.Function, x => x.Gene);

                // Filter contributions TSV and report before and after line counts
                var filteredContributions = contributions.Where(c => functionToGeneVarianceDictionary.ContainsKey(c.Function));

                // Save filtered data
                CSVHelper.SaveAsCSV<Contribution>("./output/1-filteredContributions.tsv", filteredContributions.ToList());
                Console.WriteLine("Saved filteredContributions to ./output/1-filteredContributions.tsv");

                // Decorate filtered data
                Console.WriteLine("Starting to decorate contribution with taxon and function names");
                var taxonIdNameDictionary = taxonomies.ToDictionary(x => x.FeatureId, x => x.Genus);

                var decoratedContributions = filteredContributions.Select(f => new DecoratedContribution()
                {
                    Sample = f.Sample,
                    // Overriden
                    Function = functionToGeneVarianceDictionary.GetValueOrDefault(f.Function, "NOT_FOUND").Trim(),
                    Gene = functionToGeneDictionary.GetValueOrDefault(f.Function, "NOT_FOUND").Trim(),
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

                CSVHelper.SaveAsCSV<DecoratedContribution>("./output/2-decoratedContributions.tsv", decoratedContributions.ToList());
                Console.WriteLine("Saved decoratedContributions to ./output/2-decoratedContributions.tsv");

                return 1;
            }
            catch (System.Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Processing metagenome failed!");
                Console.WriteLine("ERROR:");
                Console.WriteLine();
                Console.ResetColor();
                throw;
            }
        }

        static int GenerateOutputs(GenerateOptions options)
        {
            try
            {
                var functionAbundanceOutputsPath = Path.Combine(Environment.CurrentDirectory, "output/3-functionAbundanceOutputs.tsv");
                var decContributions = CSVHelper.ReadCSV<DecoratedContribution>("./output/2-decoratedContributions.tsv");

                var functionAbundanceOutputs = new List<FunctionAbundanceOutput>();
                var groupBySampleLocation = decContributions.GroupBy(x => x.Sample);
                foreach (var samplesForLocation in groupBySampleLocation)
                {
                    var functionGroups = samplesForLocation.GroupBy(m => m.Function);
                    foreach (var functionG in functionGroups)
                    {
                        if (options.Taxons.Any()
                        && !options.Taxons.Contains(functionG.First().Taxon))
                        {
                            continue;
                        }

                        var functionSum = functionG.Sum(x => x.TaxonFunctionAbundance);

                        functionAbundanceOutputs.Add(new FunctionAbundanceOutput()
                        {
                            Sample = samplesForLocation.Key,
                            Function = functionG.Key,
                            Sum = functionSum
                        });
                    }
                }

                CSVHelper.SaveAsCSV(functionAbundanceOutputsPath, functionAbundanceOutputs);
                Console.WriteLine("Saved functionAbundanceOutputs to:");
                Console.WriteLine(functionAbundanceOutputsPath);

                //

                var geneAbundanceOutputsPath = Path.Combine(Environment.CurrentDirectory, "output/4-geneAbundanceOutputs.tsv");
                var geneAbundanceOutputs = new List<GeneAbundanceOutput>();
                foreach (var samplesForLocation in groupBySampleLocation)
                {
                    var geneGroups = samplesForLocation.GroupBy(m => m.Gene);
                    foreach (var geneG in geneGroups)
                    {
                        if (options.Taxons.Any()
                        && !options.Taxons.Contains(geneG.First().Taxon))
                        {
                            continue;
                        }

                        var geneSum = geneG.Sum(x => x.TaxonFunctionAbundance);

                        geneAbundanceOutputs.Add(new GeneAbundanceOutput()
                        {
                            Sample = samplesForLocation.Key,
                            Gene = geneG.Key,
                            Sum = geneSum
                        });
                    }
                }

                CSVHelper.SaveAsCSV(geneAbundanceOutputsPath, geneAbundanceOutputs);
                Console.WriteLine("Saved geneAbundanceOutputs to:");
                Console.WriteLine(geneAbundanceOutputsPath);

                //


                var functionAbundanceWithTaxonOutputsPath = Path.Combine(Environment.CurrentDirectory, "output/5-functionAbundanceWithTaxonOutputs.tsv");
                var functionAbundanceWithTaxonOutputs = new List<FunctionAbundanceWithTaxonOutput>();

                foreach (var samplesForLocation in groupBySampleLocation)
                {
                    var functionGroups = samplesForLocation.GroupBy(m => m.Function);
                    foreach (var functionG in functionGroups)
                    {
                        var functionSum = functionG.Sum(x => x.TaxonFunctionAbundance);
                        var taxonGroups = functionG.GroupBy(g => g.Taxon);
                        foreach (var taxonGroup in taxonGroups)
                        {
                            var taxonSum = taxonGroup.Sum(x => x.TaxonFunctionAbundance);

                            if (options.Taxons.Any()
                            && !options.Taxons.Contains(taxonGroup.Key))
                            {
                                continue;
                            }

                            functionAbundanceWithTaxonOutputs.Add(new FunctionAbundanceWithTaxonOutput()
                            {
                                Sample = samplesForLocation.Key,
                                Function = functionG.Key,
                                Taxon = taxonGroup.Key,
                                TaxonSum = taxonSum,
                                TaxonContribution = taxonSum / functionSum
                            });
                        }

                    }
                }

                CSVHelper.SaveAsCSV(functionAbundanceWithTaxonOutputsPath, functionAbundanceWithTaxonOutputs);
                Console.WriteLine("Saved functionAbundanceWithTaxonOutputs to:");
                Console.WriteLine(functionAbundanceWithTaxonOutputsPath);
                return 1;
            }
            catch (System.Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Generating outputs failed!");
                Console.WriteLine("ERROR:");
                Console.WriteLine();
                Console.ResetColor();
                throw;
            }
        }
    }
}
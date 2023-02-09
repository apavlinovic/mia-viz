# MaiViz – a tool for processing metagenome prediction files.

This tool can open metagenome files, filter them by taxonomy, and output filtered results for further analysis. The tool can also do some basical statistcial analysis, like summing the function abundance per sample location, and function abundance per sample location + taxonomy.

## How to use

If you are building from source, run `dotnet run` to build, restore and run the tool. The tool provides you with two actions:

- process - parses the metagenome input files (contribution and description), filters them by taxonomy, and outputs filtered result
- generate - uses the filtered result to run a simple statistical analysis and outputs visualisation-friendly results

All result files are valid TSV files.

## Commands

#### process – processing metagenome files*

`dotnet run process -- -c "/path/to/contribution.tsv.gz" -d "/path/to/description.tsv.gz" -t "/path/to/taxonomy.tsv"`

Parameters:

- -c -> path to contribution tsv.gz file
- -d -> path to description tsv.gz file
- -t -> path to taxonomy tsv file

### generate - generating output files

`dotnet run generate -- -t Microbacil Macrobacil`

Parameters:
- -t -> list of taxons to include in the generate statistical analysis files

## Outputs

All output files can be found in the tool's ./output subdirectory.

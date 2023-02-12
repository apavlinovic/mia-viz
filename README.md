# MaiViz – a tool for processing metagenome prediction files.

This tool can open metagenome files, filter them by taxonomy, and output filtered results for further analysis. The tool can also do some basical statistcial analysis, like summing the function abundance per sample location, and function abundance per sample location + taxonomy.

## How to use

If you are building from source, run `dotnet run` to build, restore and run the tool. The tool provides you with two actions:

-   process - parses the metagenome input files (contribution and description), filters them by taxonomy, and outputs filtered result
-   generate - uses the filtered result to run a simple statistical analysis and outputs visualisation-friendly results

All result files are valid TSV files.

## Commands

#### process – processing metagenome files\*

**Running from Windows**

`mia-viz.exe process -c "/path/to/contribution.tsv.gz" -t "/path/to/taxonomy.tsv"`

Running from source

`dotnet run process -- -c "/path/to/contribution.tsv.gz" -t "/path/to/taxonomy.tsv"`

Parameters:

-   -c -> path to contribution tsv.gz file
-   -t -> path to taxonomy tsv file

### generate - generating output files

**Running from Windows**

`mia-viz.exe generate -t Microbacil Macrobacil`

Running from source

`dotnet run generate -- -t Microbacil Macrobacil`

Parameters:

-   -t -> list of taxons to include in the generate statistical analysis files

## Outputs

All output files can be found in the tool's ./output subdirectory.

## How to publish an EXE version

Run the following command to build a self contained, Win 64 or OSX ARM 64 compatible executable
You can find the Release ID nomenclature here: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog

`dotnet publish -c Release -r win-x64 --self-contained true`

`dotnet publish -c Release -r osx.13-arm64 --self-contained true`

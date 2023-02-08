using CsvHelper.Configuration.Attributes;

public class UnstratDescription
{
    // function	description	KBB.SXT	KBS.SXT	KS.SXT	SCB.SXT	SCS.SXT	STB.SXT	STS.SXT	VB.SXT	VS.SXT	J.SXT
    [Name("function")]
    public string Function { get; set; } = "";

    [Name("description")]
    public string Description { get; set; } = "";

    [Name("KBB.SXT")]
    public float KBB { get; set; }

    [Name("KBS.SXT")]
    public float KBS { get; set; }

    [Name("KS.SXT")]
    public float KS { get; set; }

    [Name("SCB.SXT")]
    public float SCB { get; set; }

    [Name("SCS.SXT")]
    public float SCS { get; set; }

    [Name("STB.SXT")]
    public float STB { get; set; }

    [Name("STS.SXT")]
    public float STS { get; set; }

    [Name("VB.SXT")]
    public float VB { get; set; }

    [Name("VS.SXT")]
    public float VS { get; set; }

    [Name("J.SXT")]
    public float J { get; set; }
}
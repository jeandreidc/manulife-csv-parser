// See https://aka.ms/new-console-template for more information


using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using manulife_csv_parser.model;



var chinabankModel = new AwardOutputModel
{
    DisplayName = "Chinabank",
    HomeLink = "chinabank",
    ImageSrc = "",
    TagLine = "Congratulations to all Chinabank and Chinabank Savings Awardees!",
    Clubs = [],
    NonClubs = [],
    SourceFile = @"files/chinabank.csv"
};


var manulifeModel = new AwardOutputModel
{
    DisplayName = "MCBL",
    HomeLink = "manulife",
    ImageSrc = "",
    TagLine = "Congratulations to all MCBL Awardees!",
    Clubs = [],
    NonClubs = [],
    SourceFile = @"files/mcbl.csv"
};

var serializeOptions = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};

var manulifeJson = JsonSerializer.Serialize(ExtractFromCsv(manulifeModel), serializeOptions);
var citibankJson = JsonSerializer.Serialize(ExtractFromCsv(chinabankModel), serializeOptions);
var sequenceJson = JsonSerializer.Serialize(ExtractFromCsvAwardSequence("files/sequence.csv"), serializeOptions);
File.WriteAllText(@"manulife.json", manulifeJson);
File.WriteAllText(@"chinabank.json", citibankJson);
File.WriteAllText(@"sequence.json", sequenceJson);
return;

AwardOutputModel ExtractFromCsv(AwardOutputModel awardOutputModel)
{
    var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false,
        MissingFieldFound = null,
    };
    
    using var reader = new StreamReader(awardOutputModel.SourceFile);
    using var csv = new CsvReader(reader, csvConfiguration);
    csv.Context.RegisterClassMap<AwardMap>();
    csv.Context.RegisterClassMap<WinnerMap>();
    Award? currentAward = null;
    while (csv.Read())
    {
        switch (csv.GetField(0))
        {
            case "Award":
                currentAward = awardOutputModel.AddAward(csv.GetRecord<AwardCsvModel>());
                break;
            case "Winner":
                currentAward?.AddAwardee(csv.GetRecord<WinnerCsvModel>());
                break;
            default:
                continue;
        }
    }

    return awardOutputModel;
}


List<SequenceAward> ExtractFromCsvAwardSequence(string fileSource)
{
    var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = false,
        MissingFieldFound = null,
    };
    
    using var reader = new StreamReader(fileSource);
    using var csv = new CsvReader(reader, csvConfiguration);
    List<SequenceAward> sequenceAwards = [];
    SequenceAward? currentAward = null;
    while (csv.Read())
    {
        switch (csv.GetField(0).ToLower())
        {
            case "award":
                currentAward = new SequenceAward(csv.GetField(1));
                sequenceAwards.Add(currentAward);
                break;
            case "winner":
                var winner = new SequenceWinner(csv.GetField(1), csv.GetField(2));
                var awardCount = 3;
                while (csv.TryGetField(awardCount++, out string award))
                {
                    if (string.IsNullOrWhiteSpace(award))
                        break;
                    
                    winner.Awards.Add(award);
                }
                currentAward!.Winners.Add(winner);
                break;
            default:
                continue;
        }
    }

    return sequenceAwards;
}
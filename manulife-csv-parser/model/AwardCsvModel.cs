using CsvHelper.Configuration;

namespace manulife_csv_parser.model;

public class AwardCsvModel
{
    public string Club { get; set; }
    public string Qualifier { get; set; }
    public string AwardName { get; set; }
    public int? RankLimit { get; set; }
}

public sealed class AwardMap : ClassMap<AwardCsvModel>
{
    public AwardMap()
    {
        Map(m => m.Club).Index(1);
        Map(m => m.Qualifier).Index(2);
        Map(m => m.AwardName).Index(3);
        Map(m => m.RankLimit).Optional().Index(4);
    }
}
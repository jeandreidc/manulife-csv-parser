using CsvHelper.Configuration;

namespace manulife_csv_parser.model;

public class WinnerCsvModel
{
    public string Name { get; set; }
    public string Branch { get; set; }
    public string PhotoUrl { get; set; }
}

public sealed class WinnerMap : ClassMap<WinnerCsvModel>
{
    public WinnerMap()
    {
        Map(m => m.Name).Index(1);
        Map(m => m.Branch).Index(2);
        Map(m => m.PhotoUrl).Index(3);
    }
}
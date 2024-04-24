using CsvHelper.Configuration;

namespace manulife_csv_parser.model;
public class SequenceAward
{
    public SequenceAward(string clubAward)
    {
        ClubAward = clubAward;
        Winners = [];
    }
    public string ClubAward { get; set; }
    public List<SequenceWinner> Winners {
        get;
        set;
    }
}

public class SequenceAwardCsvModel
{
    public string ClubAward { get; set; }
}


public class SequenceWinner
{
    public SequenceWinner(string sequenceNumber, string name)
    {
        SequenceNumber = sequenceNumber;
        Name = name;
        Awards = [];
    }
    public string SequenceNumber { get; set; }
    public string Name { get; set; }
    public List<string> Awards { get; set; }
}

public sealed class SequenceAwardMap : ClassMap<SequenceAwardCsvModel>
{
    public SequenceAwardMap()
    {
        Map(m => m.ClubAward).Index(1);
    }
}

public sealed class SequenceWinnerMap : ClassMap<SequenceWinner>
{
    public SequenceWinnerMap()
    {
        Map(m => m.SequenceNumber).Index(1);
        Map(m => m.Name).Index(2);
        Map(m => m.Awards).Index(3);
    }
}
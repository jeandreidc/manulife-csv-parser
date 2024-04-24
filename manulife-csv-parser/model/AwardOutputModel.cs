namespace manulife_csv_parser.model;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class AwardOutputModel
{
    public string DisplayName { get; set; }
    public string HomeLink { get; set; }
    public string ImageSrc { get; set; }
    public string TagLine { get; set; }
    public HashSet<Club> Clubs { get; set; }
    public HashSet<Club> NonClubs { get; set; }
    public string SourceFile { get; set; }

    public Award AddAward(AwardCsvModel newAward)
    {
        var clubToUse = Clubs;
        var clubNameToUse = newAward.Club;
        var containsHonor = newAward.AwardName.Contains("Honor");
        var containsHof = newAward.AwardName.Contains("Hall of Fame");
        if (containsHonor || containsHof)
        {
            clubToUse = NonClubs;
            clubNameToUse = containsHonor ? "Honor Club" : "Hall of Fame";
        }
        
        var club = clubToUse.FirstOrDefault(c => c.Name == clubNameToUse);
        if (club == null)
        {
            club = new Club(clubNameToUse);
            clubToUse.Add(club);
        }

        var qualifier = club.Qualifiers.FirstOrDefault(q => q.Name == newAward.Qualifier);

        if (qualifier == null)
        {
            qualifier = new Qualifier(newAward.Qualifier);
            club.Qualifiers.Add(qualifier);
        }

        var award = qualifier.Awards.FirstOrDefault(a => a.Name == newAward.AwardName);
        if (award == null)
        {
            award = new Award(newAward.AwardName);
            award.RankLimit = newAward.RankLimit;
            qualifier.Awards.Add(award);
        }
        
        return award;
    }
}
public class Award
{
    public string Name { get; set; }
    public HashSet<Awardee> Awardees { get; set; }
    
    public int? RankLimit { get; set; }
    public Award(string awardName)
    {
        Name = awardName;
        Awardees = [];
    }
    
    public void AddAwardee(WinnerCsvModel winner)
    {
        Awardees.Add(new Awardee
        {
            Name = winner.Name,
            Branch = winner.Branch,
            PhotoUrl = winner.PhotoUrl
        });
    }
}

public class Awardee
{
    public string Name { get; set; }
    public string Branch { get; set; }
    public string PhotoUrl { get; set; }
}

public class Club
{
    public string Name { get; set; }
    public HashSet<Qualifier> Qualifiers { get; set; }

    public Club(string name)
    {
        Name = name;
        Qualifiers = [];
    }
}
public class Qualifier
{
    public string Name { get; set; }
    public HashSet<Award> Awards { get; set; }

    public Qualifier(string name)
    {
        Name = name;
        Awards = [];
    }
}
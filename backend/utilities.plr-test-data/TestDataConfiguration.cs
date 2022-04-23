namespace PlrTestData;

public class TestDataConfiguration
{
    public ConnectionStringConfiguration ConnectionStrings { get; set; } = new();
    public List<string> PlrCollegeIdentifiers { get; set; } = new();
    public List<CardData> TestCards { get; set; } = new();

    // ------- Configuration Objects -------
    public class ConnectionStringConfiguration
    {
        public string PlrDatabase { get; set; } = string.Empty;
    }

    public class CardData
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Birthdate { get; set; } = string.Empty;
    }
}

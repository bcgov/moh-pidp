namespace PlrTestData.Data;

public record TestCard(int Id, string FirstName, string LastName, string Birthdate);

public static partial class TestData
{
    public static IEnumerable<TestCard> Cards => new TestCard[]
    {
        new(1,  "PIDP", "ONE",       "1999-12-30"),
        new(2,  "PIDP", "TWO",       "2002-08-18"),
        new(3,  "PIDP", "THREE",     "2002-02-22"),
        new(4,  "PIDP", "FOUR",      "2000-03-22"),
        new(5,  "PIDP", "FIVE",      "2000-06-07"),
        new(6,  "PIDP", "SIX",       "2001-01-17"),
        new(7,  "PIDP", "SEVEN",     "1999-11-26"),
        new(8,  "PIDP", "EIGHT",     "2000-02-19"),
        new(9,  "PIDP", "NINE",      "2000-01-22"),
        new(10, "PIDP", "TEN",       "2002-02-23"),
        new(11, "PIDP", "ELEVEN",    "2000-08-23"),
        new(12, "PIDP", "TWELVE",    "2002-06-06"),
        new(13, "PIDP", "THIRTEEN",  "2002-06-11"),
        new(14, "PIDP", "FOURTEEN",  "2000-12-21"),
        new(15, "PIDP", "FIFTEEN",   "1999-12-05"),
        new(16, "PIDP", "SIXTEEN",   "2002-06-23"),
        new(17, "PIDP", "SEVENTEEN", "2002-03-04"),
        new(18, "PIDP", "EIGHTEEN",  "2000-06-29"),
        new(19, "PIDP", "NINETEEN",  "2001-01-11"),
        new(20, "PIDP", "TWENTY",    "2000-04-01")
    };
}

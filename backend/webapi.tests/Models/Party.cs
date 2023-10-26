namespace PidpTests.Models;

using Pidp.Models;
using Xunit;

public class PartyTests
{
    [Fact]
    public void PreferredNamesFilledIn_DisplayNameShouldUsePreferredNames()
    {
        // Arrange
        var party = new Party()
        {
            FirstName = "John",
            LastName = "Dewayne",
            PreferredFirstName = "Jo",
            PreferredMiddleName = "Jr.",
            PreferredLastName = "Doe",
        };

        // Assert
        Assert.Equal("John Dewayne", party.FullName);
        Assert.Equal("Jo", party.DisplayFirstName);
        Assert.Equal("Doe", party.DisplayLastName);
        Assert.Equal("Jo Doe", party.DisplayFullName);
    }

    [Fact]
    public void PreferredNamesNotFilledIn_DisplayNameShouldUseRegularNames()
    {
        // Arrange
        var party = new Party()
        {
            FirstName = "John",
            LastName = "Dewayne",
        };

        // Assert
        Assert.Equal("John Dewayne", party.FullName);
        Assert.Equal("John", party.DisplayFirstName);
        Assert.Equal("Dewayne", party.DisplayLastName);
        Assert.Equal("John Dewayne", party.DisplayFullName);
    }
}

namespace Pidp.Infrastructure.HttpClients.BCProvider;

public class UserRepresentation
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

// TODO define shema extension

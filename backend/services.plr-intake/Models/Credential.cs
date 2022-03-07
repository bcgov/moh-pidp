namespace PlrIntake.Models;

public class Credential : BaseAuditable
{
    public Credential(string value) => this.Value = value;
    public string Value { get; set; }
}

namespace PlrIntake.Models;

public class Expertise : BaseAuditable
{
    public Expertise(string code) => this.Code = code;
    public string Code { get; set; }
}

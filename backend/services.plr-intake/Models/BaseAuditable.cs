namespace PlrIntake.Models;

public abstract class BaseAuditable
{
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}

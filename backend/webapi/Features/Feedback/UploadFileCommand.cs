namespace Pidp.Features.Feedback;

public class UploadFileCommand(string feedback, IFormFile file) : ICommand<string>
{
    public UploadFileCommand() : this(string.Empty, null) { }
    public string Feedback { get; set; } = feedback;
    public IFormFile? File { get; set; } = file;
}

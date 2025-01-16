namespace Pidp.Features.Feedback;

using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

public class UploadFileCommandHandler(IEmailService emailService) : ICommandHandler<UploadFileCommand, string>
{
    private readonly IEmailService emailService = emailService;


    public async Task<string> HandleAsync(UploadFileCommand command)
    {
        if (command.File == null || command.File.Length == 0)
        {
            throw new ArgumentException("File is not provided or empty.");
        }

        // Convert the file to a byte array
        var fileBytes = await ConvertToByteArrayAsync(command.File);

        var email = new Email(
        from: EmailService.PidpEmail,
        to: ["vinodkakarla5642@gmail.com"],
        cc: ["vinodkakarla564@gmail.com"],
        subject: $"OneHealthID Feedback",
        body: $@"Hello,<br> {command.Feedback} Thank you.",
        attachments: [new File(
            filename: command.File.FileName,
            data: fileBytes,
            mediaType: command.File.ContentType
        )]
        );

        await this.emailService.SendAsync(email);

        return "success";
    }

    private static async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

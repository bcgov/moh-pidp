namespace Pidp.Features.Feedback;

using FluentValidation;

using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

public class Create
{

    public class Command : ICommand<Model>
    {
        public string Feedback { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
        public string FullName { get; set; } = string.Empty;
    }
    public class Model
    {
        public string Status { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Feedback).NotEmpty();
    }

    public class CommandHandler(IEmailService emailService) : ICommandHandler<Command, Model>
    {
        private readonly IEmailService emailService = emailService;

        public async Task<Model> HandleAsync(Command command)
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
                cc: [command.Email],
                subject: $"Feedback from {command.FullName}",
                body: $@"Hello,<br>
                Feedback from user: {command.Email} <br>
                message: {command.Feedback} <br>
                contact: {command.Email} <br>
                Thank you.",
                attachments: [new File(
                    filename: command.File.FileName,
                    data: fileBytes,
                    mediaType: command.File.ContentType
                )]
            );

            await this.emailService.SendAsync(email);

            return new Model { Status = "success" };
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
}

namespace Pidp.Features.Feedback;

using FluentValidation;

using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

public class FileUploadHandler
{

    public class Command
    {
        public string Feedback { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }
    public class Model
    {
        public string Status { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Feedback).NotEmpty();
    }

    // public class CommandHandler(IEmailService emailService) : ICommandHandler<Command, Model>
    // {
    //     private readonly IEmailService emailService = emailService;

    //     public async Task<Model> HandleAsync(Command command)
    //     {
    //         var email = new Email(
    //             from: EmailService.PidpEmail,
    //             to: ["vinodkakarla5642@gmail.com"],
    //             cc: ["vinodkakarla564@gmail.com"],
    //             subject: $"OneHealthID Feedback POC",
    //             body: $@"Hello,<br>Thank you.",
    //             attachments: [new File(
    //                 filename: "test.pdf",
    //                 data: System.IO.File.ReadAllBytes("Features/Feedback/Employee or contractor acknowledgement of GAI Policy.pdf"),
    //                 mediaType: "application/pdf"
    //             )]
    //             );

    //         await this.emailService.SendAsync(email);


    //         return new Model { Status = "success" };
    //     }
    // }
}

namespace Pidp.Features.Feedback;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class Create
{
    public class Command : ICommand
    {
        public int PartyId { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public IFormFile? File { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Feedback).NotEmpty().Length(1, 1000);
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.File)
                .Must(file => file?.Length < 5 * 1024 * 1024).WithMessage("File size should be less than 5MB")
                .When(file => file != null)
                .Must(file => IsValidContentType(file?.ContentType)).WithMessage("Invalid file type. Only Image files are allowed.")
                .When(file => file != null);
        }

        private static bool IsValidContentType(string? contentType)
        {
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            return allowedContentTypes.Contains(contentType);
        }
    }

    public class CommandHandler(IEmailService emailService, PidpDbContext context) : ICommandHandler<Command>
    {
        private readonly IEmailService emailService = emailService;
        private readonly PidpDbContext context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.DisplayFullName,
                    party.Email
                })
                .SingleAsync();

            var email = new Email(
                from: EmailService.PidpEmail,
                to: ["onehealthidsupport@gov.bc.ca"],
                cc: party.Email is null ? [] : [party.Email],
                subject: $"Feedback from {party.DisplayFullName}",
                body: $@"Hello,<br> <br>
                Feedback from user: {party.Email} <br>
                message: {command.Feedback} <br>
                contact: {party.Email} <br> <br>
                Thank you.",
                attachments: command.File is null ? [] : [new File(
                    filename: command.File.FileName,
                    data: await ConvertToByteArrayAsync(command.File),
                    mediaType: command.File.ContentType
                )]
            );

            await this.emailService.SendAsync(email);

            var feedbackLog = new FeedbackLog
            {
                PartyId = command.PartyId,
                Feedback = command.Feedback,
                AttachmentInformation = command.File is null ? null : $"Attachment with name {command.File.FileName} and size of {command.File.Length} bytes sent"
            };

            this.context.FeedbackLogs.Add(feedbackLog);
            await this.context.SaveChangesAsync();
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

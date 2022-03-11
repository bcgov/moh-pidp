namespace Pidp.Features.Parties;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Infrastructure;
using Pidp.Models.ProfileStatus;
using Pidp.Infrastructure.Services;

public class ProfileStatus
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        public HashSet<Alert> Alerts { get; set; } = new();
        [JsonConverter(typeof(PolymorphicDictionarySerializer<string, IProfileSection>))]
        public Dictionary<string, IProfileSection> Status { get; set; } = new();
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<Model>>
    {
        private readonly IProfileStatusService profileService;

        public CommandHandler(IProfileStatusService profileService) => this.profileService = profileService;

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var sections = await this.profileService.CompileSectionsAsync(command.Id, recheckIpc: true, Section.AllSections.ToArray());

            return DomainResult.Success(new Model
            {
                Alerts = new(sections.SelectMany(section => section.Alerts)),
                Status = sections.ToDictionary(x => x.Name, x => x)
            });
        }
    }
}

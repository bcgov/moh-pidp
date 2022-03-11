namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Features.Parties.ProfileStatusInternal;
using Pidp.Infrastructure;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;
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
        private readonly IPlrClient client;
        private readonly IProfileStatusService profileService;
        private readonly PidpDbContext context;

        public CommandHandler(
            IPlrClient client,
            IProfileStatusService profileService,
            PidpDbContext context
            )
        {
            this.client = client;
            this.profileService = profileService;
            this.context = context;
        }

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            // Cert could have been entered but no IPC found, likely due to a transient error or delay in PLR record updates. Retry once.
            await this.RecheckIpc(command.Id);

            var sections = await this.profileService.CompileSectionsAsync(command.Id, Section.AllSections.ToArray());

            return DomainResult.Success(new Model
            {
                Alerts = new(sections.SelectMany(section => section.Alerts)),
                Status = sections.ToDictionary(x => x.Name, x => x)
            });
        }

        private async Task RecheckIpc(int partyId)
        {
            // var cert = await this.context.PartyCertifications
            //     .SingleOrDefaultAsync(cert => cert.PartyId == partyId);

            // if (cert == null
            //     || ())
            // {
            //     return;
            // }

            // if (profile.Ipc == null
            //                 && profile.CollegeCode != null
            //                 && profile.LicenceNumber != null)
            // {
            //     profile.Ipc = await this.RecheckIpc(command.Id, profile.CollegeCode.Value, profile.LicenceNumber, profile.Birthdate);
            // }


            // var newIpc = await this.client.GetPlrRecord(collegeCode, licenceNumber, birthdate);
            // if (newIpc != null)
            // {
            //     var cert = await this.context.PartyCertifications
            //         .SingleAsync(cert => cert.PartyId == partyId);
            //     cert.Ipc = newIpc;
            //     await this.context.SaveChangesAsync();
            // }

            // return newIpc;
        }
    }
}

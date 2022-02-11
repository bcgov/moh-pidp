namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public class ProfileStatus
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CollegeCode? CollegeCode { get; set; }
        public string? LicenceNumber { get; set; }
        [JsonIgnore]
        public string? Ipc { get; set; }
        public string? JobTitle { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityStreet { get; set; }

        public Dictionary<string, SectionStatus> Status { get; set; } = new();

        public class SectionStatus
        {
            public StatusCode StatusCode { get; set; }
            public string? Reason { get; set; }
        }

        public enum StatusCode
        {
            Available = 1,
            Completed,
            NotAvailable,
            Error
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<Model>>
    {
        private readonly IMapper mapper;
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public CommandHandler(
            IMapper mapper,
            IPlrClient client,
            PidpDbContext context)
        {
            this.mapper = mapper;
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var model = await this.context.Parties
                .Where(party => party.Id == command.Id)
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return DomainResult.NotFound<Model>();
            }

            var demographicsStatus = model.Email != null && model.Phone != null
                ? Model.StatusCode.Completed
                : Model.StatusCode.Available;

            var certificationStatus = demographicsStatus != Model.StatusCode.Completed
                ? Model.StatusCode.NotAvailable
                : model.CollegeCode == null || model.LicenceNumber == null
                    ? Model.StatusCode.Available
                    : model.Ipc != null
                        ? Model.StatusCode.Completed
                        : Model.StatusCode.Error;

            var saStatus = await this.ComputeSaStatus(model.Id, model.Ipc);

            model.Status.Add("demographics", new Model.SectionStatus { StatusCode = demographicsStatus });
            model.Status.Add("collegeCertification", new Model.SectionStatus { StatusCode = certificationStatus });
            model.Status.Add("saEforms", saStatus);

            return DomainResult.Success(model);
        }

        private async Task<Model.SectionStatus> ComputeSaStatus(int partyId, string? ipc)
        {
            var accessGranted = await this.context.AccessRequests
                .AnyAsync(access => access.PartyId == partyId
                    && access.AccessType == Models.AccessType.SAEforms);

            if (accessGranted)
            {
                return new Model.SectionStatus
                {
                    StatusCode = Model.StatusCode.Completed
                };
            }

            if (ipc == null)
            {
                return new Model.SectionStatus
                {
                    StatusCode = Model.StatusCode.NotAvailable
                };
            }

            var recordStatus = await this.client.GetRecordStatus(ipc);

            if (recordStatus == null)
            {
                return new Model.SectionStatus
                {
                    StatusCode = Model.StatusCode.Error
                };
            }

            var goodStatndingReasons = new[] { "GS", "PRAC", "TEMPPER" };
            if (recordStatus.StatusCode == "ACTIVE" && goodStatndingReasons.Contains(recordStatus.StatusReasonCode))
            {
                return new Model.SectionStatus
                {
                    StatusCode = Model.StatusCode.Available
                };
            }
            else
            {
                return new Model.SectionStatus
                {
                    StatusCode = Model.StatusCode.NotAvailable,
                    Reason = "There is a problem with your College licence, please try again later."
                };
            }
        }
    }
}

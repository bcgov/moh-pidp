namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public class ProfileStatus
{
    public class Query : IQuery<IDomainResult<Model>>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }
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

        public bool DemographicsComplete => this.Email != null && this.Phone != null;
        public bool CollegeCertificationComplete => this.CollegeCode != null && this.LicenceNumber != null;
        public bool WorkSettingComplete => this.JobTitle != null && this.FacilityName != null;

        public EnrolmentStatus SaEformsStatus { get; set; }
        public string SaEformsStatusReason { get; set; } = string.Empty;

        public enum EnrolmentStatus
        {
            Available = 1,
            Completed,
            NotAvailable
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, IDomainResult<Model>>
    {
        private readonly IMapper mapper;
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public QueryHandler(
            IMapper mapper,
            IPlrClient client,
            PidpDbContext context)
        {
            this.mapper = mapper;
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult<Model>> HandleAsync(Query query)
        {
            var model = await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (model == null)
            {
                return DomainResult.NotFound<Model>();
            }

            // TODO check submission

            if (!model.DemographicsComplete || !model.CollegeCertificationComplete)
            {
                model.SaEformsStatus = Model.EnrolmentStatus.NotAvailable;
                model.SaEformsStatusReason = "Profile not complete";
            }
            else if (model.Ipc == null)
            {
                model.SaEformsStatus = Model.EnrolmentStatus.NotAvailable;
                model.SaEformsStatusReason = "We are unable to verify your College licence at this time, please try again later.";
            }
            else
            {
                var status = await this.client.GetRecordStatus(model.Ipc);
                if (status == null)
                {
                    model.SaEformsStatus = Model.EnrolmentStatus.NotAvailable;
                    model.SaEformsStatusReason = "Error determining College licence status, please try again later.";
                }
                else
                {
                    var goodStatndingReasons = new[] { "GS", "PRAC", "TEMPPER" };
                    if (status.StatusCode == "ACTIVE" && goodStatndingReasons.Contains(status.StatusReasonCode))
                    {
                        model.SaEformsStatus = Model.EnrolmentStatus.Available;
                        model.SaEformsStatusReason = "";
                    }
                    else
                    {
                        model.SaEformsStatus = Model.EnrolmentStatus.NotAvailable;
                        model.SaEformsStatusReason = "There is a problem with your College licence, please try again later.";
                    }
                }
            }

            return DomainResult.Success(model);
        }
    }
}

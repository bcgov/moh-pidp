namespace Pidp.Infrastructure.Services;

using System.Threading.Tasks;
using Pidp.Data;
using Pidp.Models.ProfileStatus;
using Pidp.Infrastructure.HttpClients.Plr;
using System.Collections.Generic;
using Pidp.Infrastructure.Services.ProfileStatusServiceInternal;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Pidp.Models.Lookups;
using NodaTime;

public class ProfileStatusService : IProfileStatusService
{
    private readonly IMapper mapper;
    private readonly IPlrClient client;
    private readonly PidpDbContext context;

    public ProfileStatusService(
        IMapper mapper,
        IPlrClient client,
        PidpDbContext context)
    {
        this.mapper = mapper;
        this.client = client;
        this.context = context;
    }

    public async Task<IEnumerable<IProfileSection>> CompileSectionsAsync(int partyId, bool recheckIpc = false, params Section[] sections)
    {
        var profile = await this.context.Parties
            .Where(party => party.Id == partyId)
            .ProjectTo<ProfileStatusDto>(this.mapper.ConfigurationProvider)
            .SingleAsync();

        if (recheckIpc
            && profile.CollegeCode.HasValue
            && !string.IsNullOrWhiteSpace(profile.LicenceNumber)
            && profile.Ipc == null)
        {
            // Cert has been entered but no IPC found, likely due to a transient error or delay in PLR record updates. Retry once.
            profile.Ipc = await this.RecheckIpc(partyId, profile.CollegeCode.Value, profile.LicenceNumber, profile.Birthdate);
        }

        profile.PlrRecordStatus = await this.client.GetRecordStatus(profile.Ipc);

        return profile.GetSections(sections);
    }

    public async Task<StatusCode> ComputeStatusAsync(int partyId, Section section)
    {
        var result = await this.CompileSectionsAsync(partyId, false, section);
        return result.Single().StatusCode;
    }

    private async Task<string?> RecheckIpc(int partyId, CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
    {
        var newIpc = await this.client.GetPlrRecord(collegeCode, licenceNumber, birthdate);
        if (newIpc != null)
        {
            var cert = await this.context.PartyCertifications
                .SingleAsync(cert => cert.PartyId == partyId);
            cert.Ipc = newIpc;
            await this.context.SaveChangesAsync();
        }

        return newIpc;
    }
}

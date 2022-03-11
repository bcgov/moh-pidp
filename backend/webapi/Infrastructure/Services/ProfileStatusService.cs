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

    public async Task<IEnumerable<IProfileSection>> CompileSectionsAsync(int partyId, params Section[] sections)
    {
        var profile = await this.context.Parties
                   .Where(party => party.Id == partyId)
                   .ProjectTo<ProfileStatusDto>(this.mapper.ConfigurationProvider)
                   .SingleAsync();
        profile.PlrRecordStatus = await this.client.GetRecordStatus(profile.Ipc);

        throw new NotImplementedException();

    }

    public async Task<StatusCode> ComputeStatusAsync(int partyId, Section section)
    {
        var result = await this.CompileSectionsAsync(partyId, section);
        return result.Single().StatusCode;
    }
}

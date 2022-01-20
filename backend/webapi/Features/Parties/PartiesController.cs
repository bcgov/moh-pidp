namespace Pidp.Features.Parties;

using HybridModelBinding;
using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PartiesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                  [FromQuery] Index.Query query)
        => await handler.HandleAsync(query);

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateParty([FromServices] ICommandHandler<Create.Command, int> handler,
                                                     [FromBody] Create.Command command)
        => await handler.HandleAsync(command);

    [HttpGet("{partyId}/college-certification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CollegeCertification.Command>> GetPartyCollegeCertification([FromServices] IQueryHandler<CollegeCertification.Query, CollegeCertification.Command> handler,
                                                                                               [FromRoute] CollegeCertification.Query query)
        => await handler.HandleAsync(query);

    [HttpPut("{partyId}/college-certification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyCollegeCertification([FromServices] ICommandHandler<CollegeCertification.Command> handler,
                                                                    [FromHybrid] CollegeCertification.Command command)
    {
        await handler.HandleAsync(command);
        return this.NoContent();
    }

    [HttpGet("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Demographics.Command>> GetPartyDemographics([FromServices] IQueryHandler<Demographics.Query, Demographics.Command> handler,
                                                                               [FromRoute] Demographics.Query query)
        => await handler.HandleAsync(query);

    [HttpPut("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyDemographics([FromServices] ICommandHandler<Demographics.Command> handler,
                                                            [FromHybrid] Demographics.Command command)
    {
        await handler.HandleAsync(command);
        return this.NoContent();
    }

    [HttpGet("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkSetting.Command>> GetPartyWorkSetting([FromServices] IQueryHandler<WorkSetting.Query, WorkSetting.Command> handler,
                                                                             [FromRoute] WorkSetting.Query query)
        => await handler.HandleAsync(query);

    [HttpPut("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyWorkSetting([FromServices] ICommandHandler<WorkSetting.Command> handler,
                                                           [FromHybrid] WorkSetting.Command command)
    {
        await handler.HandleAsync(command);
        return this.NoContent();
    }

    [HttpGet("{id}/profile-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfileStatus.Model>> GetPartyProfileStatus([FromServices] IQueryHandler<ProfileStatus.Query, ProfileStatus.Model> handler,
                                                                               [FromHybrid] ProfileStatus.Query query)
        => await handler.HandleAsync(query);

    [HttpGet("{id}/enrolment-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<EnrolmentStatus.Model>> GetPartyEnrolmentStatus([FromServices] IQueryHandler<EnrolmentStatus.Query, EnrolmentStatus.Model> handler,
                                                                                   [FromHybrid] EnrolmentStatus.Query query)
        => await handler.HandleAsync(query);
}

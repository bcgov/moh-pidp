namespace Pidp.Features.Parties;

using Microsoft.AspNetCore.Mvc;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
public class PartiesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Index.Model>>> GetParties([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler)
        => await handler.HandleAsync(new Index.Query());

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateParty([FromServices] ICommandHandler<Create.Command, int> handler,
                                                     [FromBody] Create.Command command)
        => await handler.HandleAsync(command);

    [HttpGet("{partyId}/demographics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Demographics.Command>> GetPartyDemographics([FromServices] IQueryHandler<Demographics.Query, Demographics.Command> handler,
                                                                               [FromRoute] int partyId)
        => await handler.HandleAsync(new Demographics.Query { Id = partyId });

    [HttpPut("{partyId}/demographics")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyDemographics([FromServices] ICommandHandler<Demographics.Command> handler,
                                                            [FromBody] Demographics.Command command)
    {
        // TODO use ID
        await handler.HandleAsync(command);
        return this.NoContent();
    }

    [HttpGet("{partyId}/college-certification")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<CollegeCertification.Command>> GetPartyCollegeCertification([FromServices] IQueryHandler<CollegeCertification.Query, CollegeCertification.Command> handler,
                                                                                               [FromRoute] int partyId)
        => await handler.HandleAsync(new CollegeCertification.Query { PartyId = partyId });

    [HttpPut("{partyId}/college-certification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyCollegeCertification([FromServices] ICommandHandler<CollegeCertification.Command> handler,
                                                                    [FromBody] CollegeCertification.Command command)
    {
        await handler.HandleAsync(command);
        return this.NoContent();
    }

    [HttpGet("{partyId}/work-setting")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkSetting.Command>> GetPartyWorkSetting([FromServices] IQueryHandler<WorkSetting.Query, WorkSetting.Command> handler,
                                                                             [FromRoute] int partyId)
        => await handler.HandleAsync(new WorkSetting.Query { Id = partyId });

    [HttpPut("{partyId}/work-setting")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> UpdatePartyWorkSetting([FromServices] ICommandHandler<WorkSetting.Command> handler,
                                                           [FromBody] WorkSetting.Command command)
    {
        await handler.HandleAsync(command);
        return this.NoContent();
    }
}

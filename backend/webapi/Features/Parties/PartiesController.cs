namespace Pidp.Features.Parties;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AllowedPartyIdentityProvider)]
public class PartiesController : PidpControllerBase
{
    public PartiesController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CollegeCertification.Command>> GetPartyCollegeCertification([FromServices] IQueryHandler<CollegeCertification.Query, IDomainResult<CollegeCertification.Command>> handler,
                                                                                               [FromRoute] CollegeCertification.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPut("{partyId}/college-certification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyCollegeCertification([FromServices] ICommandHandler<CollegeCertification.Command, IDomainResult> handler,
                                                                     [FromHybrid] CollegeCertification.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpGet("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Demographics.Command>> GetPartyDemographics([FromServices] IQueryHandler<Demographics.Query, IDomainResult<Demographics.Command>> handler,
                                                                               [FromRoute] Demographics.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query)
            .ToActionResultOfT();

    [HttpPut("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyDemographics([FromServices] ICommandHandler<Demographics.Command, IDomainResult> handler,
                                                             [FromHybrid] Demographics.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command)
            .ToActionResult();

    [HttpPost("{id}/profile-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileStatus.Model>> ComputePartyProfileStatus([FromServices] ICommandHandler<ProfileStatus.Command, IDomainResult<ProfileStatus.Model>> handler,
                                                                                   [FromRoute] ProfileStatus.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command)
            .ToActionResultOfT();

    [HttpGet("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkSetting.Command>> GetPartyWorkSetting([FromServices] IQueryHandler<WorkSetting.Query, IDomainResult<WorkSetting.Command>> handler,
                                                                             [FromRoute] WorkSetting.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query)
            .ToActionResultOfT();

    [HttpPut("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyWorkSetting([FromServices] ICommandHandler<WorkSetting.Command, IDomainResult> handler,
                                                            [FromHybrid] WorkSetting.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command)
            .ToActionResult();
}

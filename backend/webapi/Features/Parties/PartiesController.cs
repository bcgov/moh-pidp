namespace Pidp.Features.Parties;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class PartiesController : PidpControllerBase
{
    public PartiesController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateParty([FromServices] ICommandHandler<Create.Command, int> handler,
                                                     [FromBody] Create.Command command)
        => await handler.HandleAsync(command);

    [HttpGet("{partyId}/college-certifications")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<CollegeCertifications.Model>>> GetCollegeCertifications([FromServices] IQueryHandler<CollegeCertifications.Query, IDomainResult<List<CollegeCertifications.Model>>> handler,
                                                                                                [FromRoute] CollegeCertifications.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpGet("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Demographics.Command>> GetPartyDemographics([FromServices] IQueryHandler<Demographics.Query, Demographics.Command> handler,
                                                                               [FromRoute] Demographics.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query)
            .ToActionResultOfT();

    [HttpPut("{id}/demographics")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyDemographics([FromServices] ICommandHandler<Demographics.Command> handler,
                                                             [FromHybrid] Demographics.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command)
            .ToActionResult();

    [HttpGet("{partyId}/licence-declaration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LicenceDeclaration.Command>> GetPartyLicenceDeclaration([FromServices] IQueryHandler<LicenceDeclaration.Query, LicenceDeclaration.Command> handler,
                                                                                           [FromRoute] LicenceDeclaration.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPut("{partyId}/licence-declaration")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string?>> UpdatePartyLicenceDeclaration([FromServices] ICommandHandler<LicenceDeclaration.Command, IDomainResult<string?>> handler,
                                                                           [FromHybrid] LicenceDeclaration.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResultOfT();

    [HttpGet("{id}/profile-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileStatus.Model>> ComputePartyProfileStatus([FromServices] IQueryHandler<ProfileStatus.Query, ProfileStatus.Model> handler,
                                                                                   [FromRoute] ProfileStatus.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query.WithUser(this.User))
            .ToActionResultOfT();

    [HttpGet("{id}/profile-status2")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileStatus2.Model>> ComputePartyProfileStatus2([FromServices] IQueryHandler<ProfileStatus2.Query, ProfileStatus2.Model> handler,
                                                                                   [FromRoute] ProfileStatus2.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query.WithUser(this.User))
            .ToActionResultOfT();
}

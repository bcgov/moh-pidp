namespace Pidp.Features.Parties;

using DomainResults.Common;
using DomainResults.Mvc;
using HybridModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
[Authorize(Policy = Policies.AnyPartyIdentityProvider)]
public class PartiesController(IPidpAuthorizationService authorizationService) : PidpControllerBase(authorizationService)
{
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
                                                             [FromHybrid][AutoValidateAlways] Demographics.Command command)
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
                                                                           [FromHybrid][AutoValidateAlways] LicenceDeclaration.Command command)
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
}

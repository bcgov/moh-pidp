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

    [HttpGet("{partyId}/access-administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccessAdministrator.Command>> GetAccessAdministrator([FromServices] IQueryHandler<AccessAdministrator.Query, AccessAdministrator.Command> handler,
                                                                                        [FromRoute] AccessAdministrator.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPut("{partyId}/access-administrator")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAccessAdministrator([FromServices] ICommandHandler<AccessAdministrator.Command> handler,
                                                               [FromHybrid] AccessAdministrator.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyLicenceDeclaration([FromServices] ICommandHandler<LicenceDeclaration.Command, IDomainResult> handler,
                                                                   [FromHybrid] LicenceDeclaration.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpGet("{partyId}/organization-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrganizationDetails.Command>> GetOrganizationDetails([FromServices] IQueryHandler<OrganizationDetails.Query, OrganizationDetails.Command> handler,
                                                                                        [FromRoute] OrganizationDetails.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPut("{partyId}/organization-details")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrganizationDetails([FromServices] ICommandHandler<OrganizationDetails.Command> handler,
                                                               [FromHybrid] OrganizationDetails.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("{id}/profile-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProfileStatus.Model>> ComputePartyProfileStatus([FromServices] ICommandHandler<ProfileStatus.Command, ProfileStatus.Model> handler,
                                                                                   [FromRoute] ProfileStatus.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command.WithUser(this.User))
            .ToActionResultOfT();

    [HttpGet("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WorkSetting.Command>> GetPartyWorkSetting([FromServices] IQueryHandler<WorkSetting.Query, WorkSetting.Command> handler,
                                                                             [FromRoute] WorkSetting.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.Id, handler, query)
            .ToActionResultOfT();

    [HttpPut("{id}/work-setting")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePartyWorkSetting([FromServices] ICommandHandler<WorkSetting.Command> handler,
                                                            [FromHybrid] WorkSetting.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.Id, handler, command)
            .ToActionResult();
}

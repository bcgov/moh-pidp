namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using DomainResults.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using static Pidp.Infrastructure.HttpClients.Ldap.HcimAuthorizationStatus;
using HybridModelBinding;

[Route("api/parties/{partyId}/[controller]")]
public class AccessRequestsController : PidpControllerBase
{
    public AccessRequestsController(IPidpAuthorizationService authorizationService) : base(authorizationService) { }

    [HttpGet]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<Index.Model>>> GetAccessRequests([FromServices] IQueryHandler<Index.Query, List<Index.Model>> handler,
                                                                         [FromRoute] Index.Query query)
        => await this.AuthorizePartyBeforeHandleAsync(query.PartyId, handler, query)
            .ToActionResultOfT();

    [HttpPost("driver-fitness")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDriverFitnessEnrolment([FromServices] ICommandHandler<DriverFitness.Command, IDomainResult> handler,
                                                                  [FromRoute] DriverFitness.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("hcim-account-transfer")]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status423Locked)]
    public async Task<IActionResult> CreateHcimAccountTransfer([FromServices] ICommandHandler<HcimAccountTransfer.Command, IDomainResult<HcimAccountTransfer.Model>> handler,
                                                               [FromHybrid] HcimAccountTransfer.Command command)
    {
        var access = await this.AuthorizationService.CheckPartyAccessibility(command.PartyId, this.User);
        if (!access.IsSuccess)
        {
            return access.ToActionResult();
        }

        var result = await handler.HandleAsync(command);
        if (!result.IsSuccess)
        {
            return result.ToActionResult();
        }

        this.Response.SafeAddHeader("No-Retry", "true");

        switch (result.Value.AuthStatus)
        {
            case AuthorizationStatus.Authorized:
                return this.NoContent();
            case AuthorizationStatus.AccountLocked:
                return this.StatusCode(StatusCodes.Status423Locked);
            case AuthorizationStatus.AuthFailure:
                this.Response.SafeAddHeader("Remaining-Attempts", result.Value.RemainingAttempts?.ToString(CultureInfo.InvariantCulture));
                return this.UnprocessableEntity();
            case AuthorizationStatus.Unauthorized:
                return this.Forbid();
            default:
                throw new NotImplementedException();
        }
    }

    [HttpPost("hcim-enrolment")]
    [Authorize(Policy = Policies.AnyPartyIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHcimEnrolment([FromServices] ICommandHandler<HcimEnrolment.Command, IDomainResult> handler,
                                                         [FromHybrid] HcimEnrolment.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("ms-teams-clinic-member")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMSTeamsClinicMemberEnrolment([FromServices] ICommandHandler<MSTeamsClinicMember.Command, IDomainResult> handler,
                                                                        [FromHybrid] MSTeamsClinicMember.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("ms-teams-privacy-officer")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateMSTeamsPrivacyOfficerEnrolment([FromServices] ICommandHandler<MSTeamsPrivacyOfficer.Command, IDomainResult> handler,
                                                                          [FromHybrid] MSTeamsPrivacyOfficer.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("prescription-refill-eforms")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePrescriptionRefillEformsEnrolment([FromServices] ICommandHandler<PrescriptionRefillEforms.Command, IDomainResult> handler,
                                                                             [FromRoute] PrescriptionRefillEforms.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("provider-reporting-portal")]
    [Authorize(Policy = Policies.BCProviderAuthentication)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProviderReportingPortalEnrolment([FromServices] ICommandHandler<ProviderReportingPortal.Command, IDomainResult> handler,
                                                                            [FromRoute] ProviderReportingPortal.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("sa-eforms")]
    [Authorize(Policy = Policies.HighAssuranceIdentityProvider)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateSAEformsEnrolment([FromServices] ICommandHandler<SAEforms.Command, IDomainResult> handler,
                                                             [FromRoute] SAEforms.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();

    [HttpPost("user-access-agreement")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptUserAccessAgreement([FromServices] ICommandHandler<UserAccessAgreement.Command, IDomainResult> handler,
                                                               [FromRoute] UserAccessAgreement.Command command)
        => await this.AuthorizePartyBeforeHandleAsync(command.PartyId, handler, command)
            .ToActionResult();
}

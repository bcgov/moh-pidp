namespace Pidp.Features.Pharmacies;

using MediatR;
using Pidp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pidp.Data;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

[Route("api/[controller]")]
public class PharmaciesController(IMediator mediator, PidpDbContext context) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    private readonly PidpDbContext context = context;

    [HttpGet("profile")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(typeof(Profile.Model), StatusCodes.Status200OK)]
    public async Task<ActionResult<Profile.Model>> GetPharmacyAdminProfile()
    {
        var partyId = this.User.GetPartyId(this.context);
        Console.WriteLine($"DEBUG: PharmaciesController.GetPharmacyAdminProfile - Retrieved PartyId: {partyId}");
        return await this.mediator.Send(new Profile.Query { PartyId = partyId });
    }

    [HttpPost]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreatePharmacy([FromBody] Create.Command command)
    {
        command.PartyId = this.User.GetPartyId(this.context);
        var newPharmacyId = await this.mediator.Send(command);
        return this.CreatedAtAction(null, new { id = newPharmacyId }, newPharmacyId);
    }

    [HttpGet("{pharmacyId}")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(typeof(Details.Model), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Details.Model?>> GetPharmacyDetails([FromRoute] int pharmacyId)
        => await this.mediator.Send(new Details.Query { PharmacyId = pharmacyId, PartyId = this.User.GetPartyId(this.context) });

    [HttpPut("{pharmacyId}")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePharmacy([FromRoute] int pharmacyId, [FromBody] Update.Command command)
    {
        command.PharmacyId = pharmacyId;
        command.RequestingPartyId = this.User.GetPartyId(this.context);
        await this.mediator.Send(command);
        return this.NoContent();
    }

    [HttpGet("{pharmacyId}/enrolment-token")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    // [Authorize(Policy = Policies.PharmacyAdmin)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<string>> GenerateEnrolmentToken([FromRoute] int pharmacyId, [FromQuery] PharmacyRole role)
    {
        var command = new GenerateEnrolmentToken.Command
        {
            PharmacyId = pharmacyId,
            RoleToAssign = role,
            RequestingPartyId = this.User.GetPartyId(this.context)
        };
        var token = await this.mediator.Send(command);
        return this.Ok(token);
    }

    [HttpGet("{pharmacyId}/staff")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    // [Authorize(Policy = Policies.PharmacyAdmin)]
    [ProducesResponseType(typeof(List<Staff.Model>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Staff.Model>>> GetStaff([FromRoute] int pharmacyId)
    {
        return await this.mediator.Send(new Staff.Query { PharmacyId = pharmacyId, PartyId = this.User.GetPartyId(this.context) });
    }

    [HttpPut("{pharmacyId}/staff/{partyId}")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    // [Authorize(Policy = Policies.PharmacyAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStaff([FromRoute] int pharmacyId, [FromRoute] int partyId, [FromBody] UpdateStaff.Command command)
    {
        command.PharmacyId = pharmacyId;
        command.PartyId = partyId;
        command.RequestingPartyId = this.User.GetPartyId(this.context);
        await this.mediator.Send(command);
        return this.NoContent();
    }

    [HttpDelete("{pharmacyId}/staff/{partyId}")]
    [Authorize(Policy = Policies.BcscAuthentication)]
    // [Authorize(Policy = Policies.PharmacyAdmin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStaff([FromRoute] int pharmacyId, [FromRoute] int partyId)
    {
        await this.mediator.Send(new DeleteStaff.Command { PharmacyId = pharmacyId, PartyId = partyId, RequestingPartyId = this.User.GetPartyId(this.context) });
        return this.NoContent();
    }
}
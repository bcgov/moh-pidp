namespace Pidp.Features.Documents;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController(PidpDbContext context) : ControllerBase
{
    private readonly PidpDbContext context = context;

    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDocument([FromRoute] Guid id)
    {
        var document = await this.context.Documents.SingleOrDefaultAsync(d => d.Id == id);

        if (document == null)
        {
            return this.NotFound();
        }

        return this.File(document.Data, document.ContentType, document.FileName);
    }
}

namespace Pidp.Features.Feedback;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pidp.Infrastructure.Services;

[Route("api/[controller]")]
public class FeedbackController(IPidpAuthorizationService authorizationService, ICommandHandler<UploadFileCommand, string> uploadFileHandler) : PidpControllerBase(authorizationService)
{
    private readonly ICommandHandler<UploadFileCommand, string> uploadFileHandler = uploadFileHandler;

    // [HttpPost]
    // [AllowAnonymous]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<Create.Model>> Feedback([FromServices] ICommandHandler<Create.Command, Create.Model> handler,
    //                                                                     [FromBody] Create.Command command)
    //     => await handler.HandleAsync(command);

    [HttpPost("upload")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand model)
    {
        try
        {
            var command = new UploadFileCommand(model.Feedback, model.File);
            var filePath = await this.uploadFileHandler.HandleAsync(command);
            return this.Ok(new { Message = "File uploaded successfully.", FilePath = filePath });
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex);
            return this.StatusCode(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return this.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}

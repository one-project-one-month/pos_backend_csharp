using System.Data;

namespace Pos.BackendApi.Features.SaleDraft;

[ApiController]
[Route("api/v1/sale-drafts")]
public sealed class SaleDraftController : ControllerBase
{
    private readonly SaleDraftService _service;

    public SaleDraftController(SaleDraftService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<List<SaleDraftSummaryModel>>> List()
        => Ok(await _service.ListAsync(GetStaffId()));

    [HttpPost]
    public async Task<ActionResult<SaleDraftModel>> Create(CreateSaleDraftRequestModel request)
    {
        var draft = await _service.CreateAsync(GetStaffId(), request);
        return CreatedAtAction(nameof(Get), new { draftId = draft.SaleDraftId }, draft);
    }

    [HttpGet("{draftId:int}")]
    public async Task<ActionResult<SaleDraftModel>> Get(int draftId)
    {
        var draft = await _service.GetAsync(draftId, GetStaffId());
        return draft is null ? NotFound() : Ok(draft);
    }

    [HttpPost("{draftId:int}/items")]
    public Task<ActionResult<SaleDraftModel>> AddItem(int draftId, AddSaleDraftItemRequestModel request)
        => ExecuteDraftAction(() => _service.AddItemAsync(draftId, GetStaffId(), request));

    [HttpPatch("{draftId:int}/items/{productCode}")]
    public Task<ActionResult<SaleDraftModel>> SetQuantity(
        int draftId,
        string productCode,
        SetSaleDraftItemQuantityRequestModel request)
        => ExecuteDraftAction(() => _service.SetQuantityAsync(draftId, GetStaffId(), productCode, request));

    [HttpDelete("{draftId:int}/items/{productCode}")]
    public Task<ActionResult<SaleDraftModel>> RemoveItem(
        int draftId,
        string productCode,
        [FromQuery] string? rowVersion)
        => ExecuteDraftAction(() => _service.RemoveItemAsync(draftId, GetStaffId(), productCode, rowVersion));

    [HttpDelete("{draftId:int}")]
    public async Task<IActionResult> Delete(int draftId, [FromQuery] string? rowVersion)
    {
        try
        {
            await _service.DeleteAsync(draftId, GetStaffId(), rowVersion);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFoundProblem(ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            return ConflictProblem(ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ConflictProblem("This draft changed in another request. Reload and try again.");
        }
    }

    [HttpPost("{draftId:int}/checkout")]
    public async Task<ActionResult<SaleDraftCheckoutResponseModel>> Checkout(
        int draftId,
        CheckoutSaleDraftRequestModel request)
    {
        try
        {
            return Ok(await _service.CheckoutAsync(draftId, GetStaffId(), request));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFoundProblem(ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            return ConflictProblem(ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ConflictProblem("This draft changed in another request. Reload and try again.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequestProblem(ex.Message);
        }
    }

    private async Task<ActionResult<SaleDraftModel>> ExecuteDraftAction(Func<Task<SaleDraftModel>> action)
    {
        try
        {
            return Ok(await action());
        }
        catch (KeyNotFoundException ex)
        {
            return NotFoundProblem(ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            return ConflictProblem(ex.Message);
        }
        catch (DbUpdateConcurrencyException)
        {
            return ConflictProblem("This draft changed in another request. Reload and try again.");
        }
        catch (OverflowException)
        {
            return BadRequestProblem("Quantity is too large.");
        }
    }

    private int GetStaffId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("StaffId");
        return int.TryParse(value, out var staffId)
            ? staffId
            : throw new UnauthorizedAccessException("Staff identifier is missing.");
    }

    private ObjectResult NotFoundProblem(string detail)
        => Problem(detail: detail, statusCode: StatusCodes.Status404NotFound);

    private ObjectResult ConflictProblem(string detail)
        => Problem(detail: detail, statusCode: StatusCodes.Status409Conflict);

    private ObjectResult BadRequestProblem(string detail)
        => Problem(detail: detail, statusCode: StatusCodes.Status400BadRequest);
}

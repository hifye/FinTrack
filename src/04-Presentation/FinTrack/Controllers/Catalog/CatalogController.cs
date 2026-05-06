using Application.Features.Catalog.Commands.Category.CreateCategory;
using Application.Features.Catalog.Commands.Category.DeleteCategory;
using Application.Features.Catalog.Commands.Category.PatchCategory;
using Application.Features.Catalog.ListItem;
using Application.Features.Catalog.Queries.Category.GetCategoriesByUserId;
using Application.Features.Catalog.Queries.Category.GetCategoryDetails;
using FinTrack.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers.Catalog;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CatalogController(IMediator mediator) : ControllerBase
{
    [ProducesResponseType(typeof(CategoryListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("category/{id:guid}")]
    public async Task<ActionResult<CategoryListItem>> GetCategoryDetails(Guid id)
    {
        var result = await mediator.Send(new GetCategoryDetailsQuery(id));
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(CategoryListItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<CategoryListItem>>> GetCategoriesByUserId()
    {
        var result = await mediator.Send(new GetCategoriesByUserIdQuery());
        return result.ToActionResult();
    }

    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("category")]
    public async Task<ActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        if (result.IsFailure)
            return result.ToActionResult();

        return CreatedAtAction(
            nameof(GetCategoryDetails),
            new { id = result.Value },
            result.Value
        );
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPatch("category/{id:guid}")]
    public async Task<ActionResult> PatchCategory(Guid id, PatchCategoryCommand command)
    {
        command = command with { Id = id };
        var result = await mediator.Send(command);
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("category/{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id));
        return result.ToActionResult(StatusCodes.Status204NoContent);
    }
}
using System.Text.Json.Serialization;
using Domain.Common;
using MediatR;

namespace Application.Features.Finance.Command.Account.PatchAccount;

public record PatchAccountCommand([property: JsonIgnore] Guid Id, string? Type, bool? IsActive) : IRequest<Result>;
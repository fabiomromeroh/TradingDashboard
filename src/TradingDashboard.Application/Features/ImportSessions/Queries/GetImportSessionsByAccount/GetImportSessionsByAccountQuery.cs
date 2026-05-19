using MediatR;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionsByAccount;

public record GetImportSessionsByAccountQuery(Guid AccountId) : IRequest<IEnumerable<ImportSessionDto>>;

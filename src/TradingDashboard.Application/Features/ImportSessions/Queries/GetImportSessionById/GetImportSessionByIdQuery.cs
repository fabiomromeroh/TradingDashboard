using MediatR;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Features.ImportSessions.Dtos;

namespace TradingDashboard.Application.Features.ImportSessions.Queries.GetImportSessionById;

public record GetImportSessionByIdQuery(Guid Id) : IRequest<Result<ImportSessionDto>>;

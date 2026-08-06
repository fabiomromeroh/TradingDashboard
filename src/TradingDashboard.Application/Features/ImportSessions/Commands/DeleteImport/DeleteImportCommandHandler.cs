using MediatR;
using TradingDashboard.Application.Abstractions.Repositories;
using TradingDashboard.Application.Abstractions.Services.Import;
using TradingDashboard.Application.Common.Models;
using TradingDashboard.Application.Interfaces;

namespace TradingDashboard.Application.Features.ImportSessions.Commands.DeleteImport
{
    public class DeleteImportCommandHandler : IRequestHandler<DeleteImportCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImportSessionRepository importSessionRepository;
        private readonly IExecutionRepository executionRepository;
        private readonly IImportService importService;

        public DeleteImportCommandHandler(IUnitOfWork unitOfWork, IImportSessionRepository importSessionRepository, IExecutionRepository executionRepository, IImportService importService)
        {
            this.unitOfWork = unitOfWork;
            this.importSessionRepository = importSessionRepository;
            this.executionRepository = executionRepository;
            this.importService = importService;
        }

        public async Task<Result> Handle(DeleteImportCommand command, CancellationToken ct)
        {
            //get importSession
            var importSession = await importSessionRepository.GetByIdAsync(command.importSessionId, ct);
            if (importSession is null) return Result.NotFound(nameof(importSession), command.importSessionId);

            //select all symbols to rebuild for this account
            var symbols = importSession.Executions.Select(x => x.Symbol).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

            //delete all executions
            await executionRepository.DeleteRangeAsync(importSession.Executions, ct);

            await unitOfWork.SaveChangesAsync(ct);

            //rebuild trades for symbols/account
            await importService.RebuildTradesAsync(importSession.AccountId, symbols, ct);

            //finally mark importSession to RolledBack status
            importSession.MarkAsRolledBack();

            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}

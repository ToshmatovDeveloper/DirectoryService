using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Infrastructure.Database;

public class TransactionScope : ITansactionScope
{
    private readonly IDbTransaction _dbTransaction;
    private readonly ILogger<TransactionScope> _logger;

    public TransactionScope(IDbTransaction dbTransaction, ILogger<TransactionScope> logger)
    {
        _dbTransaction = dbTransaction;
        _logger = logger;
    }

    public UnitResult<Error> Commit()
    {
        try
        {
            _dbTransaction.Commit();
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to begin transaction");
            return Error.Failure(new ErrorMessage(
                "transaction.commit.failed", 
                "Failed to commit transaction",
                "transactionCommit"));
        }
    }

    public void Dispose()
    {
        _dbTransaction.Dispose();
    }
    
    public UnitResult<Error> Rollback()
    {
        try
        {
            _dbTransaction.Rollback();
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to rollback transaction");
            return Error.Failure(new ErrorMessage(
                "transaction.rollback.failed", 
                "Failed to rollback transaction",
                "transactionRollback"));
        }
    }
}
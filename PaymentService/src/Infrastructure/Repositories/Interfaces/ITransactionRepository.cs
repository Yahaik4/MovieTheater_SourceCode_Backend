using PaymentService.Infrastructure.EF.Models;

namespace PaymentService.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionById (Guid transactionId);
        Task<Transaction?> GetTransactionTxnRef(string txnRef);
        Task<Transaction> CreateTransaction(Transaction transaction);
        Task<Transaction> UpdateTransaction(Transaction transaction);
    }
}

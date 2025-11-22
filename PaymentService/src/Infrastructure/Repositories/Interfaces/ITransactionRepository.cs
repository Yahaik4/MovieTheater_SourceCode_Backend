using PaymentService.Infrastructure.EF.Models;

namespace PaymentService.Infrastructure.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateTransaction(Transaction transaction);
    }
}

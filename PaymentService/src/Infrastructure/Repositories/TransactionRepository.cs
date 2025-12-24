using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Infrastructure.EF.Models;
using PaymentService.Infrastructure.Repositories.Interfaces;

namespace PaymentService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentDbContext _context;

        public TransactionRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> GetTransactionById(Guid transactionId)
        {
           return await  _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);;
        }

        public async Task<Transaction?> GetTransactionTxnRef(string txnRef)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.TxnRef == txnRef);
        }

        public async Task<Transaction> UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}

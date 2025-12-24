using PaymentService.Infrastructure.Repositories.Interfaces;
using Shared.Contracts.Exceptions;

namespace PaymentService.DomainLogic
{
    public class GetTransactionStatusLogic
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionStatusLogic(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<(string TxnRef, string Status, DateTime UpdatedAt)> Execute(string txnRef)
        {
            if (string.IsNullOrWhiteSpace(txnRef))
                throw new ValidationException("TxnRef is required");

            var tx = await _transactionRepository.GetTransactionTxnRef(txnRef);
            if (tx == null)
                throw new NotFoundException($"Transaction with TxnRef {txnRef} not found");

            var updatedAt = tx.UpdatedAt ?? tx.CreatedAt;

            return (tx.TxnRef, tx.Status, updatedAt);
        }
    }
}

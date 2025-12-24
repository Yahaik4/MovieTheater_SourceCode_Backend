using ApiGateway.DataTransferObject.ResultData;

public class TransactionStatusResultDTO : BaseResultDTO
{
    public TransactionStatusData Data { get; set; }
}

public class TransactionStatusData
{
    public string TxnRef { get; set; }
    public string Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}

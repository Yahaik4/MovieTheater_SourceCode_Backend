namespace src.DataTransferObject.ResultData
{
    public class BaseResultDTO
    {
        public bool Result {  get; set; }
        public string Message { get; set; }
        public List<MessageDetail> MessageDetail { get; set; } = new List<MessageDetail>();
    }

    public class MessageDetail
    {
        public string Code { get; set; }
        public string Desc { get; set; }
    }
}

namespace src.DomainLogic
{
    public interface IDomainLogic<IParam, IResultData>
    {
        IResultData Execute(IParam param);
    }
}

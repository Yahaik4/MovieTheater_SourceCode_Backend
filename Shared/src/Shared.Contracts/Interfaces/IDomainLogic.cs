namespace Shared.Contracts.Interfaces
{
    public interface IDomainLogic<IParam, IResultData>
    {
        IResultData Execute(IParam param);
    }
}

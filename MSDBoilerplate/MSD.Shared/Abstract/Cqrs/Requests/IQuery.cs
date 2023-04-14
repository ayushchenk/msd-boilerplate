namespace MSD.Shared.Abstract.Cqrs.Requests
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}

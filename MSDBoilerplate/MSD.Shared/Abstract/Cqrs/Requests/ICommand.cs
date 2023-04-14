namespace MSD.Shared.Abstract.Cqrs.Requests
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
}

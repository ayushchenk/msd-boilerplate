namespace MSD.Shared.Abstract
{
    public interface IMapper<in TSource, out TDestionation>
    {
        TDestionation Map(TSource source);
    }
}

namespace MSD.Shared.Abstract
{
    public interface IValidator<in T>
    {
        void Validate(T model);
    }
}

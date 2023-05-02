namespace MSD.Shared.Model
{
    public class ValidationError
    {
        public string ErrorMessage { get; }

        public ValidationError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}

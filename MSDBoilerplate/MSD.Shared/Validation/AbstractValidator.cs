using MSD.Shared.Abstract;
using MSD.Shared.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace MSD.Shared.Validation
{
    public abstract class AbstractValidator<T> : IValidator<T>
    {
        private readonly List<ValidationError> _errors;

        protected bool HasErrors => _errors.Any();

        protected AbstractValidator()
        {
            _errors = new List<ValidationError>();
        }

        public virtual void Validate(T model)
        {
            if (model == null)
            {
                AddError($"{nameof(model)} is null. Payload not present.");
            }
            else
            {
                SetupValidation(model);
            }

            if (HasErrors)
            {
                throw new ValidationException(_errors);
            }
        }

        protected void AddError(string error)
        {
            _errors.Add(new ValidationError(error));
        }

        protected abstract void SetupValidation(T model);
    }
}

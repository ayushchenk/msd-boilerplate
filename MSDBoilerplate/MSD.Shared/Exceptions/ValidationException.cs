using MSD.Shared.Model;
using MSD.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MSD.Shared.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public List<ValidationError> Errors { get; }

        public string SerializedErrors => Serializer.Serialize(Errors.Select(e => e.ErrorMessage));

        public ValidationException(string baseMessage) : base(baseMessage)
        {
            Errors = new List<ValidationError>();
        }

        public ValidationException(List<ValidationError> validationErrors)
        {
            Errors = validationErrors;
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Errors = new List<ValidationError>();
        }
    }
}

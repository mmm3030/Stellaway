using Stellaway.Common.Helpers;
using Stellaway.Common.Resources;

namespace Stellaway.Common.Exceptions;

public class ConflictException : Exception
{
    public ConflictException() : base(Resource.Conflict) { }

    public ConflictException(string message) : base(message) { }

    public ConflictException(string message, Exception innerException) : base(message, innerException) { }

    public ConflictException(string name, object key) : base(Resource.EntityConflict.Format(name, key)) { }

}
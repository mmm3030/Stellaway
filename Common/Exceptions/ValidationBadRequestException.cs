using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Stellaway.Common.Resources;

namespace Stellaway.Common.Exceptions;

public class ValidationBadRequestException : Exception
{
    public ModelStateDictionary ModelState { get; set; } = default!;

    public ValidationBadRequestException() : base(Resource.BadRequest) { }

    public ValidationBadRequestException(string message) : base(message) { }

    public ValidationBadRequestException(string message, Exception innerException) : base(message, innerException) { }
    public ValidationBadRequestException(ModelStateDictionary modelState) : base(Resource.ValidationBadRequest)
    {
        ModelState = modelState;
    }

    public ValidationBadRequestException(IEnumerable<IdentityError> errors) : base(Resource.ValidationBadRequest)
    {
        ModelState = new ModelStateDictionary();
        errors.ToList().ForEach(error => ModelState.AddModelError(error.Code, error.Description));
    }
}
using FluentValidation.Results;

namespace ShareMyAdventures.Application.Common.Exceptions;

/// <summary>
/// Any input validation
/// 400 Status Code
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="failures"></param>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

    /// <summary>
    /// 
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }
}
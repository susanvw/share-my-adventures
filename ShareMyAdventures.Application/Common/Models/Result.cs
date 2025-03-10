namespace ShareMyAdventures.Application.Common.Models;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> 
{
    /// <summary>
    /// 
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<string>? Errors { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool Succeeded { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="succeeded"></param>
    /// <param name="data"></param>
    /// <param name="errors"></param>
    protected Result(bool succeeded, T? data, IEnumerable<string>? errors = null)
    {
        Data = succeeded ? data : default;
        Errors = errors;
        Succeeded = succeeded;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T> Success(T value) => new(true, value);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Failure(string error) => new(false, default, [error]);
}
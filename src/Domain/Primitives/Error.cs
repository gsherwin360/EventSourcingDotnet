namespace Domain.Primitives;

public sealed class Error
{
	public Error(string code, string message)
	{
		this.Code = code;
		this.Message = message;
	}

	public string Code { get; }
	public string Message { get; }
}

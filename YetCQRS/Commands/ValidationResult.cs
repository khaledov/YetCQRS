namespace YetCQRS.Commands;

public record ValidationResult(IList<string> ErrorMessages, bool IsValid)
{
    internal object SomeWhen<T1, T2>(Func<T1, bool> value1, Func<object, T2> value2)
    {
        throw new NotImplementedException();
    }
}

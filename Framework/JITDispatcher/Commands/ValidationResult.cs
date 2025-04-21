namespace JITDispatcher.Commands;

public class ValidationResult
{
    public ValidationResult():this(new List<string>(), true)  
    {
        
    }
    public ValidationResult(IList<string> ErrorMessages, bool IsValid)
    {
        this.ErrorMessages = ErrorMessages;
        this.IsValid = IsValid;
    }

    public IList<string> ErrorMessages { get; set; }
    public bool IsValid { get; set; }
}

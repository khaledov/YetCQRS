namespace YetCQRS.Commands;

public record ValidationResult(IList<string> ErrorMessages, bool IsValid);

namespace NotificationAPI.Domain.ValueObjects;

public class Email
{
    public string Value { get; private set; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
            throw new ArgumentException("Email inválido");

        Value = value;
    }

    public override bool Equals(object? obj) => obj is Email email && email.Value == Value;
    public override int GetHashCode() => Value.GetHashCode();
}

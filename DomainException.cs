public class DomainException(string message) : Exception(message);

public class InvalidUserException(string message) : DomainException(message);
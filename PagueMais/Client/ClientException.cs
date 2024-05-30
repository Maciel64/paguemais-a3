namespace Exceptions
{
  public class ClientCpfAlreadyRegisteredException : Exception
  {
    public ClientCpfAlreadyRegisteredException() : base("CPF already registered.")
    {
    }

    public ClientCpfAlreadyRegisteredException(string message) : base(message)
    {
    }
  }
  public class ClientEmailAlreadyRegisteredException : Exception
  {
    public ClientEmailAlreadyRegisteredException() : base("Email already registered.")
    {
    }

    public ClientEmailAlreadyRegisteredException(string message) : base(message)
    {
    }
  }
  public class ClientPhoneAlreadyRegisteredException : Exception
  {
    public ClientPhoneAlreadyRegisteredException() : base("Phone already registered.")
    {
    }

    public ClientPhoneAlreadyRegisteredException(string message) : base(message)
    {
    }
  }
  public class ClientInvalidCpfException : Exception
  {
    public ClientInvalidCpfException() : base("Invalid CPF.")
    {
    }

    public ClientInvalidCpfException(string message) : base(message)
    {
    }
  }
}
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
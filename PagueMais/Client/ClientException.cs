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
}
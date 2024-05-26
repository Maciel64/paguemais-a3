namespace Exceptions
{
  // Define uma classe para mensagem de ERRO caso CPF igual
  public class ClientCpfAlreadyRegisteredException : Exception
  {
    // Construtor que cria uma mensagem
    public ClientCpfAlreadyRegisteredException() : base("CPF already registered.")
    {
    }

    // Construtor que aceita uma mensagem e a passa para o construtor base
    public ClientCpfAlreadyRegisteredException(string message) : base(message)
    {
    }
  }
}
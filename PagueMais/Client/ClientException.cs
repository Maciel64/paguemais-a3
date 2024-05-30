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
  public class ClientNotFoundException : Exception
  {
    public ClientNotFoundException() : base("Client not found.")
    {
    }

    public ClientNotFoundException(string message) : base(message)
    {
    }
  }
  public class ClientCpfAlreadyExistsException : Exception
  {
    public ClientCpfAlreadyExistsException() : base("The new CPF is already in use by another client.")
    {
    }

    public ClientCpfAlreadyExistsException(string message) : base(message)
    {
    }
  }

  public class ClientEmailAlreadyExistsException : Exception
  {
    public ClientEmailAlreadyExistsException() : base("The new email is already in use by another client.")
    {
    }

    public ClientEmailAlreadyExistsException(string message) : base(message)
    {
    }

  }
  public class ClientPhoneAlreadyExistsException : Exception
  {
    public ClientPhoneAlreadyExistsException() : base("The new phone is already in use by another client.")
    {
    }

    public ClientPhoneAlreadyExistsException(string message) : base(message)
    {
    }

  }
  public class ClientAgeExceededException : Exception
  {
    public ClientAgeExceededException() : base("Client age exceeds the maximum allowed limit of 120 years.")
    {
    }

    public ClientAgeExceededException(string message) : base(message)
    {
    }
  }
  public class ClientDateNotAceptedException : Exception
  {
    public ClientDateNotAceptedException() : base("Client date is incorect.")
    {
    }

    public ClientDateNotAceptedException(string message) : base(message)
    {
    }
  }
}
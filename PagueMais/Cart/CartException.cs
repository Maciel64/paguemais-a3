namespace Exceptions
{
  public class CartNotFoundException : Exception
  {

    //Mensagem de ERRO caso o Cart não seja encontrado
    public CartNotFoundException() : base("The requested Cart was not found")
    {

    }
  }

  //Mensagem de ERRO casoa quantidade de Produtos do Cart ja seja 0
  public class CartQuantityIsZeroException : Exception
  {
    public CartQuantityIsZeroException() : base("The requested Cart was not found")
    {

    }
  }
}
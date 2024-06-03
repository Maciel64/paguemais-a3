namespace Exceptions
{
  public class CartNotFoundException : Exception
  {
    public CartNotFoundException() : base("The requested Cart was not found")
    {

    }
  }

  public class CartQuantityIsZeroException : Exception
  {
    public CartQuantityIsZeroException() : base("The requested Cart was not found")
    {

    }
  }
}
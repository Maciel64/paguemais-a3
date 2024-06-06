namespace Exceptions
{
  public class ProductPriceIsInvalidException : Exception
  {
    public ProductPriceIsInvalidException() : base("The passed price is invalid") { }
  }

  public class ProductNotFoundException : Exception
  {
    public ProductNotFoundException() : base("The requested product was not found") { }
  }
  public class ProductInUseExeption : Exception
  {
    public ProductInUseExeption() : base("The requested product is in use") { }
  }
}
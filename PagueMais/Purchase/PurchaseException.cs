namespace Exceptions
{
  public class PurchaseTotalIsInvalidException : Exception
  {
    public PurchaseTotalIsInvalidException() : base("The passed total is not valid") { }
  }

  public class PurchaseNotFoundException : Exception
  {
    public PurchaseNotFoundException() : base("The requested purchase was not found") { }
  }
}
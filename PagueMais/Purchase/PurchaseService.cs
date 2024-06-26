using Entities;
using Exceptions;
using Repositories;


namespace Services
{
  public class PurchaseService(IPurchaseRepository purchaseRepository, IClientRepository clientRepository, ICartRepository cartRepository)
  {
    private readonly IPurchaseRepository _purchaseRepository = purchaseRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly ICartRepository _cartRepository = cartRepository;


    public IEnumerable<Purchase> GetAll()
    {
      return _purchaseRepository.GetAll();
    }

    //Adicionar Compra//
    public Purchase Create(Purchase purchase)
    {
      _ = _clientRepository.FindById(purchase.ClientId) ?? throw new ClientNotFoundException();

      if (purchase.Total < 0)
      {
        throw new PurchaseTotalIsInvalidException();
      }

      return _purchaseRepository.Create(purchase);
    }

    //Remover Compra//
    public void Remove(Guid purchaseId)
    {
      var purchase = _purchaseRepository.FindById(purchaseId) ?? throw new PurchaseNotFoundException();

      if (purchase.Carts is not null)
      {
        _cartRepository.RemoveAll(purchase.Carts);
      }

      _purchaseRepository.Remove(purchase);
    }

    //Editar compra//
    public void Update(Guid purchaseId, UpdatePurchaseDTO updatedPurchase)
    {
      //Verificar se ID existe
      var existingPurchase = _purchaseRepository.FindById(purchaseId) ?? throw new PurchaseNotFoundException();

      if (updatedPurchase.Total is not null)
      {
        if (updatedPurchase.Total < 0)
        {
          throw new PurchaseTotalIsInvalidException();
        }

        existingPurchase.Total = (float)updatedPurchase.Total;
      }

      if (updatedPurchase.PaymentMethod is not null)
      {
        existingPurchase.PaymentMethod = (EnumMethods)updatedPurchase.PaymentMethod;
      }

      if (updatedPurchase.Status is not null)
      {
        existingPurchase.Status = (EnumStatus)updatedPurchase.Status;
      }

      _purchaseRepository.Update(existingPurchase);
    }

    //Método para achar a compra pelo ID
    public Purchase? GetPurchaseById(Guid purchaseId)
    {
      var purchase = _purchaseRepository.FindById(purchaseId) ?? throw new PurchaseNotFoundException();
      return purchase;
    }
  }
}
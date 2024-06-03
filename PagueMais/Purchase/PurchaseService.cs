using System.Data;
using System.Runtime.InteropServices;
using Entities;
using Exceptions;
using Repositories;


namespace Services
{
  public class PurchaseService(PurchaseRepository purchaseRepository, ClientRepository clientRepository, CartRepository cartRepository)
  {
    private readonly PurchaseRepository _purchaseRepository = purchaseRepository;
    private readonly ClientRepository _clientRepository = clientRepository;
    private readonly CartRepository _cartRepository = cartRepository;

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
      var purchase = _purchaseRepository.FindById(purchaseId) ?? throw new Exception();
      return purchase;
    }
  }
}
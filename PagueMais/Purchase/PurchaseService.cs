using System.Data;
using System.Runtime.InteropServices;
using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class PurchaseService(PurchaseRepository purchaseRepository)
  {
    private readonly PurchaseRepository _PurchaseRepository = purchaseRepository;

    public async Task<IEnumerable<Purchase>> GetAllAsync()
    {
      return await _PurchaseRepository.GetAllAsync();
    }

    //Adicionar Compra//
    public async Task<Purchase> CreateAsync(Purchase purchase)
    {    
        return await _PurchaseRepository.CreateAsync(purchase);
    }

    //Remover Compra//
    public async Task RemoveAsync(Guid purchaseId)
    {
      var purchase = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();
      await _PurchaseRepository.RemoveAsync(purchase);
    }

    //Editar Usuário//
    public async Task UpdateAsync(Guid purchaseId, UpdatePurchaseDTO updatedPurchase)
    {
     //Verificar se ID existe
      var existingClient = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();


      await _PurchaseRepository.UpdateAsync(existingClient);
    }

    //Método para achar o Cliente pelo ID
    public async Task<Purchase?> GetPurchaseByIdAsync(Guid purchaseId)
    {
      var purchase = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();
      return purchase;
    }
}
}
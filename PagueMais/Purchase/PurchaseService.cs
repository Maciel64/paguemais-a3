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
      //if (purchase == null)
       // throw new ArgumentNullException(nameof(purchase));

      //Validação de dados
     // if (string.IsNullOrWhiteSpace(purchase.ClientId) || purchase.Total <= 0)
      //  throw new InvalidPurchaseException("Invalid purchase data.");

      // Verificação de cliente
      //var client = await _PurchaseRepository.FindClientByIdAsync(purchase.ClientId);
      //if (client == null)
      //  throw new ClientNotFoundException();

        return await _PurchaseRepository.CreateAsync(purchase);
    }

    //Remover Compra//
    public async Task RemoveAsync(Guid purchaseId)
    {
      var purchase = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();
      await _PurchaseRepository.RemoveAsync(purchase);
    }

    //Editar compra//
    public async Task UpdateAsync(Guid purchaseId, UpdatePurchaseDTO updatedPurchase)
    {
     //Verificar se ID existe
      var existingClient = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();


      await _PurchaseRepository.UpdateAsync(existingClient);
    }

    //Método para achar a compra pelo ID
    public async Task<Purchase?> GetPurchaseByIdAsync(Guid purchaseId)
    {
      var purchase = await _PurchaseRepository.FindByIdAsync(purchaseId) ?? throw new ClientNotFoundException();
      return purchase;
    }
}
}
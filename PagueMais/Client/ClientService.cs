using System.Data;
using System.Runtime.InteropServices;
using Entities;
using Exceptions;
using Repositories;

namespace Services
{
  public class ClientService(ClientRepository produtoRepository)
  {
    private readonly ClientRepository _clientRepository = produtoRepository;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
      return await _clientRepository.GetAllAsync();
    }

    //Adicionar usuário//
    public async Task<Client> CreateAsync(Client client)
    {
      //Verificar se a data é válida
      ValidateClientAge(client.BirthDate);

      //Teste para ver se o CPF é válido
      if (IsValidCpf(client.Cpf) == false)
      {
        throw new ClientInvalidCpfException();
      }

      //Teste para ver se o CPF se repete
      var registeredClientCpf = _clientRepository.FindByCpf(client.Cpf);

      if (registeredClientCpf is not null)
      {
        throw new ClientCpfAlreadyRegisteredException();
      }

      //Teste para ver se o Email se repete
      var registeredClientEmail = await _clientRepository.FindByEmail(client.Email);

      if (registeredClientEmail is not null)
      {
        throw new ClientEmailAlreadyRegisteredException();
      }

      //Teste para ver se o Phone se repete
      var registeredClientPhone = await _clientRepository.FindByPhone(client.Phone);

      if (registeredClientPhone is not null)
      {
        throw new ClientPhoneAlreadyRegisteredException();
      }

      return await _clientRepository.CreateAsync(client);
    }

    //Remover Usuário//
    public void RemoveAsync(Guid clientId)
    {
      var client = _clientRepository.FindById(clientId) ?? throw new ClientNotFoundException();
      _clientRepository.RemoveAsync(client);
    }

    //Editar Usuário//
    public async Task UpdateAsync(Guid clientId, UpdateClientDTO updatedClient)
    {

      //Verificar se ID existe
      var existingClient = _clientRepository.FindById(clientId) ?? throw new ClientNotFoundException();

      //Atualiza Nome se não for vazio
      if (updatedClient.Name != "")
      {
        existingClient.Name = updatedClient.Name;
      }

      //Atualiza CPF caso o não seja igual a de outro cliente, seja válido e não vazio
      if (updatedClient.Cpf != "")
      {
        var clientWithCpf = await _clientRepository.FindByCpfAsync(updatedClient.Cpf);
        if (clientWithCpf != null && clientWithCpf.Id != clientId)
          throw new ClientCpfAlreadyExistsException();

        if (IsValidCpf(updatedClient.Cpf) == false)
        {
          throw new ClientInvalidCpfException();
        }
        existingClient.Cpf = updatedClient.Cpf;
      }

      //Atualiza Email caso o não seja igual a de outro cliente e não vazio
      if (updatedClient.Email != "")
      {

        var clientWithEmail = await _clientRepository.FindByEmailAsync(updatedClient.Email);
        if (clientWithEmail != null && clientWithEmail.Id != clientId)
          throw new ClientEmailAlreadyExistsException();
        existingClient.Email = updatedClient.Email;
      }

      //Atualiza Phone caso o não seja igual a de outro cliente e não vazio
      if (updatedClient.Phone != 0)
      {
        //Verificar se o novo telefone já está em uso por outro cliente
        var clientWithPhone = await _clientRepository.FindByPhoneAsync(updatedClient.Phone);
        if (clientWithPhone != null && clientWithPhone.Id != clientId)
          throw new ClientPhoneAlreadyExistsException();
        existingClient.Phone = updatedClient.Phone;
      }

      //Atualiza Data de Nascimento caso o não seja inválido e não vazio
      if (updatedClient.BirthDate != new DateTime())
      {
        //Verificar se a data é válida
        ValidateClientAge(updatedClient.BirthDate);
        existingClient.BirthDate = updatedClient.BirthDate;
      }

      await _clientRepository.UpdateAsync(existingClient);
    }

    //Método para achar o Cliente pelo ID
    public Client? GetClientById(Guid clientId)
    {
      var client = _clientRepository.FindById(clientId) ?? throw new ClientNotFoundException();
      return client;
    }

    //Método para Testar Idade
    private static void ValidateClientAge(DateTime BirthDate)
    {
      int age = DateTime.Now.Year - BirthDate.Year;

      if (age < 0)
      {
        throw new ClientDateNotAceptedException();
      }
      if (age > 120)
      {
        throw new ClientAgeExceededException();
      }
    }

    //Método para teste do CPF
    private static bool IsValidCpf(string cpf)
    {

      cpf = new string(cpf.Where(char.IsDigit).ToArray());


      if (cpf.Length != 11)
        return false;


      if (cpf.All(c => c == cpf[0]))
        return false;


      int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
      int sum = 0;

      for (int i = 0; i < 9; i++)
        sum += int.Parse(cpf[i].ToString()) * multiplier1[i];

      int remainder = sum % 11;
      int digit1 = remainder < 2 ? 0 : 11 - remainder;


      int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
      sum = 0;

      for (int i = 0; i < 10; i++)
        sum += int.Parse(cpf[i].ToString()) * multiplier2[i];

      remainder = sum % 11;
      int digit2 = remainder < 2 ? 0 : 11 - remainder;


      return cpf.EndsWith($"{digit1}{digit2}");
    }
  }
}
using Config;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
  public class ClientRepository(Database context)
  {
    private readonly Database _context = context;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
      return await _context.Clients.ToListAsync();
    }

    public async Task<Client> CreateAsync(Client client)
    {
      _context.Clients.Add(client);
      await _context.SaveChangesAsync();

      return client;
    }

    public async Task<Client?> FindByCpf(string Cpf)
    {
      return await _context.Clients.FirstOrDefaultAsync(client => client.Cpf == Cpf);
    }

    public async Task<bool> IsValidCpfAsync(string Cpf)
    {
      return await Task.Run(() => IsValidCpf(Cpf));
    }

    private static bool IsValidCpf(string cpf)
    {
      // Remove non-numeric characters
      cpf = new string(cpf.Where(char.IsDigit).ToArray());

      // Check if the CPF has 11 digits
      if (cpf.Length != 11)
        return false;

      // Check if all digits are the same
      if (cpf.All(c => c == cpf[0]))
        return false;

      // Calculate the first verification digit
      int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
      int sum = 0;

      for (int i = 0; i < 9; i++)
        sum += int.Parse(cpf[i].ToString()) * multiplier1[i];

      int remainder = sum % 11;
      int digit1 = remainder < 2 ? 0 : 11 - remainder;

      // Calculate the second verification digit
      int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
      sum = 0;

      for (int i = 0; i < 10; i++)
        sum += int.Parse(cpf[i].ToString()) * multiplier2[i];

      remainder = sum % 11;
      int digit2 = remainder < 2 ? 0 : 11 - remainder;

      // Check if the verification digits are valid
      return cpf.EndsWith($"{digit1}{digit2}");

    }
  }
}
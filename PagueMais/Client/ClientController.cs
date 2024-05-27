using System.Net.Http.Headers;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
  // Marca a classe como um controlador de API e define a rota base para as ações deste controlador
  [ApiController]
  [Route("/api/v1/clients")]
  // Marca a classe como um controlador de API e define a rota base para as ações deste controlador
  public class ClientController(ClientService clientService) : ControllerBase
  {
    // Campo privado para armazenar a instância
    private readonly ClientService _clientService = clientService;

    // Método para lidar com requisições HTTP GET na rota base e obter todos os clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
      // Chama o método do serviço para obter todos os clientes de forma assíncrona
      var produtos = await _clientService.GetAllAsync();

      return Ok(produtos);
    }

    // Método para lidar com requisições HTTP POST na rota base e criar um novo cliente
    [HttpPost]
    public async Task<ActionResult<IEnumerable<Client>>> CreateClient([FromBody] Client client)
    {
      try
      {
        // Tenta criar um novo cliente chamando o método do serviço
        await _clientService.CreateAsync(client);
        // Retorna uma resposta com o cliente criado
        return Ok(client);
      }
      catch (ClientCpfAlreadyRegisteredException e) // ERROR CPF do cliente já esteja registrado
      {
        // Retorna uma resposta com a mensagem de erro
        return UnprocessableEntity(e.Message);
      }
    }
  }
}

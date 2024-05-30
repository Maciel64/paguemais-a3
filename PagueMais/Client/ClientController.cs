using System.Net.Http.Headers;
using Entities;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers
{
  [ApiController]
  [Route("/api/v1/clients")]
  public class ClientController(ClientService clientService) : ControllerBase
  {
    private readonly ClientService _clientService = clientService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
      var produtos = await _clientService.GetAllAsync();
      return Ok(produtos);
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<Client>>> CreateClient([FromBody] Client client)
    {
      try
      {
        await _clientService.CreateAsync(client);
        return Ok(client);
      }

      catch (ClientCpfAlreadyRegisteredException c)
      {
        return UnprocessableEntity(c.Message);
      }
      catch (ClientEmailAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }
      catch (ClientPhoneAlreadyRegisteredException p)
      {
        return UnprocessableEntity(p.Message);
      }
      catch (ClientInvalidCpfException e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}
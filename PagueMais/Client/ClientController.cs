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

      catch (ClientCpfAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }
      catch (ClientEmailAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }
      catch (ClientPhoneAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }
      catch (ClientInvalidCpfException e)
      {
        return BadRequest(e.Message);
      }
      catch (ClientAgeExceededException e)
      {
        return BadRequest(e.Message);
      }
      catch (ClientDateNotAceptedException e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
      try
      {
        await _clientService.RemoveAsync(id);
        return NoContent();
      }
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateClient(Guid id, [FromBody] Client client)
    {
      try
      {
        await _clientService.UpdateAsync(id, client);
        return NoContent();
      }
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }
      catch (ClientInvalidCpfException e)
      {
        return BadRequest(e.Message);
      }
      catch (ClientCpfAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }
      catch (ClientEmailAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }
      catch (ClientPhoneAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }
      catch (ClientAgeExceededException e)
      {
        return BadRequest(e.Message);
      }
      catch (ClientDateNotAceptedException e)
      {
        return BadRequest(e.Message);
      }
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetClientById(Guid id)
    {
      try
      {
        var client = await _clientService.GetClientByIdAsync(id);
        return Ok(client);
      }
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
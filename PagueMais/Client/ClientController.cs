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

    //Método para printar todos os Clientes
    [HttpGet]
    public ActionResult<IEnumerable<Client>> GetClients()
    {
      var produtos = _clientService.GetAll();
      return Ok(produtos);
    }

    //Método para criar um novo cliente
    [HttpPost]
    public ActionResult<IEnumerable<Client>> CreateClient([FromBody] Client client)
    {
      try
      {
        //Cria novo Cliente
        _clientService.Create(client);
        return Ok(client);
      }

      //Retorno de ERRO caso CPF já exista
      catch (ClientCpfAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }

      //Retorno de ERRO caso Email já exista
      catch (ClientEmailAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }

      //Retorno de ERRO caso Telefone já exista
      catch (ClientPhoneAlreadyRegisteredException e)
      {
        return UnprocessableEntity(e.Message);
      }

      //Retorno de ERRO caso CPF não seja válido
      catch (ClientInvalidCpfException e)
      {
        return BadRequest(e.Message);
      }

      //Retorno de ERRO caso Idade seja maior que o limite
      catch (ClientAgeExceededException e)
      {
        return BadRequest(e.Message);
      }

      //Retorno de ERRO caso Data seja no futuro
      catch (ClientDateNotAceptedException e)
      {
        return BadRequest(e.Message);
      }
    }

    //Método para deletar Cliente
    [HttpDelete("{Id}")]
    public IActionResult DeleteClient(Guid id)
    {
      try
      {
        //Deleta o Cliente
        _clientService.Remove(id);
        return NoContent();
      }

      //Retorno de ERRO caso ID do Cliente não seja encontrado
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }
    }

    //Método para Editar Cliente
    [HttpPut("{Id}")]
    public IActionResult UpdateClient(Guid id, [FromBody] UpdateClientDTO client)
    {
      try
      {
        //Atualiza o Cliente
        _clientService.Update(id, client);

        //Busca Cliente atualizado
        var updatedClient = _clientService.GetClientById(id);

        //Retorna cliente atualizado
        return Ok(updatedClient);
      }

      //Retorno de ERRO caso Cliente não exista
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }

      //Retorno de ERRO caso CPF é inválido
      catch (ClientInvalidCpfException e)
      {
        return BadRequest(e.Message);
      }

      //Retorno de ERRO caso CPF já exista
      catch (ClientCpfAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }

      //Retorno de ERRO caso Email Já exista
      catch (ClientEmailAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }

      //Retorno de ERRO caso Telefone já exista
      catch (ClientPhoneAlreadyExistsException e)
      {
        return Conflict(e.Message);
      }

      //Retorno de ERRO caso Idade seja maior que o limite
      catch (ClientAgeExceededException e)
      {
        return BadRequest(e.Message);
      }

      //Retorno de ERRO caso Data seja no futuro
      catch (ClientDateNotAceptedException e)
      {
        return BadRequest(e.Message);
      }
    }

    //Método para printar Cliente pelo ID
    [HttpGet("{Id}")]
    public IActionResult GetClientById(Guid id)
    {
      try
      {

        //Busca Cliente
        var client = _clientService.GetClientById(id);

        //Retorna Clientes
        return Ok(client);
      }

      //Retorno de ERRO caso Cliente não exista
      catch (ClientNotFoundException e)
      {
        return NotFound(e.Message);
      }
    }
  }
}
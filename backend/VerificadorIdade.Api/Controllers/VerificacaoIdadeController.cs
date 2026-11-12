using Microsoft.AspNetCore.Mvc;
using VerificadorIdade.Api.Models;

namespace VerificadorIdade.Api.Controllers;

[ApiController]
[Route("api")]
public class VerificacaoIdadeController : ControllerBase
{
    [HttpPost("verificacao-idade")]
    public ActionResult<VerificacaoIdadeResponse> VerificarIdade([FromBody] VerificacaoIdadeRequest request)
    {
        var mensagem = request.Idade >= 18
            ? "Você é maior de idade."
            : "Você é menor de idade.";

        return Ok(new VerificacaoIdadeResponse
        {
            Mensagem = mensagem
        });
    }
}

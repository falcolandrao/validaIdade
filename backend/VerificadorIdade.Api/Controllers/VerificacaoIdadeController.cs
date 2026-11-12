using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VerificadorIdade.Api.Models;

namespace VerificadorIdade.Api.Controllers;

[ApiController]
[Route("api")]
[EnableCors("AngularOrigin")]
public class VerificacaoIdadeController : ControllerBase
{
    [HttpPost("verificacao-idade")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<VerificacaoIdadeResponse> VerificarIdade([FromBody] VerificacaoIdadeRequest request)
    {
        if (request is null || request.Idade is null)
        {
            return ValidationProblem("A idade é obrigatória.");
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var mensagem = request.Idade >= 18
            ? "Você é maior de idade."
            : "Você é menor de idade.";

        return Ok(new VerificacaoIdadeResponse
        {
            Mensagem = mensagem
        });
    }
}

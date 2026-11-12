using System.Text.Json.Serialization;

namespace VerificadorIdade.Api.Models;

public class VerificacaoIdadeRequest
{
    [JsonPropertyName("idade")]
    public int Idade { get; set; }
}

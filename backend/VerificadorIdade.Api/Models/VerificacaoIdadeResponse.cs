using System.Text.Json.Serialization;

namespace VerificadorIdade.Api.Models;

public class VerificacaoIdadeResponse
{
    [JsonPropertyName("mensagem")]
    public string Mensagem { get; set; } = string.Empty;
}

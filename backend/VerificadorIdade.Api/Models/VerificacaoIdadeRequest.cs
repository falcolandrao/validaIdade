using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VerificadorIdade.Api.Models;

public class VerificacaoIdadeRequest
{
    [JsonPropertyName("idade")]
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "A idade deve ser um número inteiro não negativo.")]
    public int? Idade { get; set; }
}

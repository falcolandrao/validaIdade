using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace VerificadorIdade.Api.Tests;

public class VerificacaoIdadeEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string Endpoint = "/api/verificacao-idade";
    private readonly HttpClient _client;

    public VerificacaoIdadeEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(17, "Você é menor de idade.")]
    [InlineData(18, "Você é maior de idade.")]
    [InlineData(21, "Você é maior de idade.")]
    public async Task Post_returns_expected_message_and_json_contract(int idade, string mensagem)
    {
        using var response = await _client.PostAsJsonAsync(Endpoint, new { idade });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var properties = json.RootElement.EnumerateObject().Select(property => property.Name);

        Assert.Equal(new[] { "mensagem" }, properties);
        Assert.Equal(mensagem, json.RootElement.GetProperty("mensagem").GetString());
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"idade\":-1}")]
    [InlineData("{\"idade\":17.5}")]
    [InlineData("{\"idade\":\"inválida\"}")]
    public async Task Post_returns_problem_details_for_invalid_input(string requestBody)
    {
        using var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        await AssertBadRequestAsync(content);
    }

    [Fact]
    public async Task Post_returns_bad_request_when_body_is_missing()
    {
        using var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

        await AssertBadRequestAsync(content);
    }

    private async Task AssertBadRequestAsync(HttpContent content)
    {
        using var response = await _client.PostAsync(Endpoint, content);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        Assert.Equal((int)HttpStatusCode.BadRequest, json.RootElement.GetProperty("status").GetInt32());
    }
}

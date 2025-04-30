using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public OrdersController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpPost]
    public async Task<IActionResult> ProxyOrder()
    {
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("http://orderservice/api/orders", content);

        return StatusCode((int)response.StatusCode);
    }
}

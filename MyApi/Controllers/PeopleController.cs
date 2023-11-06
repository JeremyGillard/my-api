using Dapper;
using Microsoft.AspNetCore.Mvc;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Controllers;

[ApiController]
[Route("api")]
public class PeopleController : ControllerBase
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<PeopleController> _logger;

    public PeopleController(IDbConnectionFactory connectionFactory, ILogger<PeopleController> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    [HttpGet("people")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            _logger.LogInformation("Retrieving people from database");
            var connection = await _connectionFactory.CreateConnectionAsync();
            var sql = "SELECT * FROM Person";
            var people = await connection.QueryAsync<Person>(sql);
            var response = new
            {
                items = people
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e.StackTrace);
            return StatusCode(503, new
            {
                status = 503,
                error = "Service Unavailable",
                message = "The server is currently unable to handle the request. Please try again later."
            });
        }
    }
}
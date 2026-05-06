using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    [HttpPost("AssignCommissions/{projectId}")]
    public IActionResult AssignCommissions([FromRoute] int projectId)
    {
        return Ok(new { id = projectId, mensaje = "ID recibido con éxito" });;
    }
}
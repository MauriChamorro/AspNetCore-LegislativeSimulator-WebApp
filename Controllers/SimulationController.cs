using Microsoft.AspNetCore.Mvc;

namespace WebAppMVC.Controllers;

public class SimulationController : ControllerBase
{
    [HttpPost]
    public JsonResult AssignCommissions()
    {
        return new JsonResult("Hi");
    }
}
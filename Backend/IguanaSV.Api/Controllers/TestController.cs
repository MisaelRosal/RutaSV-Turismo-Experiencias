using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly TestService _testService;

    public TestController(TestService testService)
    {
        _testService = testService;
    }

    [HttpGet("probar-conexion")]
    public async Task<IActionResult> GetPruebaConexion()
    {
        var mensaje = await _testService.ProbarConexionAsync();
        return Ok(new
        {
            app = "IguanaSV.Api Backend",
            estado = "En linea",
            conexionBaseDatos = mensaje,
            timeStamp = DateTime.UtcNow
        });
    }
}
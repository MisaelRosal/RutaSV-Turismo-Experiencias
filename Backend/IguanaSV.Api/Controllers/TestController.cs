using Microsft.AspNetCore.Mvc;

[Route("api/[test]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly TestService_testService;

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
            conexionBaseDatos = mensaje
            timeStamp = DateTime.UtcNow
        });
    }
}
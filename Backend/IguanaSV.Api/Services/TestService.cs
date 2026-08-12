using Microsoft.EntityFrameworkCore;

public class TestService{
    private readonly RutaDbContext _context;

    public TestService(RutaDbContext context)
    {
        _context = context;
    }

    public async Task<string> ProbarConexionAsync(){
        bool puedeConectarse = await _context.Database.CanConnectAsync();

        if(puedeConectarse){
            return "Conexión exitosa a la base de datos.";
        } else {
            return "No se pudo conectar a la base de datos.";
        }
    }
}
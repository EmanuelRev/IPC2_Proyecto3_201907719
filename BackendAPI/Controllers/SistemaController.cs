// codigo para el reseteooo

using Microsoft.AspNetCore.Mvc;
using BackendAPI.Datos;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SistemaController : ControllerBase
    {
        [HttpPost("limpiarDatos")]
        public IActionResult LimpiarDatos()
        {
        
            if (Memoria.ListaClientes != null)
            {
                Memoria.ListaClientes.Limpiar();
            }
            
            if (Memoria.ListaBancos != null)
            {
                Memoria.ListaBancos.Limpiar();
            }

            return Ok(new { mensaje = "Sistema reseteado exitosamente. La memoria está limpia." });
        }
    }
}
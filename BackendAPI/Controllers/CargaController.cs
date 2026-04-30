// carga de fstossssss

using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using BackendAPI.Utilidades;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CargaController : ControllerBase
    {
        [HttpPost("configuracion")]
        public async Task<IActionResult> CargarConfiguracion()
        {
        
            using StreamReader reader = new StreamReader(Request.Body);
            string xmlContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(xmlContent))
            {
                return BadRequest("El archivo XML está vacío.");
            }

            LectorXML lector = new LectorXML();
            lector.ProcesarConfiguracion(xmlContent);

            return Ok("Configuración de Bancos y Clientes cargada exitosamente.");
        }

        [HttpPost("transacciones")]
        public async Task<IActionResult> CargarTransacciones()
        {
            using StreamReader reader = new StreamReader(Request.Body);
            string xmlContent = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(xmlContent))
            {
                return BadRequest("El archivo XML está vacío.");
            }

            LectorXML lector = new LectorXML();
            lector.ProcesarTransacciones(xmlContent);

            return Ok("Transacciones encoladas exitosamente.");
        }
    }
}
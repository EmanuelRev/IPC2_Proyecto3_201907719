// controladorrrñ

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

            
            string respuestaXml = lector.GenerarRespuestaConfiguracion();
            return Content(respuestaXml, "application/xml");
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

            MotorPagos motor = new MotorPagos();
            motor.ProcesarColaTransacciones();

            string respuestaXml = lector.GenerarRespuestaTransacciones();
            return Content(respuestaXml, "application/xml");
        }
    }
}
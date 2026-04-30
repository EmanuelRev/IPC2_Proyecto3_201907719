using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FrontendWeb.Pages
{
    public class CargaArchivosModel : PageModel
    {
        public string MensajeRespuesta { get; set; }

        // AQUÍ VA TU PUERTO DEL BACKEND
        private readonly string ApiUrlBase = "http://localhost:5184/api/carga"; 

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostConfiguracionAsync(IFormFile archivoConfig)
        {
            try
            {
                if (archivoConfig == null || archivoConfig.Length == 0)
                {
                    MensajeRespuesta = "Error: No seleccionaste ningún archivo o el archivo está vacío.";
                    return Page();
                }

                using var reader = new StreamReader(archivoConfig.OpenReadStream());
                string xmlContent = await reader.ReadToEndAsync();

                using HttpClient client = new HttpClient();
                var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                
                var response = await client.PostAsync($"{ApiUrlBase}/configuracion", content);
                
                if (response.IsSuccessStatusCode)
                {
                    MensajeRespuesta = await response.Content.ReadAsStringAsync();
                }
                else 
                {
                    MensajeRespuesta = $"La API rechazó la petición. Código de error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                MensajeRespuesta = $"Fallo de conexión crítico: {ex.Message} \n¿Está el backend encendido en el puerto correcto?";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostTransaccionesAsync(IFormFile archivoTransac)
        {
            try
            {
                if (archivoTransac == null || archivoTransac.Length == 0)
                {
                    MensajeRespuesta = "Error: No seleccionaste ningún archivo o el archivo está vacío.";
                    return Page();
                }

                using var reader = new StreamReader(archivoTransac.OpenReadStream());
                string xmlContent = await reader.ReadToEndAsync();

                using HttpClient client = new HttpClient();
                var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");
                
                var response = await client.PostAsync($"{ApiUrlBase}/transacciones", content);
                
                if (response.IsSuccessStatusCode)
                {
                    MensajeRespuesta = await response.Content.ReadAsStringAsync();
                }
                else 
                {
                    MensajeRespuesta = $"La API rechazó la petición. Código de error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                MensajeRespuesta = $"Fallo de conexión crítico: {ex.Message} \n¿Está el backend encendido en el puerto correcto?";
            }

            return Page();
        }
    }
}
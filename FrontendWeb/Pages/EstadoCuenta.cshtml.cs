using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontendWeb.Pages
{
    public class ClienteResumen
    {
        public string nit { get; set; }
        public string nombre { get; set; }
        public decimal totalPagado { get; set; }
        public decimal saldoAFavor { get; set; }
    }

    public class EstadoCuentaModel : PageModel
    {
        public List<ClienteResumen> Clientes { get; set; } = new List<ClienteResumen>();
        
        public string MensajeError { get; set; } 
        
        private readonly string ApiUrl = "http://localhost:5184/api/consultas/clientes";

        public async Task OnGetAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(ApiUrl);
                
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Clientes = JsonSerializer.Deserialize<List<ClienteResumen>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    MensajeError = $"El backend rechazó la consulta. Código: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Fallo de conexión crítico: {ex.Message}";
            }
        }
    }
}
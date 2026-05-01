// code para las graficas

using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FrontendWeb.Pages
{
    public class BancoIngreso
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal total { get; set; }
    }

    public class GraficasModel : PageModel
    {
        public List<BancoIngreso> Bancos { get; set; } = new List<BancoIngreso>();
        public string MensajeError { get; set; }

        private readonly string ApiUrl = "http://localhost:5184/api/consultas/bancos";

        public async Task OnGetAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                var response = await client.GetAsync(ApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    Bancos = JsonSerializer.Deserialize<List<BancoIngreso>>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
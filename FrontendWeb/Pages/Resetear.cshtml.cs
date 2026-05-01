
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontendWeb.Pages
{
    public class ResetearModel : PageModel
    {
        public string MensajeSistema { get; set; }
        public string TipoAlerta { get; set; }

        
        public async Task OnPostAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                
                
                var response = await client.PostAsync("http://localhost:5184/api/sistema/limpiardatos", null);
                
                if (response.IsSuccessStatusCode)
                {
                    MensajeSistema = "¡El sistema se ha reseteado correctamente! La memoria está completamente vacía.";
                    TipoAlerta = "alert-success";
                }
                else
                {
                    MensajeSistema = $"El backend rechazó la solicitud. Código: {response.StatusCode}";
                    TipoAlerta = "alert-warning";
                }
            }
            catch (System.Exception ex)
            {
                MensajeSistema = $"Fallo de conexión crítico: {ex.Message}";
                TipoAlerta = "alert-danger";
            }
        }
    }
}

namespace BackendAPI.Modelos
{
    public class Factura
    {
        public string Numero { get; set; }
        public string Fecha { get; set; }
        public string NITCliente { get; set; }
        public decimal Total { get; set; }
        public decimal SaldoPendiente { get; set; }
    }

    
}
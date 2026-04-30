
namespace BackendAPI.Modelos
{
    

    public class Pago
    {
        public string CodigoBanco { get; set; }
        public string Fecha { get; set; }
        public string NITCliente { get; set; }
        public decimal Importe { get; set; }
    }
}
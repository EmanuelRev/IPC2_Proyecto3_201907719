//cleintesss

using BackendAPI.Estructuras;

namespace BackendAPI.Modelos
{
    public class Cliente
    {
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public decimal SaldoAFavor { get; set; } = 0;
        
    
        public Cola FacturasPendientes { get; set; } = new Cola();
    }
}
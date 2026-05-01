//cleintesss

using BackendAPI.Estructuras;

namespace BackendAPI.Modelos
{
    public class Cliente
    {
        public string NIT { get; set; }
        public string Nombre { get; set; }
        public decimal SaldoAFavor { get; set; } = 0;
        public decimal TotalPagado { get; set; } = 0; 
        public ListaEnlazada Facturas { get; set; } = new ListaEnlazada();
        public Pila Historial { get; set; } = new Pila();
    }
}

using BackendAPI.Estructuras;

namespace BackendAPI.Modelos
{
    public class Banco
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal TotalRecaudado { get; set; } = 0;
        
        
        public ListaEnlazada PagosRecibidos { get; set; } = new ListaEnlazada();
    }
}
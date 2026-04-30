// memoria cosdigp

using BackendAPI.Estructuras;

namespace BackendAPI.Datos
{
    public static class Memoria
    {
        
        public static ListaEnlazada ListaBancos { get; set; } = new ListaEnlazada();
        public static ListaEnlazada ListaClientes { get; set; } = new ListaEnlazada();
        
        
        public static Cola TransaccionesEntrantes { get; set; } = new Cola();
    }
}
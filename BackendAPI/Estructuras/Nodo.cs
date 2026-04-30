
// nodooo
namespace BackendAPI.Estructuras
{
    public class Nodo
    {
        public object Dato { get; set; }
        public Nodo Siguiente { get; set; }

        public Nodo(object dato)
        {
            Dato = dato;
            Siguiente = null;
        }
    }
}
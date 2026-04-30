//mantener el poliii

namespace BackendAPI.Estructuras
{
    public class ListaEnlazada
    {
        public Nodo Cabeza { get; private set; }

        public void Agregar(object dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (Cabeza == null)
            {
                Cabeza = nuevo;
            }
            else
            {
                Nodo actual = Cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevo;
            }
        }
    }
}
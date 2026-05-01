// pilasss

namespace BackendAPI.Estructuras
{
    public class Pila
    {
        public Nodo Tope { get; private set; }

        public void Push(object dato)
        {
            Nodo nuevo = new Nodo(dato);
            nuevo.Siguiente = Tope;
            Tope = nuevo;
        }

        public object Pop()
        {
            if (EstaVacia())
            {
                return null;
            }
            object dato = Tope.Dato;
            Tope = Tope.Siguiente;
            return dato;
        }

        public object VerTope()
        {
            return Tope != null ? Tope.Dato : null;
        }

        public bool EstaVacia()
        {
            return Tope == null;
        }
    }
}
// pilasss

namespace BackendAPI.Estructuras
{
    public class Pila
    {
        private Nodo tope;

        public void Push(object dato)
        {
            Nodo nuevo = new Nodo(dato);
            nuevo.Siguiente = tope;
            tope = nuevo;
        }

        public object Pop()
        {
            if (EstaVacia()) return null;

            object dato = tope.Dato;
            tope = tope.Siguiente;

            return dato;
        }

        public object Primero()
        {
            if (EstaVacia()) return null;
            return tope.Dato;
        }

        public bool EstaVacia()
        {
            return tope == null;
        }
    }
}
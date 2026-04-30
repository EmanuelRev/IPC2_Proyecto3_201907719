// colaa...

namespace BackendAPI.Estructuras
{
    public class Cola
    {
        private Nodo frente;
        private Nodo final;

        public void Push(object dato)
        {
            Nodo nuevo = new Nodo(dato);
            if (frente == null)
            {
                frente = nuevo;
                final = nuevo;
            }
            else
            {
                final.Siguiente = nuevo;
                final = nuevo;
            }
        }

        public object Pop()
        {
            if (EstaVacia()) return null;

            object dato = frente.Dato;
            frente = frente.Siguiente;
            if (frente == null) final = null; 

            return dato;
        }

        public object Primero()
        {
            if (EstaVacia()) return null;
            return frente.Dato;
        }

        public bool EstaVacia()
        {
            return frente == null;
        }
    }
}
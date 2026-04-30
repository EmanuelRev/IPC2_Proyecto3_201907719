// para eeror y pagosss

using BackendAPI.Estructuras;
using BackendAPI.Modelos;
using BackendAPI.Datos;
using System.Collections.Generic;

namespace BackendAPI.Utilidades
{
    public class MotorPagos
    {
        public static List<string> ErroresLogicos = new List<string>();

        public void ProcesarColaTransacciones()
        {
            ErroresLogicos.Clear();

            while (!Memoria.TransaccionesEntrantes.EstaVacia())
            {
                object transaccion = Memoria.TransaccionesEntrantes.Pop();

                if (transaccion is Factura)
                {
                    ProcesarFactura((Factura)transaccion);
                }
                else if (transaccion is Pago)
                {
                    ProcesarPago((Pago)transaccion);
                }
            }
        }

        private void ProcesarFactura(Factura factura)
        {
            Cliente cliente = BuscarCliente(factura.NITCliente);
            if (cliente == null)
            {
                ErroresLogicos.Add($"Factura {factura.Numero}: El cliente con NIT {factura.NITCliente} no existe en el sistema.");
                return;
            }

            if (cliente.SaldoAFavor > 0)
            {
                if (cliente.SaldoAFavor >= factura.SaldoPendiente)
                {
                    cliente.SaldoAFavor -= factura.SaldoPendiente;
                    factura.SaldoPendiente = 0;
                }
                else
                {
                    factura.SaldoPendiente -= cliente.SaldoAFavor;
                    cliente.SaldoAFavor = 0;
                }
            }

            if (factura.SaldoPendiente > 0)
            {
                cliente.FacturasPendientes.Push(factura);
            }

            cliente.Historial.Push(factura);
        }

        private void ProcesarPago(Pago pago)
        {
            Banco banco = BuscarBanco(pago.CodigoBanco);
            if (banco == null)
            {
                ErroresLogicos.Add($"Pago rechazado: El banco con código {pago.CodigoBanco} no existe en el sistema.");
                return;
            }

            Cliente cliente = BuscarCliente(pago.NITCliente);
            if (cliente == null)
            {
                ErroresLogicos.Add($"Pago rechazado: El cliente con NIT {pago.NITCliente} no existe en el sistema.");
                return;
            }

            cliente.SaldoAFavor += pago.Importe;

            while (cliente.SaldoAFavor > 0 && !cliente.FacturasPendientes.EstaVacia())
            {
                Factura facturaMasAntigua = (Factura)cliente.FacturasPendientes.Primero();

                if (cliente.SaldoAFavor >= facturaMasAntigua.SaldoPendiente)
                {
                    cliente.SaldoAFavor -= facturaMasAntigua.SaldoPendiente;
                    facturaMasAntigua.SaldoPendiente = 0;
                    cliente.FacturasPendientes.Pop();
                }
                else
                {
                    facturaMasAntigua.SaldoPendiente -= cliente.SaldoAFavor;
                    cliente.SaldoAFavor = 0;
                }
            }

            cliente.Historial.Push(pago);
        }

        private Cliente BuscarCliente(string nit)
        {
            Nodo actual = Memoria.ListaClientes.Cabeza;
            while (actual != null)
            {
                Cliente c = (Cliente)actual.Dato;
                if (c.NIT == nit) return c;
                actual = actual.Siguiente;
            }
            return null;
        }

        private Banco BuscarBanco(string codigo)
        {
            Nodo actual = Memoria.ListaBancos.Cabeza;
            while (actual != null)
            {
                Banco b = (Banco)actual.Dato;
                if (b.Codigo == codigo) return b;
                actual = actual.Siguiente;
            }
            return null;
        }
    }
}

// lector xmllll
using System;
using System.Xml;
using System.Text.RegularExpressions;
using BackendAPI.Estructuras;
using BackendAPI.Modelos;
using BackendAPI.Datos;

namespace BackendAPI.Utilidades
{
    public class LectorXML
    {
        private static readonly Regex RegexNIT = new Regex(@"[A-Za-z0-9\-]+");
        private static readonly Regex RegexFecha = new Regex(@"[0-3][0-9]/[0-1][0-9]/[0-9]{4}");

        private int bancosCreados = 0;
        private int bancosActualizados = 0;
        private int clientesCreados = 0;
        private int clientesActualizados = 0;

        private int facturasNuevas = 0;
        private int facturasDuplicadas = 0;
        private int facturasError = 0;
        private int pagosNuevos = 0;
        private int pagosDuplicados = 0;
        private int pagosError = 0;

        public void ProcesarConfiguracion(string xmlContent)
        {
            bancosCreados = 0;
            bancosActualizados = 0;
            clientesCreados = 0;
            clientesActualizados = 0;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            XmlNodeList nodosBancos = doc.SelectNodes("//banco"); 
            if (nodosBancos != null)
            {
                foreach (XmlNode nodo in nodosBancos)
                {
                    string codigo = nodo["codigo"]?.InnerText.Trim();
                    string nombre = nodo["nombre"]?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(codigo))
                    {
                        ActualizarOAgregarBanco(codigo, nombre);
                    }
                }
            }

            XmlNodeList nodosClientes = doc.SelectNodes("//cliente");
            if (nodosClientes != null)
            {
                foreach (XmlNode nodo in nodosClientes)
                {
                    string nitCrudo = nodo["NIT"]?.InnerText;
                    string nombre = nodo["nombre"]?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(nitCrudo))
                    {
                        Match matchNIT = RegexNIT.Match(nitCrudo);
                        if (matchNIT.Success)
                        {
                            string nitLimpio = matchNIT.Value;
                            ActualizarOAgregarCliente(nitLimpio, nombre);
                        }
                    }
                }
            }
        }

        private void ActualizarOAgregarBanco(string codigo, string nombre)
        {
            Nodo actual = Memoria.ListaBancos.Cabeza;
            bool existe = false;

            while (actual != null)
            {
                Banco b = (Banco)actual.Dato; 
                if (b.Codigo == codigo)
                {
                    b.Nombre = nombre; 
                    existe = true;
                    bancosActualizados++;
                    break;
                }
                actual = actual.Siguiente;
            }

            if (!existe)
            {
                Banco nuevoBanco = new Banco { Codigo = codigo, Nombre = nombre };
                Memoria.ListaBancos.Agregar(nuevoBanco);
                bancosCreados++;
            }
        }

        private void ActualizarOAgregarCliente(string nit, string nombre)
        {
            Nodo actual = Memoria.ListaClientes.Cabeza;
            bool existe = false;

            while (actual != null)
            {
                Cliente c = (Cliente)actual.Dato;
                if (c.NIT == nit)
                {
                    c.Nombre = nombre; 
                    existe = true;
                    clientesActualizados++;
                    break;
                }
                actual = actual.Siguiente;
            }

            if (!existe)
            {
                Cliente nuevoCliente = new Cliente { NIT = nit, Nombre = nombre };
                Memoria.ListaClientes.Agregar(nuevoCliente);
                clientesCreados++;
            }
        }

        public string GenerarRespuestaConfiguracion()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            xml += "<respuesta>\n";
            xml += "  <clientes>\n";
            xml += $"    <creados>{clientesCreados}</creados>\n";
            xml += $"    <actualizados>{clientesActualizados}</actualizados>\n";
            xml += "  </clientes>\n";
            xml += "  <bancos>\n";
            xml += $"    <creados>{bancosCreados}</creados>\n";
            xml += $"    <actualizados>{bancosActualizados}</actualizados>\n";
            xml += "  </bancos>\n";
            xml += "</respuesta>";
            
            return xml;
        }

        public void ProcesarTransacciones(string xmlContent)
        {
            facturasNuevas = 0;
            facturasDuplicadas = 0;
            facturasError = 0;
            pagosNuevos = 0;
            pagosDuplicados = 0;
            pagosError = 0;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            XmlNodeList nodosFacturas = doc.SelectNodes("//factura");
            if (nodosFacturas != null)
            {
                foreach (XmlNode nodo in nodosFacturas)
                {
                    string numero = nodo["numeroFactura"]?.InnerText.Trim();
                    string nitCrudo = nodo["NITcliente"]?.InnerText;
                    string fechaCruda = nodo["fecha"]?.InnerText;
                    string valorStr = nodo["valor"]?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(nitCrudo) && !string.IsNullOrEmpty(fechaCruda))
                    {
                        Match matchNIT = RegexNIT.Match(nitCrudo);
                        Match matchFecha = RegexFecha.Match(fechaCruda);

                        if (matchNIT.Success && matchFecha.Success && decimal.TryParse(valorStr, out decimal valor))
                        {
                            if (ExisteFactura(numero))
                            {
                                facturasDuplicadas++;
                            }
                            else
                            {
                                Factura nuevaFactura = new Factura
                                {
                                    Numero = numero,
                                    NITCliente = matchNIT.Value,
                                    Fecha = matchFecha.Value,
                                    Total = valor,
                                    SaldoPendiente = valor 
                                };
                                Memoria.TransaccionesEntrantes.Push(nuevaFactura);
                                facturasNuevas++;
                            }
                        }
                        else
                        {
                            facturasError++;
                        }
                    }
                    else
                    {
                        facturasError++;
                    }
                }
            }

            XmlNodeList nodosPagos = doc.SelectNodes("//pago");
            if (nodosPagos != null)
            {
                foreach (XmlNode nodo in nodosPagos)
                {
                    string codigoBanco = nodo["codigoBanco"]?.InnerText.Trim();
                    string nitCrudo = nodo["NITcliente"]?.InnerText;
                    string fechaCruda = nodo["fecha"]?.InnerText;
                    string importeStr = nodo["valor"]?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(nitCrudo) && !string.IsNullOrEmpty(fechaCruda))
                    {
                        Match matchNIT = RegexNIT.Match(nitCrudo);
                        Match matchFecha = RegexFecha.Match(fechaCruda);

                        if (matchNIT.Success && matchFecha.Success && decimal.TryParse(importeStr, out decimal importe))
                        {
                            Pago nuevoPago = new Pago
                            {
                                CodigoBanco = codigoBanco,
                                NITCliente = matchNIT.Value,
                                Fecha = matchFecha.Value,
                                Importe = importe
                            };
                            Memoria.TransaccionesEntrantes.Push(nuevoPago);
                            pagosNuevos++;
                        }
                        else
                        {
                            pagosError++;
                        }
                    }
                    else
                    {
                        pagosError++;
                    }
                }
            }
        }

        private bool ExisteFactura(string numero)
        {
            Nodo actualCliente = Memoria.ListaClientes.Cabeza;
            while (actualCliente != null)
            {
                Cliente c = (Cliente)actualCliente.Dato;
                Nodo actualFactura = c.Facturas.Cabeza;
                while (actualFactura != null)
                {
                    Factura f = (Factura)actualFactura.Dato;
                    if (f.Numero == numero)
                    {
                        return true;
                    }
                    actualFactura = actualFactura.Siguiente;
                }
                actualCliente = actualCliente.Siguiente;
            }
            return false;
        }

        public string GenerarRespuestaTransacciones()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
            xml += "<transacciones>\n";
            xml += "  <facturas>\n";
            xml += $"    <nuevasFacturas>{facturasNuevas}</nuevasFacturas>\n";
            xml += $"    <facturasDuplicadas>{facturasDuplicadas}</facturasDuplicadas>\n";
            xml += $"    <facturasConError>{facturasError}</facturasConError>\n";
            xml += "  </facturas>\n";
            xml += "  <pagos>\n";
            xml += $"    <nuevosPagos>{pagosNuevos}</nuevosPagos>\n";
            xml += $"    <pagosDuplicados>{pagosDuplicados}</pagosDuplicados>\n";
            xml += $"    <pagosConError>{pagosError}</pagosConError>\n";
            xml += "  </pagos>\n";
            xml += "</transacciones>";
            
            return xml;
        }
    }
}
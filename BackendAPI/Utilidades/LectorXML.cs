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
        private static readonly Regex RegexNIT = new Regex(@"[0-9]+-[0-9Kk]+");
        private static readonly Regex RegexFecha = new Regex(@"[0-3][0-9]/[0-1][0-9]/[0-9]{4}");

        public void ProcesarConfiguracion(string xmlContent)
        {
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
                    string nitCrudo = nodo["nit"]?.InnerText;
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
                    break;
                }
                actual = actual.Siguiente;
            }

            if (!existe)
            {
                Banco nuevoBanco = new Banco { Codigo = codigo, Nombre = nombre };
                Memoria.ListaBancos.Agregar(nuevoBanco);
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
                    break;
                }
                actual = actual.Siguiente;
            }

            if (!existe)
            {
                Cliente nuevoCliente = new Cliente { NIT = nit, Nombre = nombre };
                Memoria.ListaClientes.Agregar(nuevoCliente);
            }
        }

        public void ProcesarTransacciones(string xmlContent)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            XmlNodeList nodosFacturas = doc.SelectNodes("//factura");
            if (nodosFacturas != null)
            {
                foreach (XmlNode nodo in nodosFacturas)
                {
                    string numero = nodo["numero"]?.InnerText.Trim();
                    string nitCrudo = nodo["nit"]?.InnerText;
                    string fechaCruda = nodo["fecha"]?.InnerText;
                    string valorStr = nodo["valor"]?.InnerText.Trim();

                    if (!string.IsNullOrEmpty(nitCrudo) && !string.IsNullOrEmpty(fechaCruda))
                    {
                        Match matchNIT = RegexNIT.Match(nitCrudo);
                        Match matchFecha = RegexFecha.Match(fechaCruda);

                        if (matchNIT.Success && matchFecha.Success && decimal.TryParse(valorStr, out decimal valor))
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
                        }
                    }
                }
            }

            XmlNodeList nodosPagos = doc.SelectNodes("//pago");
            if (nodosPagos != null)
            {
                foreach (XmlNode nodo in nodosPagos)
                {
                    string codigoBanco = nodo["codigoBanco"]?.InnerText.Trim();
                    string nitCrudo = nodo["nit"]?.InnerText;
                    string fechaCruda = nodo["fecha"]?.InnerText;
                    string importeStr = nodo["importe"]?.InnerText.Trim();

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
                        }
                    }
                }
            }
        }
    }
}
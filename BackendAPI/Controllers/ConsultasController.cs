// contolador ----------

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using BackendAPI.Estructuras;
using BackendAPI.Modelos;
using BackendAPI.Datos;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsultasController : ControllerBase
    {
        [HttpGet("clientes")]
        public IActionResult ObtenerEstadoClientes()
        {
            var listaResumen = new List<object>();

            Nodo actual = Memoria.ListaClientes.Cabeza;
            while (actual != null)
            {
                Cliente c = (Cliente)actual.Dato;

                listaResumen.Add(new
                {
                    nit = c.NIT,
                    nombre = c.Nombre,
                    totalPagado = c.TotalPagado, 
                    saldoAFavor = c.SaldoAFavor
                });

                actual = actual.Siguiente;
            }

            return Ok(listaResumen);
        }

        [HttpGet("bancos")]
        public IActionResult ObtenerIngresosBancos()
        {
            var listaIngresos = new List<object>();

            Nodo actual = Memoria.ListaBancos.Cabeza;
            while (actual != null)
            {
                Banco b = (Banco)actual.Dato;
                listaIngresos.Add(new
                {
                    codigo = b.Codigo,
                    nombre = b.Nombre,
                    total = b.TotalRecaudado
                });

                actual = actual.Siguiente;
            }

            return Ok(listaIngresos);
        }

        [HttpGet("ingresos-tres-meses")]
        public IActionResult GetIngresosTresMeses(int mes, int anio)
        {
            var resultado = new List<object>();
            Nodo actualBanco = Memoria.ListaBancos.Cabeza;

            while (actualBanco != null)
            {
                Banco b = (Banco)actualBanco.Dato;
                decimal[] totales = new decimal[3];

                for (int i = 0; i < 3; i++)
                {
                    int mesBusqueda = mes - i;
                    int anioBusqueda = anio;
                    
                    if (mesBusqueda <= 0)
                    {
                        mesBusqueda += 12;
                        anioBusqueda--;
                    }

                    Nodo actualPago = b.PagosRecibidos.Cabeza;
                    while (actualPago != null)
                    {
                        Pago p = (Pago)actualPago.Dato;
                        DateTime fechaPago = DateTime.ParseExact(p.Fecha, "dd/MM/yyyy", null);

                        if (fechaPago.Month == mesBusqueda && fechaPago.Year == anioBusqueda)
                        {
                            totales[i] += p.Importe;
                        }
                        actualPago = actualPago.Siguiente;
                    }
                }

                resultado.Add(new
                {
                    nombre = b.Nombre,
                    m1 = totales[0],
                    m2 = totales[1],
                    m3 = totales[2]
                });
                
                actualBanco = actualBanco.Siguiente;
            }
            
            return Ok(resultado);
        }
    }
}
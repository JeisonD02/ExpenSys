using ExpenSysCom.Models;
using System.Collections.Generic;

namespace ExpenSysCom.ViewModels
{
    public class DashboardViewModel
    {
        // Totales
        public decimal TotalGastos { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalAhorros { get; set; }
        public decimal TotalMetas { get; set; }


        public List<string> Anos { get; set; }
        public List<decimal> GastosAno { get; set; }
        public List<decimal> IngresosAno { get; set; }

        public List<string> Meses { get; set; }
        public List<decimal> GastosMes { get; set; }
        public List<decimal> IngresosMes { get; set; }

        public List<string> Semanas { get; set; }
        public List<decimal> GastosSemana { get; set; }
        public List<decimal> IngresosSemana { get; set; }

        public List<string> Dias { get; set; }
        public List<decimal> GastosDia { get; set; }
        public List<decimal> IngresosDia { get; set; }




        //Saludo al Usuario
        public string NombreUsuario { get; set; }


        
        public List<Gasto> Gastos { get; set; }
        public List<Ingreso> Ingresos { get; set; }
        public List<Meta> Metas { get; set; }
        public List<Ahorro> Ahorros { get; set; }

        public DashboardViewModel()
        {
            // Inicializamos las listas para evitar errores de referencia nula
            Gastos = new List<Gasto>();
            Ingresos = new List<Ingreso>();
            Metas = new List<Meta>();
            Ahorros = new List<Ahorro>();
        }
    }
}

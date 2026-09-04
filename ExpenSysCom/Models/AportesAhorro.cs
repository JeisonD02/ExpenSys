using System;
using System.Collections.Generic;

namespace ExpenSysCom.Models;

public partial class AportesAhorro
{
    public int IdAporteAhorro { get; set; }

    public decimal Valor { get; set; }

    public DateTime Fecha { get; set; }

    public int IdAhorro { get; set; }

    public int IdIngreso { get; set; }

    public virtual Ahorro? IdAhorroNavigation { get; set; } = null!;

    public virtual Ingreso? IdIngresoNavigation { get; set; } = null!;
}

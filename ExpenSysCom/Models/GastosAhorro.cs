using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenSysCom.Models;

public partial class GastosAhorro
{
    [Key]
    public int IdGastoAhorro { get; set; }

    public decimal Valor { get; set; }

    public DateTime Fecha { get; set; }


    public int IdAhorro { get; set; }

    public int IdGasto { get; set; }

    public virtual Ahorro IdAhorroNavigation { get; set; } = null!;

    public virtual Gasto IdGastoNavigation { get; set; } = null!;
}

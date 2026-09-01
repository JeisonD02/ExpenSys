using System;
using System.Collections.Generic;
using ExpenSysCom.Data;

namespace ExpenSysCom.Modelos;

public partial class Meta
{
    public int IdMeta { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Valor { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public int IdAhorro { get; set; }

    public string IdUsuario { get; set; }

    public int IdEstado { get; set; }

    public virtual Ahorro IdAhorroNavigation { get; set; } = null!;

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace ExpenSysCom.Modelos;

public partial class Ingreso
{
    public int IdIngreso { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Valor { get; set; }

    public DateTime Fecha { get; set; }

    public string IdUsuario { get; set; } = null!;

    public int IdCategoriaIngreso { get; set; }

    public virtual ICollection<AportesAhorro> AportesAhorros { get; set; } = new List<AportesAhorro>();

    public virtual CategoriasIngreso IdCategoriaIngresoNavigation { get; set; } = null!;

    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;
}

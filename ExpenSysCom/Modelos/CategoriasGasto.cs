using System;
using System.Collections.Generic;

namespace ExpenSysCom.Modelos;

public partial class CategoriasGasto
{
    public int IdCategoriaGasto { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
}

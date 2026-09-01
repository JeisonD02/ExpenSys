using System;
using System.Collections.Generic;

namespace ExpenSysCom.Modelos;

public partial class CategoriasIngreso
{
    public int IdCategoriaIngreso { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
}

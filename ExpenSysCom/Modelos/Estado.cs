using System;
using System.Collections.Generic;

namespace ExpenSysCom.Modelos;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ahorro> Ahorros { get; set; } = new List<Ahorro>();

    public virtual ICollection<Meta> Meta { get; set; } = new List<Meta>();
}

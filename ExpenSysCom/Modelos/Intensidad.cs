using System;
using System.Collections.Generic;

namespace ExpenSysCom.Modelos;

public partial class Intensidad
{
    public int IdIntensidad { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Ahorro> Ahorros { get; set; } = new List<Ahorro>();
}

using System;
using System.Collections.Generic;
using ExpenSysCom.Enums;

namespace ExpenSysCom.Models;

public partial class Ingreso
{
    public int IdIngreso { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Valor { get; set; }

    public DateTime Fecha { get; set; }

    public string IdUsuario { get; set; } = null!;


    public virtual ICollection<AportesAhorro> AportesAhorros { get; set; } = new List<AportesAhorro>();

    public CategoriaIngreso CategoriaIngreso { get; set; }

    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;
}

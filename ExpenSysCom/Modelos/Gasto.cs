using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ExpenSysCom.Modelos;

public partial class Gasto
{
    public int IdGasto { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Valor { get; set; }

    public DateTime Fecha { get; set; }

    public string IdUsuario { get; set; }

    public int IdCategoriaGasto { get; set; }

    public virtual ICollection<GastosAhorro> GastosAhorros { get; set; } = new List<GastosAhorro>();

    public virtual CategoriasGasto IdCategoriaGastoNavigation { get; set; } = null!;

    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;
}

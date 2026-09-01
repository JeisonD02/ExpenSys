using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using ExpenSysCom.Data;

namespace ExpenSysCom.Modelos;

public partial class Ahorro
{
    public int IdAhorro { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Porcentaje { get; set; }

    public string IdUsuario { get; set; } = null!;

    public int IdEstado { get; set; }

    public int IdIntensidad { get; set; }

    public virtual ICollection<AportesAhorro> AportesAhorros { get; set; } = new List<AportesAhorro>();

    public virtual ICollection<GastosAhorro> GastosAhorros { get; set; } = new List<GastosAhorro>();

    public virtual Estado IdEstadoNavigation { get; set; } = null!;

    public virtual Intensidad IdIntensidadNavigation { get; set; } = null!;

    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Meta> Meta { get; set; } = new List<Meta>();
}
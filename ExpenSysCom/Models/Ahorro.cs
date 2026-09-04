using ExpenSysCom.Data;
using ExpenSysCom.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace ExpenSysCom.Models;

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


    public Estado Estado { get; set;}

    public Intensidad Intensidad { get; set; }


    public virtual ApplicationUser IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Meta> Meta { get; set; } = new List<Meta>();
}
using ExpenSysCom.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

public class FechaMaximaActualAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is DateTime fecha)
        {
            if (fecha > DateTime.Today)
            {
                return new ValidationResult("No se permiten fechas futuras");
            }
        }
        return ValidationResult.Success;
    }
}

public class GastoViewModel
{
    public int IdGasto { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Ingrese un Valor valido")]
    public decimal Valor { get; set; }


    [DataType(DataType.Date)]
    [Required(ErrorMessage = "La fecha es obligatoria")]
    [FechaMaximaActual(ErrorMessage = "La fecha no puede ser futura")]
    public DateTime Fecha { get; set; }


    [Required(ErrorMessage = "Debe seleccionar una categoría")]
    public CategoriaGasto CategoriaGasto { get; set; }
}

using System;
using System.Collections.Generic;
using System.IO;
using ExpenSysCom.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace ExpenSysCom.Services
{
    public class ReportePdf
    {
        private string TextoSeguro(string texto, string valorPorDefecto = "")
        {
            return string.IsNullOrEmpty(texto) ? valorPorDefecto : texto;
        }

        public byte[] CrearPdf(
            string tipoReporte,
            string nombreEmpresa,
            string usuario,
            List<DatoReporte> items,
            string rutaLogo,  
            string desde,
            string hasta)
        {
            PdfDocument doc = new PdfDocument();
            doc.Info.Title = $"Reporte de {tipoReporte}";

            var fontTitulo = new XFont("Arial", 28, XFontStyle.Bold);
            var fontSubtitulo = new XFont("Arial", 12, XFontStyle.Regular);
            var fontTabla = new XFont("Arial", 10, XFontStyle.Regular);
            var fontTablaBold = new XFont("Arial", 10, XFontStyle.Bold);

            var verde = XColor.FromArgb(0, 128, 0);
            var negro = XColors.Black;
            var grisClaro = XColor.FromArgb(240, 240, 240);

            PdfPage page = doc.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            double margin = 40;
            double y = margin;

            //logo
            try
            {
                if (File.Exists(rutaLogo))
                {
                    using var imgStream = File.OpenRead(rutaLogo);
                    var logo = XImage.FromStream(() => imgStream);

                    
                    double logoWidth = 100;
                    double logoHeight = logo.PixelHeight * (logoWidth / logo.PixelWidth);
                    gfx.DrawImage(logo, margin, y, logoWidth, logoHeight);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar el logo: " + ex.Message);
            }

            
            double alturaLogo = 50; 
            double alturaTexto = fontTitulo.Height;
            double yTexto = y + (alturaLogo - alturaTexto) / 2;


            var textWidth = gfx.MeasureString("ExpenSys", fontTitulo).Width;
            double xTexto = (page.Width - textWidth) / 2; 

            gfx.DrawString("ExpenSys", fontTitulo, new XSolidBrush(verde),
                new XPoint(xTexto, yTexto + alturaTexto),
                XStringFormats.Default);

            y += alturaLogo + 20;

            
            gfx.DrawString($"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm}", fontSubtitulo, XBrushes.Gray, margin, y);
            y += 15;
            gfx.DrawString($"Usuario: {usuario}", fontSubtitulo, XBrushes.Gray, margin, y);

            y += 30;

           
            gfx.DrawString($"Reporte: {tipoReporte}", fontSubtitulo, XBrushes.Black, margin, y);
            y += 20;

          
            gfx.DrawString($"Desde: {desde} - Hasta: {hasta}", fontSubtitulo, XBrushes.Black, margin, y);
            y += 30;

           

            double anchoDisponible = page.Width - 2 * margin;

            double anchoNombre = anchoDisponible * 0.25;
            double anchoCategoria = anchoDisponible * 0.20;    
            double anchoDescripcion = anchoDisponible * 0.30;
            double anchoValor = anchoDisponible * 0.15;
            double anchoFecha = anchoDisponible * 0.15;

            double[] columnasX = {
                margin,
                margin + anchoNombre,
                margin + anchoNombre + anchoCategoria,
                margin + anchoNombre + anchoCategoria + anchoDescripcion,
                margin + anchoNombre + anchoCategoria + anchoDescripcion + anchoValor,
                margin + anchoNombre + anchoCategoria + anchoDescripcion + anchoValor + anchoFecha
            };

            double alturaFila = 20;

           
            gfx.DrawRectangle(new XSolidBrush(grisClaro), margin, y - 15, anchoDisponible, alturaFila);

            
            gfx.DrawString("Nombre", fontTablaBold, XBrushes.Black, new XRect(columnasX[0] + 5, y - 15, anchoNombre - 10, alturaFila), XStringFormats.CenterLeft);
            gfx.DrawString("Categoría", fontTablaBold, XBrushes.Black, new XRect(columnasX[1] + 5, y - 15, anchoCategoria - 10, alturaFila), XStringFormats.CenterLeft);
            gfx.DrawString("Descripción", fontTablaBold, XBrushes.Black, new XRect(columnasX[2] + 5, y - 15, anchoDescripcion - 10, alturaFila), XStringFormats.CenterLeft);
            gfx.DrawString("Valor", fontTablaBold, XBrushes.Black, new XRect(columnasX[3] + 5, y - 15, anchoValor - 10, alturaFila), XStringFormats.CenterRight);
            gfx.DrawString("Fecha", fontTablaBold, XBrushes.Black, new XRect(columnasX[4] + 5, y - 15, anchoFecha - 10, alturaFila), XStringFormats.Center);

            y += alturaFila;

            bool filaPar = false;

            foreach (var item in items)
            {
                if (y + alturaFila > page.Height - margin)
                {
                    gfx.DrawString($"Página {doc.PageCount}", fontSubtitulo, XBrushes.Gray,
                        new XRect(margin, page.Height - margin + 10, page.Width - 2 * margin, 20), XStringFormats.Center);

                    page = doc.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = margin;

                    gfx.DrawRectangle(new XSolidBrush(grisClaro), margin, y - 15, anchoDisponible, alturaFila);

                    gfx.DrawString("Nombre", fontTablaBold, XBrushes.Black, new XRect(columnasX[0] + 5, y - 15, anchoNombre - 10, alturaFila), XStringFormats.CenterLeft);
                    gfx.DrawString("Categoría", fontTablaBold, XBrushes.Black, new XRect(columnasX[1] + 5, y - 15, anchoCategoria - 10, alturaFila), XStringFormats.CenterLeft);
                    gfx.DrawString("Descripción", fontTablaBold, XBrushes.Black, new XRect(columnasX[2] + 5, y - 15, anchoDescripcion - 10, alturaFila), XStringFormats.CenterLeft);
                    gfx.DrawString("Valor", fontTablaBold, XBrushes.Black, new XRect(columnasX[3] + 5, y - 15, anchoValor - 10, alturaFila), XStringFormats.CenterRight);
                    gfx.DrawString("Fecha", fontTablaBold, XBrushes.Black, new XRect(columnasX[4] + 5, y - 15, anchoFecha - 10, alturaFila), XStringFormats.Center);

                    y += alturaFila;
                }

                if (filaPar)
                {
                    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(245, 245, 245)), margin, y - 3, anchoDisponible, alturaFila);
                }
                filaPar = !filaPar;

                
                gfx.DrawString(TextoSeguro(item.Nombre, "(Sin nombre)"), fontTabla, XBrushes.Black, new XRect(columnasX[0] + 5, y, anchoNombre - 10, alturaFila), XStringFormats.CenterLeft);
                gfx.DrawString(TextoSeguro(item.Categoria, "(Sin categoría)"), fontTabla, XBrushes.Black, new XRect(columnasX[1] + 5, y, anchoCategoria - 10, alturaFila), XStringFormats.CenterLeft);
                gfx.DrawString(TextoSeguro(item.Descripcion, "(Sin descripción)"), fontTabla, XBrushes.Black, new XRect(columnasX[2] + 5, y, anchoDescripcion - 10, alturaFila), XStringFormats.CenterLeft);
                gfx.DrawString(item.Valor.ToString("C"), fontTabla, XBrushes.Black, new XRect(columnasX[3] + 5, y, anchoValor - 10, alturaFila), XStringFormats.CenterRight);
                gfx.DrawString(item.Fecha.ToShortDateString(), fontTabla, XBrushes.Black, new XRect(columnasX[4] + 5, y, anchoFecha - 10, alturaFila), XStringFormats.Center);

                y += alturaFila;
            }

            gfx.DrawString($"Página {doc.PageCount}", fontSubtitulo, XBrushes.Gray,
                new XRect(margin, page.Height - margin + 10, page.Width - 2 * margin, 20), XStringFormats.Center);

            using var ms = new MemoryStream();
            doc.Save(ms);

            return ms.ToArray();
        }
    }
}
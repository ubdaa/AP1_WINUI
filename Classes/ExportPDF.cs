using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AP1_WINUI.Data.Modeles;
using AP1_WINUI.Service;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace AP1_WINUI
{
    internal class ExportPDF
    {

        // J'aime vraiment pas comment la méthode est écrite, y'a moyen de faire bien mieux, mais je comprends pas tout à l'UWP
        public static async void ConvertirFicheEnPdf(Data.Modeles.FicheFrais ficheFrais, string titre)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont policeTitre = new XFont("Segoe UI", 24, XFontStyle.Bold);
            XFont policeInfo = new XFont("Segoe UI", 16);
            gfx.DrawString("Fiche Frais", policeTitre, XBrushes.Black, new XRect(0, 20, page.Width, page.Height), XStringFormats.TopCenter);
            gfx.DrawString($"11 {ficheFrais.Date:MMMM yyyy} - 10 {ficheFrais.Date.AddMonths(1):MMMM yyyy}", policeInfo, XBrushes.Black, new XRect(0, 60, page.Width, page.Height), XStringFormats.TopCenter);
            gfx.DrawString($"De {await LoginService.NomUtilisateur(ficheFrais.IdUtilisateur)} - Fiche n°{ficheFrais.IdFicheFrais} - Etat fiche : {ficheFrais.Etat}", policeInfo, XBrushes.Black, new XRect(0, 100, page.Width, page.Height), XStringFormats.TopCenter);

            double x = 50;
            double y = 140;
            double width = page.Width - 2 * x; 
            double height = 20; 

            XFont policeHeader = new XFont("Segoe UI", 12, XFontStyle.Bold);
            XFont policeCell = new XFont("Segoe UI", 12);
            if (ficheFrais.Forfaits.Count != 0)
            {
                gfx.DrawString("Forfaits", policeTitre, XBrushes.Black, new XRect(x, y, width, height), XStringFormats.TopLeft);
                y += height * 2;
                {
                    string[] headers = { "Date", "Libellé", "Quantité", "Montant", "Total" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        // clairement une formule tirée par les cheveux j'ai dû la trouver sur internet
                        gfx.DrawRectangle(XPens.Black, XBrushes.LightGray, x + i * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString(headers[i], policeHeader, XBrushes.Black, new XRect(x + i * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);
                    }
                
                    // merci internet
                    foreach (var frais in ficheFrais.Forfaits)
                    {
                        y += height;
                        gfx.DrawRectangle(XPens.Black, x, y, width / headers.Length, height);
                        gfx.DrawString(frais.Date.ToString(), policeCell, XBrushes.Black, new XRect(x, y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + width / headers.Length, y, width / headers.Length, height);
                        gfx.DrawString(frais.Nom, policeCell, XBrushes.Black, new XRect(x + width / headers.Length, y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + 2 * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString(frais.Quantite.ToString(), policeCell, XBrushes.Black, new XRect(x + 2 * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + 3 * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString($"{frais.Montant:F2}", policeCell, XBrushes.Black, new XRect(x + 3 * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + 4 * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString($"{frais.Quantite * frais.Montant:F2}", policeCell, XBrushes.Black, new XRect(x + 4 * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);
                    }
                }
            }

            // l'espace entre les deux tableaux
            if (ficheFrais.Forfaits.Count != 0)
                y += height * (ficheFrais.Forfaits.Count + 1) + 20;

            if (ficheFrais.HorsForfaits.Count != 0)
            {
                gfx.DrawString("Hors Forfaits", policeTitre, XBrushes.Black, new XRect(x, y, width, height), XStringFormats.TopLeft);
                y += height * 2;
                {
                    string[] headers = { "Date", "Libellé", "Montant" };

                    for (int i = 0; i < headers.Length; i++)
                    {
                        gfx.DrawRectangle(XPens.Black, XBrushes.LightGray, x + i * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString(headers[i], policeHeader, XBrushes.Black, new XRect(x + i * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);
                    }

                    foreach (var frais in ficheFrais.HorsForfaits)
                    {
                        y += height;
                        gfx.DrawRectangle(XPens.Black, x, y, width / headers.Length, height);
                        gfx.DrawString(frais.Date.ToString(), policeCell, XBrushes.Black, new XRect(x, y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + width / headers.Length, y, width / headers.Length, height);
                        gfx.DrawString(frais.Nom, policeCell, XBrushes.Black, new XRect(x + width / headers.Length, y, width / headers.Length, height), XStringFormats.Center);

                        gfx.DrawRectangle(XPens.Black, x + 2 * (width / headers.Length), y, width / headers.Length, height);
                        gfx.DrawString($"{frais.Montant:F2}", policeCell, XBrushes.Black, new XRect(x + 2 * (width / headers.Length), y, width / headers.Length, height), XStringFormats.Center);
                    }
                }
            }

            // Sauvegarder le document PDF
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            savePicker.FileTypeChoices.Add("PDF Document", new List<string>() { ".pdf" });
            savePicker.SuggestedFileName = titre;

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outputStream = stream.AsStreamForWrite())
                    {
                        document.Save(outputStream);
                        outputStream.Flush();
                    }
                }
            }
        }
    }
}
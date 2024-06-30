using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using AP1_WINUI.Data.Modeles;
using AP1_WINUI.Service;
using System.Reflection;

namespace AP1_WINUI
{
    internal class ExportPDF
    {
        public static PdfPTable ConvertListToTablePdf<T>(List<T> list)
        {
            Type type = list.GetType();
            int nombreAttributs = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length;

            BaseColor headerRow = new BaseColor(227, 227, 227);
            PdfPTable table = new PdfPTable(nombreAttributs);
            table.WidthPercentage = 100;

            // colonne des header de table
            if (list is List<Forfait>)
            {
                PdfPCell cell = new PdfPCell(new Phrase("Date"));
                cell.BackgroundColor = headerRow;
                table.AddCell(cell);

                //PdfPCell cell = new PdfPCell(new Phrase("Nom"));
                cell.BackgroundColor = headerRow;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Montant"));
                cell.BackgroundColor = headerRow;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Quantite"));
                cell.BackgroundColor = headerRow;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Total"));
                cell.BackgroundColor = headerRow;
                table.AddCell(cell);
            }

            foreach (object item in list)
            {
                PdfPCell cell = new PdfPCell(new Phrase(item.ToString()));
                table.AddCell(cell);
            }

            return table;
        }

        public static async void ConvertirFicheEnPdf(Data.Modeles.FicheFrais ficheFrais, string cheminFichier, string titre)
        {
            string directory = Path.GetDirectoryName(cheminFichier);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(cheminFichier, FileMode.Create));
            document.Open();

            // Ajouter le titre
            {
                Font titleFont = FontFactory.GetFont("Sogoe UI", 24, Font.BOLD);
                Font titleDescript = FontFactory.GetFont("Sogoe UI", 16);

                Paragraph p = new Paragraph("Fiche Frais", titleFont);
                document.Add(p);
                document.Add(new Paragraph("\n"));

                p = new Paragraph("11 " + ficheFrais.Date.ToString("MMMM yyyy") + " - 10 " + ficheFrais.Date.AddMonths(1).ToString("MMMM yyyy"), titleDescript);
                document.Add(p);

                p = new Paragraph("De " + (await LoginService.NomUtilisateur(ficheFrais.IdUtilisateur)) + " - Fiche n°" + ficheFrais.IdFicheFrais + " - Etat fiche : " + ficheFrais.Etat.ToString(), titleDescript);
                document.Add(p);
                document.Add(new Paragraph("\n"));

            }

            //document.Add(ConvertListToTablePdf(ficheFrais.Forfaits));

            document.Close();

            OuvrirPdfDansNavigateur(cheminFichier);
        }

        public static void OuvrirPdfDansNavigateur(string cheminFichier)
        {
            Process.Start(cheminFichier);
        }
    }
}

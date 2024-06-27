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

namespace AP1_WINUI
{
    internal class ExportPDF
    {
        public static void ConvertirFicheEnPdf(Data.Modeles.FicheFrais ficheFrais, string cheminFichier, string titre)
        {
            string directory = Path.GetDirectoryName(cheminFichier);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            Document document = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(cheminFichier, FileMode.Create));
            document.Open();

            // Ajouter le titre
            {
                Font titleFont = FontFactory.GetFont("Sogoe UI", 18, Font.BOLD);
                Font titleDescript = FontFactory.GetFont("Sogoe UI", 10);

                Paragraph title = new Paragraph(titre, titleFont);
                document.Add(title);
                document.Add(new Paragraph("\n"));
            }

            //pour la table
            /*{
                BaseColor colEcart = new BaseColor(206, 245, 206);
                BaseColor colRecalageVert = new BaseColor(144, 238, 144);
                BaseColor colRecalageRouge = new BaseColor(240, 128, 128);
                BaseColor colVeh = new BaseColor(245, 245, 220);
                BaseColor headerRow = new BaseColor(227, 227, 227);

                PdfPTable pdfTable = new PdfPTable(dt.Columns.Count);
                pdfTable.WidthPercentage = 100;
                Font font = FontFactory.GetFont("Arial", 7);
                Font fontHeader = FontFactory.GetFont("Arial", 7, Font.BOLD);


                float[] columnWidths = new float[dt.Columns.Count];
                int nbColumns = dt.Columns.Count;
                for (int i = 0; i < nbColumns; i++)
                {
                    columnWidths[i] = 1f;
                }
                pdfTable.SetWidths(columnWidths);

                // Ajouter les en-têtes de colonnes
                foreach (DataColumn column in dt.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.ColumnName, fontHeader));
                    cell.Padding = 3;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.BackgroundColor = headerRow;
                    pdfTable.AddCell(cell);
                }

                // Ajouter les données de la DataTable
                foreach (DataRow row in dt.Rows)
                {
                    int index = 0;
                    foreach (object cellData in row.ItemArray)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(cellData.ToString(), font));
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;

                        if (index == nbColumns - 2)
                            cell.BackgroundColor = colEcart;
                        if (index == nbColumns - 1)
                        {
                            int rec = int.Parse(cellData.ToString());
                        }
                        if (index == 0 || index == 1 || index == 2)
                            cell.BackgroundColor = colVeh;

                        cell.Padding = 3;
                        pdfTable.AddCell(cell);

                        index++;
                    }
                }
                document.Add(pdfTable);
            }

            // date de génération
            {
                Font titleFont = FontFactory.GetFont("Arial", 7, Font.ITALIC);

                Paragraph title = new Paragraph("Fichier généré à " + DateTime.Now.ToString(), titleFont);
                title.Alignment = Element.ALIGN_RIGHT;
                document.Add(title);
            }*/

            document.Close();

            OuvrirPdfDansNavigateur(cheminFichier);
        }

        public static void OuvrirPdfDansNavigateur(string cheminFichier)
        {
            Process.Start(cheminFichier);
        }
    }
}

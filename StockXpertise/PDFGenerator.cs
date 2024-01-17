using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Diagnostics;

namespace StockXpertise
{
    public class PDFGenerator
    {
        public static void GeneratePDF(string texte, Table table, string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    PdfWriter writer = new PdfWriter(fs);
                    PdfDocument pdf = new PdfDocument(writer);
                    Document document = new Document(pdf);

                    if (!string.IsNullOrEmpty(texte) && table != null)
                    {
                        // Générer à la fois le texte et le tableau sur la même page
                        Paragraph paragraph = new Paragraph(texte);
                        paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);

                        table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        Div container = new Div()
                            .Add(paragraph)
                            .Add(new Paragraph("\n"))
                            .Add(table);

                        document.Add(container);
                    }
                    else if (!string.IsNullOrEmpty(texte))
                    {
                        // Générer uniquement le texte
                        Paragraph paragraph = new Paragraph(texte);
                        paragraph.SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER);
                        document.Add(paragraph);
                    }
                    else if (table != null)
                    {
                        // Générer uniquement le tableau
                        table.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        document.Add(table);
                    }
                    document.Close();

                    MessageBox.Show("Le fichier PDF a été généré avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Ouvre le fichier avec l'application par défaut après la génération
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue lors de la génération du PDF : " + ex.Message);
            }
        }
    }
}

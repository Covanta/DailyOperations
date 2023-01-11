using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace CovantaWebForms.Classes.Helpers
{
    public static class PDFHelper
    {
        public static string CreateSupplierDataPdf(string supplierId,
                                    string fullName,
                                    string phoneNumber,
                                    string email,
                                    string company,
                                    string street,
                                    string city,
                                    string state,
                                    string zipCode,
                                    string country,
                                    string covantaContact,
                                    string categoryName,
                                    List<string> subCategories,
                                    string region,
                                    string supplierDiversity
            )
        {
            string filepath = string.Format(@"{0}SupplierData_{1}.pdf", Path.GetTempPath(), supplierId);
            Document doc = new Document(PageSize.A4);

            PdfWriter.GetInstance(doc, new FileStream(filepath, FileMode.Create));

            doc.Open();

            // SUPLIER FORM TABLE
            PdfPTable supplierTable = new PdfPTable(2);

            AddRow(supplierTable, "Covanta Contact", covantaContact, null);
            AddRow(supplierTable, "Company", company, null);
            AddRow(supplierTable, "Full Name", fullName, null);
            AddRow(supplierTable, "Phone", phoneNumber, null);
            AddRow(supplierTable, "Email", email, null);
            AddRow(supplierTable, "Street", street, null);
            AddRow(supplierTable, "City", city, null);
            AddRow(supplierTable, "State/Province", state, null);
            AddRow(supplierTable, "Zip/Postal Code", zipCode, null);
            AddRow(supplierTable, "Country", country, null);
            AddRow(supplierTable, "Category", categoryName, null);
            AddRow(supplierTable, "Sub Categories", string.Empty, subCategories);
            AddRow(supplierTable, "Region", region, null);
            AddRow(supplierTable, "Diverse Supplier", supplierDiversity, null);

            doc.Add(supplierTable);

            // END SUPPLIER TABLE

            doc.Close();

            return filepath;
        }

        public static void AddRow(PdfPTable table, string fieldName, string value, List<string> subCategories)
        {
            Font boldFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.BOLD);
            Font normalFont = new Font(Font.FontFamily.TIMES_ROMAN, 18, Font.NORMAL);

            PdfPCell cell1 = new PdfPCell();
            cell1.AddElement(new Phrase(fieldName, boldFont));
            cell1.HorizontalAlignment = 2;

            PdfPCell cell2 = new PdfPCell();
            if (fieldName == "Sub Categories")
            {
                foreach (string subCategory in subCategories)
                {
                    cell2.AddElement(new Phrase(subCategory, normalFont));
                }
            }
            else
            {
                cell2.AddElement(new Phrase(value, normalFont));
            }

            cell2.HorizontalAlignment = Element.ALIGN_LEFT;

            table.AddCell(cell1);
            table.AddCell(cell2);
        }

        public static bool MergePDFs(List<string> fileNames, string targetPdf)
        {
            bool merged = true;
            using (FileStream stream = new FileStream(targetPdf, FileMode.Create))
            {
                Document document = new Document();
                PdfCopy pdf = new PdfCopy(document, stream);
                PdfReader reader = null;
                try
                {
                    document.Open();
                    foreach (string file in fileNames)
                    {
                        reader = new PdfReader(file);
                        pdf.AddDocument(reader);
                        reader.Close();
                    }
                }
                catch (Exception)
                {
                    merged = false;
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                finally
                {
                    if (document != null)
                    {
                        document.Close();
                    }
                }
            }
            return merged;
        }

        public static void ConvertImageToPdf(FileUpload fileAttachments, string targetPath)
        {
            //Save the Uploaded Image file.
            string filePath = string.Format("{0}{1}", Path.GetTempPath(), Path.GetFileName(fileAttachments.PostedFile.FileName));
            fileAttachments.SaveAs(filePath);

            using (FileStream stream = new FileStream(targetPath, FileMode.Create))
            {
                //Initialize the PDF document object.
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    //Add the Image file to the PDF document object.
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(filePath);
                    img.SetAbsolutePosition(0, 0);
                    img.ScaleAbsoluteHeight(pdfDoc.PageSize.Height);
                    img.ScaleAbsoluteWidth(pdfDoc.PageSize.Width);
                    pdfDoc.Add(img);
                    pdfDoc.Close();
                }
            }

            File.Delete(filePath);
        }

        public static void ConvertWordToPdf(FileUpload fileAttachments, string targetPath)
        {
            //Save the Uploaded Image file.
            string filePath = string.Format("{0}{1}", Path.GetTempPath(), Path.GetFileName(fileAttachments.PostedFile.FileName));
            fileAttachments.SaveAs(filePath);
            //Load Document  
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(filePath);

            //Convert Word to PDF  
            document.SaveToFile(targetPath, Spire.Doc.FileFormat.PDF);

            File.Delete(filePath);
        }

        public static void RemovePdfAnnotations(string filePath)
        {
            PdfReader reader = new PdfReader(filePath);
            PdfDictionary pageDict = reader.GetPageN(1); // 1st page is 1
            pageDict.Remove(PdfName.ANNOTS);
            reader.Close();
        }
    }
}
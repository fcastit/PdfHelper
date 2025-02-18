using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Text;

namespace PdfHelper
{
    //https://itextpdf.com/en/resources/faq/technical-support/itext-5-legacy/how-decrypt-pdf-document-owner-password
    //https://itextpdf.com/en/resources/faq/technical-support/itext-5-legacy/how-convert-pdfstamper-byte-array
    public static class PdfHelperBL
    {
        public static bool Protect(string filenameSource, string ownerPassword)
        {
            return Protect(filenameSource, String.Empty, ownerPassword);
        }

        public static bool Protect(string filenameSource, string userPassword, string ownerPassword)
        {
            try
            {
                byte[] pdfProtected = EncryptPdf(filenameSource, userPassword, ownerPassword);

                string filenameDest = String.Format("{0}_PROTECTED.pdf", Path.Combine(Path.GetDirectoryName(filenameSource), Path.GetFileNameWithoutExtension(filenameSource)));

                File.WriteAllBytes(filenameDest, pdfProtected);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public static bool Unprotect(string filenameSource, string ownerPassword)
        {
            try
            {
                byte[] pdfUnprotected = DecryptPdf(filenameSource, ownerPassword);

                if (pdfUnprotected == null)
                    pdfUnprotected = DecryptPdfNew(filenameSource, ownerPassword);

                string filenameDest = String.Format("{0}_UNPROTECTED.pdf", Path.Combine(Path.GetDirectoryName(filenameSource), Path.GetFileNameWithoutExtension(filenameSource)));

                File.WriteAllBytes(filenameDest, pdfUnprotected);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private static byte[] EncryptPdf(string src, string userPassword, string ownerPassword)
        {
            PdfReader reader = new PdfReader(src);

            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    byte[] userPasswordBytes = null;
                    if (!String.IsNullOrEmpty(userPassword))
                        userPasswordBytes = Encoding.UTF8.GetBytes(userPassword);

                    stamper.SetEncryption(userPasswordBytes, Encoding.UTF8.GetBytes(ownerPassword), PdfWriter.ALLOW_PRINTING, (PdfWriter.ENCRYPTION_AES_128 | PdfWriter.DO_NOT_ENCRYPT_METADATA));

                    reader.Close();
                }
                return ms.ToArray();
            }
        }

        private static byte[] DecryptPdf(string src, string ownerPassword)
        {
            try
            {
                PdfReader.unethicalreading = true;

                PdfReader reader = null;

                byte[] ownerPasswordBytes = null;
                if (!String.IsNullOrEmpty(ownerPassword))
                    ownerPasswordBytes = Encoding.UTF8.GetBytes(ownerPassword);

                if (ownerPasswordBytes != null)
                    reader = new PdfReader(src, ownerPasswordBytes);
                else
                    reader = new PdfReader(src);

                //string userPassword = System.Text.Encoding.UTF8.GetString(reader.ComputeUserPassword());

                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfStamper stamper = new PdfStamper(reader, ms))
                    {
                        Helper.Dispose(stamper);

                        reader.Close();
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        private static byte[] DecryptPdfNew(string src, string ownerPassword)
        {
            MyReader.unethicalreading = true;

            MyReader reader = null;

            reader.decryptOnPurpose();

            byte[] ownerPasswordBytes = null;
            if (!String.IsNullOrEmpty(ownerPassword))
                ownerPasswordBytes = Encoding.UTF8.GetBytes(ownerPassword);

            if (ownerPasswordBytes != null)
                reader = new MyReader(src, ownerPasswordBytes);
            else
                reader = new MyReader(src);

            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(reader, ms))
                {
                    Helper.Dispose(stamper);

                    reader.Close();
                }
                return ms.ToArray();
            }
        }
    }

    class MyReader : PdfReader
    {
        public MyReader(string filename)
        {
            new PdfReader(filename);
        }

        public MyReader(string filename, byte[] ownerPasswordBytes)
        {
            new PdfReader(filename, ownerPasswordBytes);
        }

        public void decryptOnPurpose()
        {
            this.encrypted = false;
        }
    }
}

using iTextSharp.text.pdf;

namespace PdfHelper
{
    class MyPdfReader : PdfReader
    {
        public MyPdfReader(string filename)
        {
            new PdfReader(filename);
        }

        public MyPdfReader(string filename, byte[] ownerPasswordBytes)
        {
            new PdfReader(filename, ownerPasswordBytes);
        }

        public void decryptOnPurpose()
        {
            this.encrypted = false;
        }
    }
}
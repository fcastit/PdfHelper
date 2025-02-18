namespace PdfHelper
{
    public static class Helper
    {
        public static void Dispose<TDisposable>(TDisposable disposableObject)
            where TDisposable : class, IDisposable
        {
            if (disposableObject != null)
            {
                disposableObject.Dispose();
            }
        }
    }
}

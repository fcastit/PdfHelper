using PdfHelper;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the path of the file to protect/unprotect:");
        string filePath = Console.ReadLine();

        Console.WriteLine("Do you want to protect or unprotect the file? (P/U):");
        string action = Console.ReadLine().ToUpper();

        if (action == "P")
        {
            Console.WriteLine("Enter the owner password:");
            string ownerPassword = Console.ReadLine();

            if (PdfHelperBL.Protect(filePath, ownerPassword))
            {
                Console.WriteLine("File protected successfully.");
            }
            else
            {
                Console.WriteLine("Failed to protect the file.");
            }
        }
        else if (action == "U")
        {
            Console.WriteLine("Enter the owner password (if needed):");
            string ownerPassword = Console.ReadLine();

            if (PdfHelperBL.Unprotect(filePath, ownerPassword))
            {
                Console.WriteLine("File unprotected successfully.");
            }
            else
            {
                Console.WriteLine("Failed to unprotect the file.");
            }
        }
        else
        {
            Console.WriteLine("Invalid action. Please enter 'P' to protect or 'U' to unprotect.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

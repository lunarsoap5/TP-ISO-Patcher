namespace RandomizerPatchApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();
                Application.Run(new Form1());
            }
            else
            {
                bool patchStatus = Form1.PatchISO(args[0]);
                if (patchStatus)
                {
                    Console.WriteLine("Patching Complete!");
                }
                else
                {
                    Console.WriteLine("Patching Failed!");
                }
            }
        }
    }
}
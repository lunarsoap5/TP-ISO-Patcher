namespace RandomizerPatchApp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using System.Diagnostics;

    public partial class Form1 : Form
    {
        public static readonly GCR gcrInstance = new();

        public Form1()
        {
            InitializeComponent();
            TextBoxWriter writer = new TextBoxWriter(this.outputTBox);
            Console.SetOut(writer);
        }

        private void gCodeTbox_TextChanged(object sender, EventArgs e) { }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            bool patchStatus = PatchISO(isoTBox.Text, gameFilesTBox.Text);
            if (patchStatus)
            {
                Console.WriteLine("Patching Completed!");
            }
            else
            {
                Console.WriteLine("Patching Failed!");
            }
        }

        public static bool PatchISO(string isoPath, string gameFilesPath)
        {
            if (isoPath == "")
            {
                Console.WriteLine("No input file selected!");
                return false;
            }
            string gameCode;
            string gameRegion;
            using (FileStream fs = new FileStream(isoPath, FileMode.Open, FileAccess.Read))
            {
                byte[] codeAsBytes = new byte[6];
                var memBuffer = fs.Read(codeAsBytes, 0, codeAsBytes.Length);
                gameCode = System.Text.Encoding.Default.GetString(codeAsBytes);
                fs.Close();

                switch (gameCode)
                {
                    case "GZ2E01":
                    {
                        gameRegion = "US";
                        break;
                    }
                    case "GZ2P01":
                    {
                        gameRegion = "EU";
                        break;
                    }
                    case "GZ2J01":
                    {
                        gameRegion = "JP";
                        break;
                    }

                    default:
                    {
                        Console.WriteLine(
                            "Loaded ISO is not a Twilight Princess ISO. Please check your file and try again."
                        );
                        return false;
                    }
                }
            }
            string randoFiles = ".\\PatchFiles\\RandoFiles\\";
            Process process = new Process();
            Console.WriteLine("Patching Seeds...");
            PatchSeedFiles(randoFiles, gameRegion);
            randoFiles = ".\\PatchFiles\\RandoFiles\\" + gameRegion;
            Console.WriteLine("Done.");

            //Extract the ISO
            string tempISOPath = ".\\tempISO";
            Console.WriteLine("Extracting ISO...");

            string[] gcrArg = { "--extract", isoPath, tempISOPath };

            RandomizerPatchApp.GCR.GCRMain(gcrArg);
            if (!Directory.Exists(tempISOPath))
            {
                Console.WriteLine("ERROR! ISO Extraction failed! Please check your files");
                return false;
            }
            Console.WriteLine("Done.");
            try
            {
                //Patch the DOL with the proper Gecko Code based on the game region.
                Console.WriteLine("Patching dol with Gecko Code...");
                string[] dirs = Directory.GetFiles(
                    @randoFiles,
                    "REL_Loader_V2_" + gameRegion + "*"
                ); //get the gecko code file. They always start with REL_Loader
                if (dirs.Length == 0)
                {
                    throw new Exception(
                        "No Gecko Code file found that matches your ISO region. Check your files and try again. File should be stored in .\\PatchFiles"
                    );
                }
                ExecuteCommand(
                    ".\\bin\\GeckoLoader-Master\\GeckoLoader.exe",
                    "\""
                        + ".\\tempISO\\root\\&&systemdata\\Start.dol\" "
                        + dirs[0]
                        + " --dest "
                        + ".\"\\tempISO\\root\\&&systemdata\\Start.dol\" --optimize"
                );
                Console.WriteLine("Successfully patched dol with Gecko Code!");

                //Move all of the new game files over to the tempISO directory
                if (gameFilesPath.Length != 0)
                {
                    Console.WriteLine("Copying new game files to game folder...");
                    string[] gameFiles = Directory.GetFiles(gameFilesPath);
                    if (gameFiles.Length != 0)
                    {
                        foreach (string file in gameFiles)
                        {
                            string fileName = Path.GetFileName(file);
                            Console.WriteLine("Copied " + file + " to game folder!");
                            CopyFile(gameFilesPath, tempISOPath + "\\root", fileName);
                        }
                    }
                    string[] gameFolders = Directory.GetDirectories(gameFilesPath);
                    if (gameFolders.Length != 0)
                    {
                        foreach (string folder in gameFolders)
                        {
                            string gameFolder = folder.Substring(
                                gameFilesPath.Length,
                                (folder.Length - gameFilesPath.Length)
                            );
                            gameFolder = tempISOPath + "\\root" + gameFolder;
                            //Console.WriteLine(folder + " " + gameFolder);
                            Console.WriteLine("Copied " + folder + " to game folder!");
                            CopyDirectory(folder, gameFolder, true);
                        }
                    }
                }

                //Move all of the rando files over to the tempISO dicrectory
                string[] movableFolders = Directory.GetDirectories(randoFiles);
                if (movableFolders.Length != 0)
                {
                    Console.WriteLine("Copying Randomizer files to game folder...");
                    foreach (string folder in movableFolders)
                    {
                        string randoFolder = folder.Substring(
                            randoFiles.Length,
                            (folder.Length - randoFiles.Length)
                        );
                        randoFolder = tempISOPath + "\\root" + randoFolder;
                        //Console.WriteLine(folder + " " + gameFolder);
                        Console.WriteLine("Copied " + folder + " to game folder!");
                        CopyDirectory(folder, randoFolder, true);
                    }
                }

                //Re-pack the ISO
                Console.WriteLine("Repacking the ISO...");
                string newISOName = (
                    "Twilight-Princess-Randomizer-" + DateTime.Now.ToString("HHmmss") + ".iso"
                );

                gcrArg = new string[]
                {
                    "--rebuild",
                    tempISOPath + "\\root\\",
                    newISOName,
                    "--noGameTOC"
                };

                RandomizerPatchApp.GCR.GCRMain(gcrArg);
                byte[] isoBytes = File.ReadAllBytes(newISOName); // read in the file as an array of bytes
                if (isoBytes.Length == 0)
                {
                    throw new Exception(
                        "Failed to re-pack the ISO. Check your file structure and re-try."
                    );
                }
                Console.WriteLine("Cleaning Up...");
                System.IO.Directory.Delete(tempISOPath, true); // delete the temp ISO directory once we are done with it.
                //Delete any seed files patched into the ISO.
                string[] seedFiles = Directory.GetFiles(randoFiles + "\\mod\\seed\\");
                foreach (string file in seedFiles)
                {
                    File.Delete(file); // Delete the seed file.
                }

                return true;
            }
            catch (Exception ex)
            {
                System.IO.Directory.Delete(tempISOPath, true); // delete the temp ISO directory once we are done with it.
                //Delete any seed files patched into the ISO.
                string[] seedFiles = Directory.GetFiles(randoFiles + "\\mod\\seed\\");
                foreach (string file in seedFiles)
                {
                    File.Delete(file); // Delete the seed file.
                }

                return false;
            }
        }

        static void PatchSeedFiles(string filesPath, string gameRegion)
        {
            string[] gameFiles = Directory.GetFiles(".\\_seeds\\");
            if (gameFiles.Length == 0)
            {
                Console.WriteLine(
                    "No seeds found. Please check your files and directory. Seeds should be placed in the .\\_seeds folder"
                );
                return;
            }
            foreach (string file in gameFiles)
            {
                string fileName = Path.GetFileName(file);
                byte[] bytes = File.ReadAllBytes(file); // read in the file as an array of bytes
                if (bytes[0] == 0x47) // Seed hasn't been trimmed
                {
                    Console.WriteLine("Found Seed: " + fileName);
                    bytes = bytes.Skip(0x14A0).ToArray(); // remove the first 0x14A0 bytes of the array as those are just the GCI header
                    if (fileName.Length > 32)
                    {
                        fileName = fileName.Substring(0, 31); // File name cannot be more than 32 characters in length
                    }
                    File.WriteAllBytes(
                        filesPath + "\\" + gameRegion + "\\mod\\seed\\" + fileName,
                        bytes
                    ); // create the new seed file
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            outputTBox.SelectionStart = outputTBox.Text.Length;
            outputTBox.ScrollToCaret();
        }

        public class TextBoxWriter : TextWriter
        {
            // The control where we will write text.
            private readonly Control myControl;

            public TextBoxWriter(Control control)
            {
                this.myControl = control;
            }

            public override void Write(char value)
            {
                this.myControl.Text += value;
            }

            public override void Write(string value)
            {
                myControl.Text += value;
            }

            public override Encoding Encoding
            {
                get { return Encoding.Unicode; }
            }
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e) { }

        private void isoButton_Click(object sender, EventArgs e)
        {
            var fDialog = new OpenFileDialog
            {
                Title = "Browse",
                InitialDirectory = @".",
                Filter = "ISO files (*.iso)|*.iso",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                isoTBox.Text = fDialog.FileName;
            }
        }

        public static void ExecuteCommand(string command, string commandArgs)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = command,
                    Arguments = commandArgs,
                    CreateNoWindow = true
                }
            };
            p.Start();
            p.WaitForExit();
        }

        private static void CopyFile(string sourcePath, string targetPath, string fileName)
        {
            // Use Path class to manipulate file and directory paths.
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location:
            // Create a new target folder.
            // If the directory already exists, this method does not create a new directory.
            System.IO.Directory.CreateDirectory(targetPath);

            // To copy a file to another location and
            // overwrite the destination file if it already exists.
            System.IO.File.Copy(sourceFile, destFile, true);

            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 newForm = new Form2();
            DialogResult dialogResult = newForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                newForm.Dispose();
            }
        }

        private void gameFilesButton_Click(object sender, EventArgs e)
        {
            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                gameFilesTBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}

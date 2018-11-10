using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using LibCPK;
using KPT.XMLBuild;
using KPT.Parser;
using System.Xml;

namespace KPT
{
    struct FileLocationMeta
    {
        public string leadingPath;
        public string switchPath;
        public string subPath;
        public string fileName;

        public FileLocationMeta(string leadingPath, string switchPath, string subPath, string fileName)
        {
            this.leadingPath = leadingPath;
            this.switchPath = switchPath;
            this.subPath = subPath;
            this.fileName = fileName;
        }

    }

    class Dumper
    {
        
        // Looking to depreceate these
        private const string originalDirectory = "Original";
        private const string editableDirectory = "Editable";
        private const string rawDirectory = "Raw";
        private Tools tools;





        /// <summary>
        /// Used by recursive function PopulateFileList to signal early termination should happpen
        /// </summary>
        private bool fileListPopulateFailed = false;

        public Dumper()
        {
            tools = new Tools();
        }

        private void InitalizeDirectoryStructure(string targetDirectoryPath)
        {
            MessageBox.Show(targetDirectoryPath);
            Environment.Exit(0);
        }

        /// <summary>
        /// Check that the directory exists and it roughly lines up with is expected from the ISO dir (i.e. has a PSP_GAME directory)
        /// </summary>
        /// <param name="directory">The path of the directory to be checked</param>
        /// <returns>True if passes tests, false if fails tests</returns>
        private bool IsExpectedISODirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                string errorMessage = string.Format("Could not find directory {0}. Please check that the path is correct and that the directory exists.", directory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var initialDirectoryListing = Directory.GetDirectories(directory);
            var initialDirectoryContents = new HashSet<string>();

            foreach (var dir in initialDirectoryListing)
            {
                initialDirectoryContents.Add(Path.GetFileName(dir));
            }

            if (!initialDirectoryContents.Contains("PSP_GAME"))
            {
                string errorMessage = string.Format("Directory {0} did not match the expected format. Expected a directory containing a \"PSP_GAME\" directory.", directory); // this is to nudge people in the right path and be disabled or customized if necessary should another format come into play
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check that the directory has been initalized to fit the expected working environment of the program
        /// </summary>
        /// <param name="directory">The path of the directory to be checked</param>
        /// <returns>True if passes tests, false if fails tests</returns>
        /// <remarks>
        /// At current, the test is a very simple "does this text file exist" - this can be easily faked, but there is no real benefit to doing so
        /// </remarks>
        private bool CheckInitedDirectory(string directory)
        {
            if (!File.Exists(Path.Combine(directory, "valid.txt")))
            {
                string errorMessage = string.Format("Directory {0} is not a valid directory. Please initalize it first.", directory);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Go through a directory and split it into an "Original" directory with a copy of the original files and an "Editble" directory with processed files that can be edited by users
        /// </summary>
        /// <param name="sourceDirectoryPath">The path of the directory to be processed</param>
        /// <param name="targetDirectoryPath">The path of the directory to store the processed contents</param>
        /// <returns>True is succesfully processed, false if processing failed</returns>
        public bool ProcessDirectory(string sourceDirectoryPath)
        {
            string rootDir = sourceDirectoryPath; // not compliant with the ProjectFolder structure

            if (!CheckInitedDirectory(rootDir))
            {
                return false;
            }

            sourceDirectoryPath = Path.Combine(rootDir, ProjectFolder.extractedISODir); // now that we know it is an inited directory we start working with the ISO files

            if (!IsExpectedISODirectory(sourceDirectoryPath)) 
            {
                return false;
            }

            List<FileLocationMeta> fileList = new List<FileLocationMeta>();

            if (!PopulateFileList(sourceDirectoryPath, fileList))
            {
                return false;
            }

            string editableFilesDirectory = Path.Combine(ProjectFolder.GetRootDir(), ProjectFolder.editableGameFiesDir);

            if (!DebugSettings.SKIP_ORIGINAL_DIRECTORY_COPYING) // needs to test if dir already exists / handle exception
            {
                string targetDir = Path.Combine(rootDir, ProjectFolder.repackedGameFilesDir);
                DirectoryGuard.CheckDirectory(targetDir);
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(sourceDirectoryPath, targetDir); // i would like "the program has not crashed" progress box but i'm not sure it can be done if things are done this way. hmm. perhaps writing a recursive copying function would be best after all.
            }

            foreach (var file in fileList)
            {
                string fileExtension = Path.GetExtension(file.fileName);
                fileExtension = fileExtension.ToLower();
                switch (fileExtension)
                {
                    case ".cpk":
                        FilterCPKFile(file, sourceDirectoryPath, rootDir);
                        break;
                    default:
                        break;
                }               
            }

            return true;
        }

        private CPKBuildObject FilterCPKFile(FileLocationMeta file, string sourceDirectoryPath, string targetDirectoryPath)
        {

            var cpkFile = new CPK(new Tools()); // this function gets a bit confusings since file, cpkFile and embeddedFile are all thrown around - i will need to fix that
            var filePath = Path.Combine(sourceDirectoryPath, file.subPath, file.fileName);

            if (!cpkFile.ReadCPK(filePath, ActiveEncodings.currentEncoding))
            {
                string errorMessage = string.Format("Unknown error while attempting to open {0}.", filePath);
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // this could be replaced with a custom form that allows the user to skip all errors or a simple "errors while opening X files" after the files are done being read. though, the later option would require a small restructuring of the code.
                Environment.Exit(1);
            }

            int realFileCount = 0;

            foreach (var embeddedFile in cpkFile.FileTable)
            {
                if (embeddedFile.FileType.ToString() == "FILE")
                {
                    realFileCount += 1;
                }
            }

            if (realFileCount == 0)
            {
                string errorMessage = string.Format("CPK file {0} was empty.", filePath); // i am not sure this should be a fatal error - i will attempt to come back to it once i have a more complete picture of how the build system works and thus have a better idea of how to handle such an eventuality
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            CPKBuildObject cpkBuildInstructions = new CPKBuildObject();

            string originalFileLocation = Path.Combine(ProjectFolder.extractedISODir, file.subPath, file.fileName);
            cpkBuildInstructions.SetOriginalFileLocation(originalFileLocation);
            string targetFileLocation = Path.Combine(ProjectFolder.repackedGameFilesDir, file.subPath, file.fileName);
            cpkBuildInstructions.SetTargetFileLocation(targetFileLocation);


            if (realFileCount > 1) // if there is more than one file in the CPK we move the files within it to their own directory
            {
                string newSubDir = Path.GetFileNameWithoutExtension(file.fileName);
                string newSubPath = Path.Combine(file.subPath, newSubDir);
                file.subPath = newSubPath;
            }

            foreach (var embeddedFile in cpkFile.FileTable)
            {

                if (embeddedFile.FileType != "FILE")
                {
                    continue; // skip headers etc.
                }

                if (FileParser.IsParseable(embeddedFile.FileName.ToString())) // use this to determine whether to unpack or not, not save location
                {
                    file.switchPath = editableDirectory;
                }
                else
                {
                    file.switchPath = rawDirectory;
                }

                string targetFileAbsolutePath = Path.Combine(targetDirectoryPath, ProjectFolder.unpackedGameFilesDir, file.subPath, embeddedFile.FileName.ToString());
                DirectoryGuard.CheckDirectory(targetFileAbsolutePath);

                byte[] fileAsBytes = GrabCPKData(filePath, embeddedFile);

                if (DebugSettings.ALLOW_FILE_WRITES)
                {
                    FileStream fs = new FileStream(targetFileAbsolutePath, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write(fileAsBytes);

                    bw.Close();
                    fs.Close();
                }

                if (DebugSettings.COPY_UNPACKED_FILES)
                {
                    string secondTargetPath = Path.Combine(targetDirectoryPath, ProjectFolder.reassembledGameFilesDir, file.subPath, embeddedFile.FileName.ToString());
                    DirectoryGuard.CheckDirectory(secondTargetPath);
                    
                    FileStream fs = new FileStream(secondTargetPath, FileMode.Create);
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write(fileAsBytes);

                    bw.Close();
                    fs.Close();
                }

                string relativeFilePath = Path.Combine(file.subPath, embeddedFile.FileName.ToString());
                uint fileID = (uint)embeddedFile.ID;

                var cpkMeta = new CPKEmbeddedFileMeta();

                cpkMeta.filePath = Path.Combine(ProjectFolder.reassembledGameFilesDir, relativeFilePath);
                cpkMeta.fileName = embeddedFile.FileName.ToString();
                cpkMeta.checksumType = Checksum.MD5;
                cpkMeta.checksumValue = Checksums.GetMD5(fileAsBytes);
                cpkMeta.ID = fileID;

                cpkBuildInstructions.AddFile(fileID, cpkMeta);

            }

            cpkBuildInstructions.SerializeToDisk(Path.Combine(targetDirectoryPath, ProjectFolder.buildScriptsDir));

            return cpkBuildInstructions;

        }

        private byte[] GrabCPKData(string filePath, FileEntry fileInfo)
        {

            long offset = Convert.ToInt64(fileInfo.FileOffset);
            int size = Convert.ToInt32(fileInfo.FileSize);

            FileStream fs = new FileStream(filePath, FileMode.Open); // i dislike opening a steam a second time but my cursory reading of LibCPK shows no direct interface like this that i can see
            BinaryReader br = new BinaryReader(fs);

            byte[] rawData = tools.GetData(br, offset, size);

            br.Close();
            fs.Close();

            return rawData;
        }

        /// <summary>
        /// Iterate recursively through a given directory and its subdirectories and fill the provided list with information on all files contained within
        /// </summary>
        /// <param name="leadingPath">The directory to start from</param>
        /// <param name="fileList">The file list which should be populated</param>
        private bool PopulateFileList(string leadingPath, List<FileLocationMeta> fileList)
        {
            return PopulateFileList(leadingPath, "", fileList);
        }

        /// <summary>
        /// Iterate recursively through a given directory and its subdirectories and fill the provided list with information on all files contained within
        /// </summary>
        /// <param name="leadingPath">The directory to start from</param>
        /// <param name="subPath">Used to keep track of the current subdirectory that is being iterated though (usually set to "" initially)</param>
        /// <param name="fileList">The file list which should be populated</param>
        private bool PopulateFileList(string leadingPath, string subPath, List<FileLocationMeta> fileList)
        {

            if (fileListPopulateFailed) // try to quit as fast as possible if there has been an exception
            {
                return false;
            }

            string currentAbsolutePath = Path.Combine(leadingPath, subPath);

            try
            {               
                var subdirectories = Directory.GetDirectories(currentAbsolutePath);

                foreach (var subdirectory in subdirectories)
                {
                    string dirWithoutPath = Path.GetFileName(subdirectory);
                    string newSubPath = Path.Combine(subPath, dirWithoutPath);
                    PopulateFileList(leadingPath, newSubPath, fileList);
                }

                var currentDirectoryFiles = Directory.GetFiles(currentAbsolutePath);

                foreach (var file in currentDirectoryFiles)
                {
                    string filenameWithoutPath = Path.GetFileName(file);
                    FileLocationMeta newFileData = new FileLocationMeta();
                    newFileData.leadingPath = leadingPath;
                    newFileData.subPath = subPath;
                    newFileData.fileName = filenameWithoutPath;
                    fileList.Add(newFileData);
                }

                return true;

            }
            catch (Exception e) {

                if (fileListPopulateFailed) // don't bother showing more than one exception message
                {
                    fileListPopulateFailed = true;
                    string errorMessage = string.Format("There was an error while looking for files in {0}.\r\n\r\n{1}", currentAbsolutePath, e.Message);
                    MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return false;

            }

        }

    }
}

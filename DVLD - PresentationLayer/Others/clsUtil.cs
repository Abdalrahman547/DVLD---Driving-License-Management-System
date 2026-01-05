using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD___Driving_License_Management
{
    internal class clsUtil
    {
        // GUID => Global Unique Identifier
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString();
        }
        public static void CreateFolderIfDoesNotExists(string FolderName)
        {
            if (!Directory.Exists(FolderName))
            {
                Directory.CreateDirectory(FolderName);
            }
        }
        public static string ReplaceFileNameWithPersonID(string SourceFile, int PersonID)
        {
            string FileName = SourceFile;

            FileInfo FI = new FileInfo(FileName);
            string ext = FI.Extension;
            
            return PersonID.ToString() + ext;
        }
        public static bool CopyImageToProjectImagesFolder(ref string SourceImageFile, int PersonID)
        {
            string ImagesFolder = @"C:\DVLD - People Images\";

            CreateFolderIfDoesNotExists(ImagesFolder);

            string DestinationFile = ImagesFolder + ReplaceFileNameWithPersonID(SourceImageFile, PersonID);

            try
            {
                File.Copy(SourceImageFile, DestinationFile, true);
            }
            catch(IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK);

                return false;
            }

            SourceImageFile = DestinationFile;

            return true;

        }

    }
}


using System.IO;
namespace DroidDrawables.Helpers
{
    public static class IOHelper
    {
        /// <summary>
        /// Erase the directory specified by folderPath, first it clears
        /// the files in the directory and then goes recursively inside
        /// the sub directories
        /// </summary>
        /// <param name="folderPath">Folder which is to be erased</param>
        /// <param name="recursive">Go recursively inside sub directories for erase operation</param>
        /// <returns>true on success</returns>
        public static bool EraseDirectory(string folderPath, bool recursive)
        {
            //Safety check for directory existence.
            if (!Directory.Exists(folderPath))
                return false;

            foreach(string file in Directory.GetFiles(folderPath))
            {
                File.Delete(file);
            }

            //Iterate to sub directory only if required.
            if (recursive)
            {
                foreach (string dir in Directory.GetDirectories(folderPath))
                {
                    EraseDirectory(dir, recursive);
                }
            }
            return true;
        }
    }
}

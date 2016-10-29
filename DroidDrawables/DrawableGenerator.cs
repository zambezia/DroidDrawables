using System;
using System.IO;
using DroidDrawables.Helpers;
using DroidDrawables.Models;
using DroidDrawables.Exceptions;
using System.Threading.Tasks;
using DroidDrawables.Preferences;

namespace DroidDrawables
{
    public class DrawableGenerator
    {
        private string []m_drawableFolders = {"drawable-xxhdpi", "drawable-xhdpi", "drawable-hdpi", "drawable-mdpi", "drawable-ldpi"};
        private string[] m_mipmapFolders = { "mipmap-xxhdpi", "mipmap-xhdpi", "mipmap-hdpi", "mipmap-mdpi", "mipmap-ldpi" };
        private double[] m_drawableScaleFactor = { 
                                                     DrawableScaleFactor.Xxhdpi / DrawableScaleFactor.Xxxhdpi,
                                                     DrawableScaleFactor.Xhdpi / DrawableScaleFactor.Xxxhdpi,
                                                     DrawableScaleFactor.Hdpi / DrawableScaleFactor.Xxxhdpi,
                                                     DrawableScaleFactor.Mdpi / DrawableScaleFactor.Xxxhdpi,
                                                     DrawableScaleFactor.Ldpi / DrawableScaleFactor.Xxxhdpi
                                                 };

        Session m_session;
        ImageHelper m_imgHelper;
        MainWindow m_mainWindow;

        public DrawableGenerator(Session session, MainWindow mainWindow)
        {
            this.m_session = session;
            m_imgHelper = new ImageHelper();
            m_mainWindow = mainWindow;
        }

        public void GenerateDrawables()
        {
            if (this.m_session == null)
            {
                throw new NullSessionException(Properties.Resources.EXC_SESSION_NOT_INITIALIZED);
            }

            //Check if mipmap-xxxhdpi or drawable-xxxhdpi folder exists in 'res' folder
            if (Directory.GetDirectories(m_session.AndroidAssetPath, "*-xxxhdpi").Length == 0)
            {
                m_mainWindow.AddLogMessage(Properties.Resources.MSG_INVALID_INPUT_DIR);
                FinalizeDrawableGeneration();
                return;
            }
            
            Task task = Task.Factory.StartNew(() =>
                 {
                     GenerateDrawables("drawable-xxxhdpi", m_drawableFolders);
                     GenerateDrawables("mipmap-xxxhdpi", m_mipmapFolders);
                 });
            Task completion = task.ContinueWith(t => FinalizeDrawableGeneration());
        }

        private void FinalizeDrawableGeneration()
        {
            m_mainWindow.UpdateUIOnCompletion();
        }

        private void GenerateDrawables(string xxxhdpiFolder, string[] foldersToGenerate)
        {
            xxxhdpiFolder = Path.Combine(m_session.AndroidAssetPath, xxxhdpiFolder);
            if (!Directory.Exists(xxxhdpiFolder))
            {
                //throw new IOInputFolderNotFound(Properties.Resources.EXC_INPUT_DIR_MISSING);
                m_mainWindow.AddLogMessage(Properties.Resources.MSG_INPUT_DIR_MISSING, xxxhdpiFolder);
                return;
            }

            //All image files inside drawable-xxxhdpi folder
            string[] resPngFiles = Directory.GetFiles(xxxhdpiFolder);
            int index = 0;
            //Iterate through all PNG images in XXXHDpi drawable folder and apply necessary
            //image scaling and filter to produce ldpi, mdpi, hdpi, xhdpi and xxhdpi folders.
            foreach (var folder in foldersToGenerate)
            {
                double scaleRatio = m_drawableScaleFactor[index];
                m_mainWindow.AddLogMessage("Generating {0} - Scale Ratio: {1:F2}", folder, scaleRatio);
                string outDrawableFolder = Path.Combine(m_session.OutputPath, folder);

                if (AppSettings.ClearResourceFolder)
                    IOHelper.EraseDirectory(outDrawableFolder, true);

                //Safety check, if folder did not exist create one
                if (!Directory.Exists(outDrawableFolder))
                {
                    Directory.CreateDirectory(outDrawableFolder);
                }

                foreach (string sourceImageFile in resPngFiles)
                {
                    string destinationImageFile = Path.Combine(outDrawableFolder, Path.GetFileName(sourceImageFile));
                    string logMessage = "";
                    m_imgHelper.Resize(sourceImageFile, destinationImageFile, scaleRatio, ref logMessage);
                    m_mainWindow.AddLogMessage(logMessage);
                }
                index++;
            }
        }
    }
}

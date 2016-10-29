using System;

namespace DroidDrawables.Models
{
    public class Session
    {
        /// <summary>
        /// The 'res' directory with drawable-xxxhdpi folder, this directory should contain all drawable
        /// resources (max resolution should be 196 x 196 as suggested by android documentation).
        /// </summary>
        public string AndroidAssetPath { get; private set; }
        
        /// <summary>
        /// The output folder by default is AndroidAssetPath, all drawable resources including ldpi, mdpi, hdpi, 
        /// xhdpi and xxhdpi are created at this location.
        /// </summary>
        public string OutputPath { get; set; }

        public Session(string _andriodResPath, string _output)
        {
            AndroidAssetPath = _andriodResPath;
            OutputPath = _output;
        }
    }
}

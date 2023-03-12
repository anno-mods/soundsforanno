using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SoundsForAnno.Assetexport
{
    public class Logger
    {
        private string OutputFileName = "export.log";
        private static StreamWriter? writer; 
        public Logger() {
            writer ??= new StreamWriter(File.Create(OutputFileName));
        }

        public void Log(String message) {
            writer?.WriteLine(message);
        }
    }
}

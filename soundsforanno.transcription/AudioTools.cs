using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundsForAnno.Transcription
{
    public class AudioTools
    {
        static string bnkextr_path = "./lib/bnkextr.exe";
        static string ffmpeg_path = "./lib/ffmpeg.exe";
        static string vgmstream_path = "./lib/vgmstream/vgmstream.exe";

        /// <summary>
        /// DeepSpeech is picky af about the wave it wants. 
        /// </summary>
        /// <param name="input_file"></param>
        /// <param name="output_file"></param>
        public static async Task ReencodeWavAsync(String input_file, String output_file)
        {
            String parameters = $"-i {input_file} -y -f wav -bitexact -acodec pcm_s16le -ac 1 -ar 16000 -af \"adelay=1s:all=true\" {output_file}";

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = ffmpeg_path;
                p.StartInfo.Arguments = parameters;
                p.Start();
                await p.WaitForExitAsync();
            }
        }

        public static async Task ExtractBankAsync(String input_file)
        {
            String parameters = input_file;
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = bnkextr_path;
                p.StartInfo.Arguments = parameters;
                p.Start();
                await p.WaitForExitAsync();
            }
        }

        public static async Task ConvertWemToWavAsync(String input_wem, String output_wav)
        {
            String parameters = input_wem;
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = vgmstream_path;
                p.StartInfo.Arguments = parameters;
                p.Start();
                await p.WaitForExitAsync();
            }
            File.Move($"{input_wem}.wav", output_wav, true);
        }
    }
}

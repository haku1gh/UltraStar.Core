using System;
using UltraStar.Core;
using UltraStar.Core.Audio;

namespace TestAudioConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Recording Devices:");
            Console.WriteLine("================================================================================");
            USAudioRecordingDeviceInfo[] recordingDeviceInfo = AudioRecording.GetDevices();
            foreach (USAudioRecordingDeviceInfo deviceInfo in recordingDeviceInfo)
            {
                Console.WriteLine("Name      : " + deviceInfo.Name);
                Console.WriteLine("Type      : " + deviceInfo.Type);
                Console.WriteLine("IsDefault : " + deviceInfo.IsDefault);
                Console.WriteLine("Inputs    : " + deviceInfo.Inputs);
                string inputNames = "";
                foreach (string name in deviceInfo.InputNames)
                    inputNames += name + Environment.NewLine + "            ";
                Console.WriteLine("InputNames: " + inputNames);
                Console.WriteLine("Channels  : " + deviceInfo.Channels);
                Console.WriteLine("SampleRate: " + deviceInfo.Samplerate);
                Console.WriteLine("Volume    : " + deviceInfo.Volume);
                Console.WriteLine();
            }

            Console.WriteLine("");
            Console.WriteLine("Playback Devices:");
            Console.WriteLine("================================================================================");
            Class1.Asdsad();

            Console.ReadKey();
        }
    }
}

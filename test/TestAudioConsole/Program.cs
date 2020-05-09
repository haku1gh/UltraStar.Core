using System;
using UltraStar.Core;
using UltraStar.Core.Audio;
using UltraStar.Core.Utils;

namespace TestAudioConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Recording Devices:");
            Console.WriteLine("================================================================================");
            USAudioRecordingDeviceInfo[] recordingDeviceInfos = AudioRecording.GetDevices();
            foreach (USAudioRecordingDeviceInfo deviceInfo in recordingDeviceInfos)
            {
                Console.WriteLine("DeviceID   : " + deviceInfo.DeviceID);
                Console.WriteLine("Name       : " + deviceInfo.Name);
                Console.WriteLine("Type       : " + deviceInfo.Type);
                Console.WriteLine("IsDefault  : " + deviceInfo.IsDefault);
                Console.WriteLine("Inputs     : " + deviceInfo.Inputs);
                string inputNames = Environment.NewLine;
                if (deviceInfo.InputNames != null)
                {
                    if(deviceInfo.InputNames.Length > 0)
                        inputNames = deviceInfo.InputNames[0] + Environment.NewLine;
                    for (int i = 1; i < deviceInfo.InputNames.Length; i++)
                        inputNames += "             " + deviceInfo.InputNames[i] + Environment.NewLine;
                }
                Console.Write(    "InputNames : " + inputNames);
                Console.WriteLine("Channels   : " + deviceInfo.Channels);
                Console.WriteLine("SampleRate : " + deviceInfo.Samplerate);
                Console.WriteLine("Volume     : " + deviceInfo.Volume.ToString("F3").Replace(',', '.') + " (" + (VolumeConversion.ConvertLinearToLoudness(deviceInfo.Volume) * 100) + ")");
                Console.WriteLine();
            }

            Console.WriteLine("");
            Console.WriteLine("Playback Devices:");
            Console.WriteLine("================================================================================");
            USAudioPlaybackDeviceInfo[] playbackDeviceInfos = AudioPlayback.GetDevices();
            foreach(USAudioPlaybackDeviceInfo deviceInfo in playbackDeviceInfos)
            {
                Console.WriteLine("DeviceID        : " + deviceInfo.DeviceID);
                Console.WriteLine("Name            : " + deviceInfo.Name);
                Console.WriteLine("Type            : " + deviceInfo.Type);
                Console.WriteLine("IsDefault       : " + deviceInfo.IsDefault);
                Console.WriteLine("MinBufferLength : " + deviceInfo.MinimumBufferLength + " us");
                Console.WriteLine("Latency         : " + deviceInfo.Latency + " us");
                Console.WriteLine("Channels        : " + deviceInfo.Channels);
                Console.WriteLine("SampleRate      : " + deviceInfo.Samplerate);
                Console.WriteLine("Volume          : " + deviceInfo.Volume.ToString("F3").Replace(',', '.') + " (" + (VolumeConversion.ConvertLinearToLoudness(deviceInfo.Volume) * 100) + ")");
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}

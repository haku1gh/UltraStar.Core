using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UltraStar.Core;
using UltraStar.Core.Unmanaged.FFmpeg;

namespace TestVideoConsole
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SetupLogging();

            string url = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4";
            FFmpegVideoStreamDecoder vsd = new FFmpegVideoStreamDecoder(url);
            vsd.Seek(95.0d);
            bool result = vsd.DecodeNextFrame(out int errorCode);
            result = vsd.DecodeNextFrame(out errorCode);
            result = vsd.DecodeNextFrame(out errorCode);
            result = vsd.DecodeNextFrame(out errorCode);
            AVFrame* _pFrame = vsd.CurrentFrame;

            using (FFmpegFrameConverter ffc = new FFmpegFrameConverter(_pFrame->Width, _pFrame->Height, (AVPixelFormat)_pFrame->Format, 1f, 400, 400, FFmpegScaleMode.Bicubic, AVPixelFormat.AV_PIX_FMT_YUVJ420P, 1))
            {
                AVFrame* convertedframe = ffc.Convert(_pFrame);
                Stream wrStream = File.Create("decodedframe2.jpg");
                FFmpegImagePacketEncoder.EncodeInJPEG(wrStream, convertedframe, 6, 24);
                wrStream.Close();
            }

            vsd.Close();

            Console.ReadKey();
        }

        private static unsafe void SetupLogging()
        {
            FFmpeg.AVLogLevel = FFmpegLogLevel.Error;

            // do not convert to local function
#pragma warning disable IDE0039 // Use local function
            AvLogSetCallback logCallback = (p0, level, format, vl) =>
            {
                if (level > FFmpeg.AVLogLevel) return;

                var lineSize = 1024;
                var lineBuffer = stackalloc byte[lineSize];
                var printPrefix = 1;
                FFmpeg.AVFormatLogLine(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
                var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(line);
                Console.ResetColor();
            };
#pragma warning restore IDE0039 // Use local function

            FFmpeg.AVSetLogCallback(logCallback);
        }
    }
}

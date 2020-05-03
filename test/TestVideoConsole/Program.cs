using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UltraStar.Core;
using UltraStar.Core.Unmanaged.FFmpeg;
using UltraStar.Core.Video;
using UltraStar.Core.Utils;

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
            vsd.Seek(6.5d);
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

            sw.Restart();
            VideoDecoder videoDecoder = VideoDecoder.Open(url, UsPixelFormat.RGBA);
            Console.WriteLine("FFmpeg Video Decoder created. Property stats:");
            Console.WriteLine("=======================================================================================");
            Console.WriteLine("DecoderRunning : " + videoDecoder.DecoderRunning);
            Console.WriteLine("Items in Buffer: " + videoDecoder.ItemsCount);
            Console.WriteLine("Codec          : " + videoDecoder.CodecName);
            Console.WriteLine("CodecLongName  : " + videoDecoder.CodecLongName);
            Console.WriteLine("Width          : " + videoDecoder.Width);
            Console.WriteLine("Height         : " + videoDecoder.Height);
            Console.WriteLine("Duration       : " + videoDecoder.Duration);
            Console.WriteLine("FrameRate      : " + videoDecoder.FrameRate);
            Console.WriteLine("PixelFormat    : " + videoDecoder.PixelFormat);
            Console.WriteLine("StartTimestamp : " + videoDecoder.StartTimestamp);

            Console.WriteLine("");
            Console.WriteLine("Decoding all frames:");
            Console.WriteLine("=======================================================================================");
            int counter = 0;
            while (videoDecoder.DecoderRunning)
            {
                while (!videoDecoder.ItemsAvailable)
                {
                    System.Threading.Thread.Sleep(20);
                    if (!videoDecoder.DecoderRunning) break;
                }
                if (videoDecoder.ItemsAvailable)
                {
                    TimestampItem<byte[]> entry = videoDecoder.NextItem();
                    if (counter % 100 == 0)
                        Console.WriteLine("Frame: " + counter.ToString().PadLeft(5, '0') + ", Timestamp: " + Math.Round((double)entry.Timestamp / 1000000, 3).ToString("F3").PadLeft(8, '0').Replace(',', '.'));
                    counter++;
                }
                //if (counter == 500) break;
            }
            System.Threading.Thread.Sleep(1000);
            videoDecoder.Close();
            sw.Stop();
            Console.WriteLine("Total Time: " + Math.Round((double)sw.ElapsedMilliseconds / 1000, 3).ToString("F3").PadLeft(8, '0').Replace(',', '.') + "sec");

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

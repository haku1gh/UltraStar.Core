using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using UltraStar.Core;
using UltraStar.Core.Audio;
using UltraStar.Core.Clock;
using UltraStar.Core.Utils;
using UltraStar.Core.Video;

namespace TestGUI
{
    public partial class MainWindow : Form
    {
        private VideoDecoder videoDecoder;
        private AudioDecoder audioDecoder;
        private ImageDecoder imageDecoder;
        private AudioPlayback audioPlayback;
        private GameClock clock;
        private UsImage image;

        public MainWindow()
        {
            InitializeComponent();
            sw.Start();
            sw.Stop();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            sw.Restart();
            string url = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4";
            string url2 = "http://clips.vorwaerts-gmbh.de/big_buck_bunny.mp4";
            string url3 = "<Enter picture file here>";

            imageDecoder = ImageDecoder.Open(UsPixelFormat.BGR24, 400, 1);
            image = imageDecoder.DecodeImage(url3);
            int stride = image.Width * 3;
            int rest = stride % 4;
            if (rest != 0) stride += 4 - rest;
            imageBox.Image = new Bitmap(image.Width, image.Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0));

            videoDecoder = VideoDecoder.Open(url, UsPixelFormat.BGR24, 1280);

            audioDecoder = AudioDecoder.Open(url2);
            while (!audioDecoder.BufferFull)
            {
                System.Threading.Thread.Sleep(10);
            }
            audioPlayback = AudioPlayback.Open(audioDecoder.SampleRate, audioDecoder.Channels, audioDecoder.GetAudioPlaybackCallback());
            audioPlayback.Stop();
            clock = new GameClock(25 * 100000);
            clock.Start();
            long delay = (-clock.Elapsed) - AudioPlayback.DefaultDevice.Latency;
            audioPlayback.Start(delay);
            delay += clock.Elapsed + AudioPlayback.DefaultDevice.Latency;
            labAudioDelay.Text = ((double)delay / 1000000).ToString("F3").Replace(',', '.') + " ms";
            sw.Stop();
            labDelay.Text = ((double)sw.ElapsedTicks / 10000).ToString("F1").Replace(',', '.') + " ms";
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            videoDecoder.Close();
            audioPlayback.Close();
            audioDecoder.Close();
        }

        TimestampItem<byte[]> nextEntry;
        bool requireNewItem = true;

        static int jitterItems = 48;
        long delay = 0;
        long[] jitter = new long[jitterItems];
        int jitterPos = 0;
        long[] procTime = new long[jitterItems];
        int procTimePos = 0;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        Bitmap im;
        int frameCount = 0;
        int frameCount2 = 0;

        private void videoTimer_Tick(object sender, EventArgs e)
        {
            bool changes = true;
            sw.Restart();
            while (changes)
            {
                changes = false;
                if (!requireNewItem)
                {
                    if (clock.Elapsed >= (nextEntry.Timestamp - delay)) // Frame can now be displayed
                    {
                        frameBox.Image = im;
                        jitter[jitterPos++] = clock.Elapsed - nextEntry.Timestamp;
                        if (jitterPos == jitterItems) jitterPos = 0;
                        long avgJitter = 0;
                        foreach (long j in jitter) avgJitter += j;
                        avgJitter /= jitterItems;
                        delay += avgJitter / jitterItems;
                        labTime.Text = ((double)clock.Elapsed / 1000000).ToString("F1").Replace(',', '.') + " sec";
                        labJitter.Text = ((double)avgJitter / 1000).ToString("F1").Replace(',', '.') + " ms";
                        labBuffer.Text = (videoDecoder.ItemsCount / videoDecoder.FrameRate * 1000).ToString("F0").Replace(',', '.') + " ms | " + videoDecoder.ItemsCount.ToString() + " Fr";
                        labAudioBuffer.Text = (audioDecoder.ItemsCount * 512 * 1000 / audioDecoder.SampleRate).ToString("F0").Replace(',', '.') + " ms | " + (audioDecoder.ItemsCount * 512 / 1000).ToString() + " kSa";
                        labDelay.Text = ((double)delay / 1000).ToString("F1").Replace(',', '.') + " ms";
                        frameCount2++;
                        requireNewItem = true;
                        changes = true;
                    }
                }
                if(requireNewItem && videoDecoder.ItemsAvailable)
                {
                    nextEntry = videoDecoder.NextItem();
                    frameCount++;
                    int stride = videoDecoder.Width * 3;
                    int rest = stride % 4;
                    if (rest != 0) stride += 4 - rest;
                    im = new Bitmap(videoDecoder.Width, videoDecoder.Height, stride, PixelFormat.Format24bppRgb, Marshal.UnsafeAddrOfPinnedArrayElement(nextEntry.Item, 0));
                    requireNewItem = false;
                    changes = true;
                }
            }
            sw.Stop();
            procTime[procTimePos++] = sw.ElapsedTicks;
            if (procTimePos == jitterItems) procTimePos = 0;
            long avgProcTime = 0;
            foreach (long j in procTime) avgProcTime += j;
            avgProcTime /= jitterItems;
            labProcTime.Text = ((double)avgProcTime / 10000).ToString("F1").Replace(',', '.') + " ms";
        }
    }
}

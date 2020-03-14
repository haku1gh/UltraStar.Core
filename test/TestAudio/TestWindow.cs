using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using UltraStar.Core;
using UltraStar.Core.Audio;

namespace TestAudio
{
    public partial class TestWindow : Form
    {
        USAudioRecordingDeviceInfo[] recordingDeviceInfo;
        AudioRecording recording;

        public TestWindow()
        {
            InitializeComponent();

            //UsConfig.LibraryRootPath = "";
            butRecordingRefresh_Click(this, null);
            //
            recording = AudioRecording.Open(recordingDeviceInfo[0], 0, recordingCallback);
            recording.Start();
            System.Threading.Thread.Sleep(1000);
            recording.Pause();
            System.Threading.Thread.Sleep(1000);
            recording.Resume();
            System.Threading.Thread.Sleep(1000);
            recording.Stop();
            recording.ReInitialize();
            System.Threading.Thread.Sleep(1000);
            recording.Start();
        }

        private void recordingCallback(AudioRecording handle, float[] buffer, int length, bool paused)
        {
        }

        private void cbRecordingDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (USAudioRecordingDeviceInfo deviceInfo in recordingDeviceInfo)
            {
                if((string)cbRecordingDevices.SelectedItem == deviceInfo.Name)
                {
                    tbRecordingName.Text = deviceInfo.Name;
                    tbRecordingType.Text = deviceInfo.Type;
                    tbRecordingIsDefault.Text = (deviceInfo.IsDefault) ? "Yes" : "No";
                    tbRecordingInputs.Text = deviceInfo.Inputs.ToString();
                    string inputNames = "";
                    foreach (string name in deviceInfo.InputNames)
                        inputNames += name + Environment.NewLine;
                    tbRecordingInputNames.Text = inputNames;
                    tbRecordingChannels.Text = deviceInfo.Channels.ToString();
                    tbRecordingSampleRate.Text = deviceInfo.Samplerate.ToString();
                    tbRecordingVolume.Text = deviceInfo.Volume.ToString();
                }
            }
        }

        private void butRecordingRefresh_Click(object sender, EventArgs e)
        {
            recordingDeviceInfo = AudioRecording.GetDevices();
            cbRecordingDevices.Items.Clear();
            foreach (USAudioRecordingDeviceInfo deviceInfo in recordingDeviceInfo)
                cbRecordingDevices.Items.Add(deviceInfo.Name);
            if (cbRecordingDevices.Items.Count > 0)
                cbRecordingDevices.SelectedIndex = 0;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.Events;

namespace VoiceStreamer
{
    /// <summary>
    /// Record the voice from the microphone
    /// and convert it to the byte array
    /// </summary>
    [Serializable]
    public class VoiceRecoder
    {
        private const int SAMPLE_RATE = 16000;
        private const int MICROPHONE_INTERVAL = 1;

        public string[] devices { get; private set; }
        public bool isRecording => Microphone.IsRecording(deviceSelected);

        public string deviceSelected { get; private set; }

        [SerializeField]
        private float SendingInterval = 0.1f;

        private AudioClip audioClip;
        private int microphoneIndexHead = 0;
        private float[] microphoneBuffer = new float[SAMPLE_RATE * MICROPHONE_INTERVAL];
        private UnityAction<float[]> dataReceiver;

        public void Initialize(UnityAction<float[]> dataReceiver)
        {
            this.dataReceiver = dataReceiver;
        }

        public void Update()
        {
            if (isRecording)
            {
                UpdateRecording();
            }
        }

        public void SelectDevice(string deviceName)
        {
            deviceSelected = deviceName;
        }

        public void StartRecording()
        {
            //null guard
            if (deviceSelected == null)
            {
                deviceSelected = Microphone.devices[0];
            }

            audioClip = Microphone.Start(deviceSelected, true, MICROPHONE_INTERVAL, SAMPLE_RATE);
            microphoneIndexHead = 0;
        }

        public void StopRecording()
        {
            UpdateRecording();
            Microphone.End(deviceSelected);
        }

        private void UpdateRecording()
        {
            int position = Microphone.GetPosition(deviceSelected);
            if (
                //if no data to read...
                (position < 0)
                || (position == microphoneIndexHead)
                //if not enough data to send...
                || (
                    SAMPLE_RATE * SendingInterval
                    <= GetDataLength(microphoneBuffer.Length, microphoneIndexHead, position)
                )
            )
            {
                //...skip
                return;
            }

            //get data
            audioClip.GetData(microphoneBuffer, 0);

            //arrange to single array
            int dataLength = GetDataLength(microphoneBuffer.Length, microphoneIndexHead, position);
            float[] dataToSend = new float[dataLength];
            if (microphoneIndexHead < position)
            {
                //head to position
                Array.Copy(microphoneBuffer, microphoneIndexHead, dataToSend, 0, dataLength);
            }
            else
            {
                int length1 = microphoneBuffer.Length - microphoneIndexHead;

                //head to last
                Array.Copy(microphoneBuffer, microphoneIndexHead, dataToSend, 0, length1);

                //first to position
                Array.Copy(microphoneBuffer, 0, dataToSend, length1, position);
            }

            //update head
            microphoneIndexHead = position + 1;
            if (microphoneIndexHead >= microphoneBuffer.Length)
            {
                microphoneIndexHead -= microphoneBuffer.Length;
            }

            //pass data
            dataReceiver.Invoke(dataToSend);
        }

        private int GetDataLength(int microphoneBufferLength, int head, int position)
        {
            if (head < position)
            {
                return position - head;
            }
            else
            {
                return microphoneBufferLength - head + position;
            }
        }
    }
}

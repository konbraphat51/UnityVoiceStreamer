using UnityEngine;

namespace VoiceStreamer
{
    public class VoiceStreamerFacade : MonoBehaviour
    {
        [SerializeField]
        private VoiceRecoder voiceRecoder = new VoiceRecoder();

        [SerializeField]
        private VoiceSender voiceSender = new VoiceSender();

        public string[] devices => voiceRecoder.devices;
        public string deviceSelected => voiceRecoder.deviceSelected;
        public bool isRecording => voiceRecoder.isRecording;

        private void Awake()
        {
            voiceRecoder.Initialize(OnRecorded);
        }

        private void Start()
        {
            voiceSender.Start();
        }

        private void Update()
        {
            voiceRecoder.Update();
        }

        public void SelectDevice(string deviceName)
        {
            voiceRecoder.SelectDevice(deviceName);
        }

        public void StartRecording()
        {
            voiceRecoder.StartRecording();
        }

        public void StopRecording()
        {
            voiceRecoder.StopRecording();
        }

        public void ToggleRecording(bool on)
        {
            if (on)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        private void OnRecorded(float[] voiceData)
        {
            voiceSender.SendVoice(voiceData);
        }
    }
}

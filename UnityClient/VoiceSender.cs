using System;
using PythonConnection;
using UnityEngine;

namespace VoiceStreamer
{
    [Serializable]
    public class VoiceSender
    {
        [Serializable]
        public class VoiceData : DataClass
        {
            public float[] voiceData;
        }

        [SerializeField]
        private string dataTypeName = "Voice";

        [SerializeField]
        private PythonConnector pythonConnector;

        public void Start()
        {
            pythonConnector.StartConnection();
        }

        public void SendVoice(float[] voiceData)
        {
            VoiceData data = new VoiceData { voiceData = voiceData };

            pythonConnector.Send(dataTypeName, data);
        }
    }
}

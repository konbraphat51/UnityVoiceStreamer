using System;
using System.Collections.Generic;
using PythonConnection;

namespace VoiceStreamer
{
    public class VoiceDataDecoder : DataDecoder
    {
        protected override Dictionary<string, Type> DataToType()
        {
            //do nothing (not receiving data)
            return new Dictionary<string, Type>();
        }
    }
}

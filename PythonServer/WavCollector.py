import time
import numpy as np
import soundfile as sf
from UnityConnector import UnityConnector

SAMPLE_RATE = 16000

voice_data = []

def save_wav(buffer_list):
    filename = time.strftime("%Y%m%d-%H%M%S") + ".wav"
    sf.write(filename, np.array(buffer_list), SAMPLE_RATE)

def on_timeout():
    print("timeout")
    
def on_stopped():
    print("stopped")

connector = UnityConnector(
    on_timeout=on_timeout,
    on_stopped=on_stopped
)

def on_data_received(data_type, data):
    print("received")
    
    voice_data.extend(data["voiceData"])

    # every 5 seconds save the data
    if len(voice_data) > SAMPLE_RATE * 5:
        save_wav(voice_data)
        voice_data.clear()
        print("saved")

print("connecting...")

connector.start_listening(
    on_data_received
)

print("connected")

while(True):
    pass
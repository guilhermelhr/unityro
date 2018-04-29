using System.Text;
using UnityEngine;

public class WAVLoader
{
    public class WAVFile
    {
        public float[] leftChannel;
        public float[] rightChannel;
        public int samples;
        public int sampleRate;
        public int channels;
    }

    // convert two bytes to one float in the range -1 to 1
    private static float bytesToFloat(byte firstByte, byte secondByte) {
        // convert two bytes to one short (little endian)
        short s = (short) ((secondByte << 8) | firstByte);
        // convert to range from -1 to (just below) 1
        
        return s / (short.MaxValue + 1f);
    }

    // Returns left and right double arrays. 'right' will be null if sound is mono.
    public static WAVFile OpenWAV(byte[] wav) {
        if(!Encoding.ASCII.GetString(wav, 0, 4).Equals("RIFF")) {
            throw new System.Exception("Invalid WAV file");
        }

        WAVFile file = new WAVFile();
        file.sampleRate = System.BitConverter.ToInt32(wav, 24);
        
        // Determine if mono or stereo
        int channels = file.channels = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

        // Get past all the other sub chunks to get to the data subchunk:
        int pos = 12;   // First Subchunk ID from 12 to 16

        // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
        while(!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97)) {
            pos += 4;
            int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
            pos += 4 + chunkSize;
        }
        pos += 8;

        // Pos is now positioned to start of actual sound data.
        //int samples = (wav.Length - pos) / 2;     // 2 bytes per sample (16 bit sound mono)
        //if(channels == 2)
        //    samples /= 2;        // 4 bytes per sample (16 bit stereo)

        int bytesPerSample = System.BitConverter.ToInt16(wav, 34) / 8;
        int samples = (wav.Length - pos) / (bytesPerSample * channels);

        file.samples = samples;

        // Allocate memory (right will be null if only mono sound)
        float[] left = file.leftChannel = new float[samples];
        float[] right;
        if(channels == 2)
            right = file.rightChannel = new float[samples];
        else
            right = file.rightChannel = null;

        // Write to double array/s:
        int i = 0;
        while(pos < wav.Length) {
            if(bytesPerSample > 1) {
                left[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if(channels == 2) {
                    right[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }
            } else {//8bits per sample
                left[i] = (wav[pos] / 255f) * 2 - 1f;
                
                pos += 1;
                if(channels == 2) {
                    right[i] = (wav[pos] / 255f) * 2 - 1f;
                    pos += 1;
                }
            }

            int byteRate = System.BitConverter.ToInt32(wav, 28);
            float samplesToFade = Mathf.Clamp(byteRate * 0.25f, 1, samples); //fade for 0.25s
            //fade to avoid audio popping on loops
            if(i >= samples - samplesToFade) {
                left[i] *= ((samples - i) / samplesToFade);
            }
            if(i <= samplesToFade) {
                left[i] *= i / samplesToFade;
            }

            i++;
        }

        return file;
    }
}

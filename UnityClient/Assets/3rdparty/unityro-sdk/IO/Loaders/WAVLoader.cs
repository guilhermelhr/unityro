using System.Collections.Generic;
using System.IO;
using System;
using System.Text;
using UnityEngine;

namespace ROIO.Loaders {
    public class WAVLoader {
        public class WAVFile {
            public float[] leftChannel;
            public float[] rightChannel;
            public int samples;
            public int sampleRate;
            public int channels;
        }

        // convert two bytes to one float in the range -1 to 1
        private static float bytesToFloat(byte firstByte, byte secondByte) {
            // convert two bytes to one short (little endian)
            short s = (short) (secondByte << 8 | firstByte);
            // convert to range from -1 to (just below) 1

            return s / (short.MaxValue + 1f);
        }

        // Returns left and right double arrays. 'right' will be null if sound is mono.
        public static WAVFile OpenWAV(byte[] wav) {
            if (!Encoding.ASCII.GetString(wav, 0, 4).Equals("RIFF")) {
                throw new System.Exception("Invalid WAV file");
            }

            WAVFile file = new WAVFile();
            file.sampleRate = System.BitConverter.ToInt32(wav, 24);

            // Determine if mono or stereo
            int channels = file.channels = wav[22];     // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12;   // First Subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97)) {
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
            int samples = (wav.Length - pos) / Mathf.Max(bytesPerSample * channels, 1);

            file.samples = samples;

            // Allocate memory (right will be null if only mono sound)
            float[] left = file.leftChannel = new float[samples];
            float[] right;
            if (channels == 2)
                right = file.rightChannel = new float[samples];
            else
                right = file.rightChannel = null;

            // Write to double array/s:
            int i = 0;
            while (pos < wav.Length) {
                if (bytesPerSample > 1) {
                    left[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                    if (channels == 2) {
                        right[i] = bytesToFloat(wav[pos], wav[pos + 1]);
                        pos += 2;
                    }
                } else {//8bits per sample
                    left[i] = wav[pos] / 255f * 2 - 1f;

                    pos += 1;
                    if (channels == 2) {
                        right[i] = wav[pos] / 255f * 2 - 1f;
                        pos += 1;
                    }
                }

                int byteRate = System.BitConverter.ToInt32(wav, 28);
                float samplesToFade = Mathf.Clamp(byteRate * 0.25f, 1, samples); //fade for 0.25s
                                                                                 //fade to avoid audio popping on loops
                if (i >= samples - samplesToFade) {
                    left[i] *= (samples - i) / samplesToFade;
                }
                if (i <= samplesToFade) {
                    left[i] *= i / samplesToFade;
                }

                i++;
            }

            return file;
        }
    }

    public static class SavWav {

        const int HEADER_SIZE = 44;

        public static bool Save(string filepath, AudioClip clip) {
            // Make sure directory exists if user is saving to sub dir.
            Directory.CreateDirectory(Path.GetDirectoryName(filepath));

            using (var fileStream = CreateEmpty(filepath)) {

                ConvertAndWrite(fileStream, clip);

                WriteHeader(fileStream, clip);
            }

            return true; // TODO: return false if there's a failure saving the file
        }

        public static AudioClip TrimSilence(AudioClip clip, float min) {
            var samples = new float[clip.samples];

            clip.GetData(samples, 0);

            return TrimSilence(new List<float>(samples), min, clip.channels, clip.frequency);
        }

        public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz) {
            return TrimSilence(samples, min, channels, hz, false, false);
        }

        public static AudioClip TrimSilence(List<float> samples, float min, int channels, int hz, bool _3D, bool stream) {
            int i;

            for (i = 0; i < samples.Count; i++) {
                if (Mathf.Abs(samples[i]) > min) {
                    break;
                }
            }

            samples.RemoveRange(0, i);

            for (i = samples.Count - 1; i > 0; i--) {
                if (Mathf.Abs(samples[i]) > min) {
                    break;
                }
            }

            samples.RemoveRange(i, samples.Count - i);

            var clip = AudioClip.Create("TempClip", samples.Count, channels, hz, _3D, stream);

            clip.SetData(samples.ToArray(), 0);

            return clip;
        }

        static FileStream CreateEmpty(string filepath) {
            var fileStream = new FileStream(filepath, FileMode.Create);
            byte emptyByte = new byte();

            for (int i = 0; i < HEADER_SIZE; i++) //preparing the header
            {
                fileStream.WriteByte(emptyByte);
            }

            return fileStream;
        }

        static void ConvertAndWrite(FileStream fileStream, AudioClip clip) {

            var samples = new float[clip.samples];

            clip.GetData(samples, 0);

            Int16[] intData = new Int16[samples.Length];
            //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]

            Byte[] bytesData = new Byte[samples.Length * 2];
            //bytesData array is twice the size of
            //dataSource array because a float converted in Int16 is 2 bytes.

            int rescaleFactor = 32767; //to convert float to Int16

            for (int i = 0; i < samples.Length; i++) {
                intData[i] = (short) (samples[i] * rescaleFactor);
                Byte[] byteArr = new Byte[2];
                byteArr = BitConverter.GetBytes(intData[i]);
                byteArr.CopyTo(bytesData, i * 2);
            }

            fileStream.Write(bytesData, 0, bytesData.Length);
        }

        static void WriteHeader(FileStream fileStream, AudioClip clip) {

            var hz = clip.frequency;
            var channels = clip.channels;
            var samples = clip.samples;

            fileStream.Seek(0, SeekOrigin.Begin);

            Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            Byte[] chunkSize = BitConverter.GetBytes(fileStream.Length - 8);
            fileStream.Write(chunkSize, 0, 4);

            Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            Byte[] subChunk1 = BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            UInt16 two = 2;
            UInt16 one = 1;

            Byte[] audioFormat = BitConverter.GetBytes(one);
            fileStream.Write(audioFormat, 0, 2);

            Byte[] numChannels = BitConverter.GetBytes(channels);
            fileStream.Write(numChannels, 0, 2);

            Byte[] sampleRate = BitConverter.GetBytes(hz);
            fileStream.Write(sampleRate, 0, 4);

            Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
            fileStream.Write(byteRate, 0, 4);

            UInt16 blockAlign = (ushort) (channels * 2);
            fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bps = 16;
            Byte[] bitsPerSample = BitConverter.GetBytes(bps);
            fileStream.Write(bitsPerSample, 0, 2);

            Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(datastring, 0, 4);

            Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
            fileStream.Write(subChunk2, 0, 4);

            //		fileStream.Close();
        }
    }
}
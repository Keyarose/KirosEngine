using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX;
using SlimDX.DXGI;
using SlimDX.D3DCompiler;
using SlimDX.Windows;
using SlimDX.Direct3D11;
using SlimDX.DirectSound;
using SlimDX.Multimedia;

using Device = SlimDX.Direct3D11.Device;
using Buffer = SlimDX.Direct3D11.Buffer;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace KirosEngine.Sound
{
    /// <summary>
    /// Basic sound object
    /// </summary>
    class BaseSound : IDisposable
    {
        //TODO:exception checking, mp3 handling
        //note sound wont work in debug

        /// <summary>
        /// Abstract class that all headers derive from
        /// </summary>
        abstract class SoundHeaderType
        {

        };

        class WaveHeaderType : SoundHeaderType
        {
            //RIFF chunk
            public int chunkId;
            public int chunkSize;
            public int format;
            //fmt chunk
            public int subChunkId;
            public int subChunkSize;
            public int audioFormat;
            public int numChannels;
            public int sampleRate;
            public int bytesRate;
            public int blockAlign;
            public int bitsPerSample;
            //data chunk
            public int dataChunkId;
            public int dataSize;
        };

        class Mp3HeaderType : SoundHeaderType
        {

        };

        private DirectSound _directSound;
        private Guid _soundDriver;
        private PrimarySoundBuffer _primarySoundBuffer;
        private SecondarySoundBuffer _secondarySoundBuffer;
        private string _fileType;
        private SoundHeaderType _soundHeader;
        private byte[] _data;

        public BaseSound(Guid soundDriver)
        {
            _soundDriver = soundDriver;
        }

        public void Initialize(IntPtr window, string file)
        {
            SoundBufferDescription bufferDisc;
            SoundBufferDescription bufferDisc2;
            WaveFormat waveFormat;

            _directSound = new DirectSound(_soundDriver);
            _directSound.SetCooperativeLevel(window, CooperativeLevel.Priority);

            LoadWaveFile(file);

            waveFormat = new WaveFormat();
            waveFormat.BitsPerSample = (short)((WaveHeaderType)_soundHeader).bitsPerSample;
            waveFormat.BlockAlignment = 4;
            waveFormat.Channels = (short)((WaveHeaderType)_soundHeader).numChannels;
            waveFormat.FormatTag = WaveFormatTag.Pcm;
            waveFormat.SamplesPerSecond = ((WaveHeaderType)_soundHeader).sampleRate;
            waveFormat.AverageBytesPerSecond = waveFormat.SamplesPerSecond * waveFormat.BlockAlignment;

            bufferDisc = new SoundBufferDescription()
            {
                SizeInBytes = 8 * waveFormat.AverageBytesPerSecond,
                AlgorithmFor3D = DirectSound3DAlgorithmGuid.Default3DAlgorithm,
                Format = waveFormat,
                Flags = BufferFlags.GlobalFocus | BufferFlags.ControlVolume
            };

            _primarySoundBuffer = new PrimarySoundBuffer(_directSound, bufferDisc);

            bufferDisc2 = new SoundBufferDescription()
            {
                SizeInBytes = ((WaveHeaderType)_soundHeader).dataSize,
                AlgorithmFor3D = DirectSound3DAlgorithmGuid.Default3DAlgorithm,
                Format = waveFormat,
                Flags = BufferFlags.ControlVolume
            };

            _secondarySoundBuffer = new SecondarySoundBuffer(_directSound, bufferDisc2);

            _secondarySoundBuffer.Write<byte>(_data, 0, LockFlags.None);
            _secondarySoundBuffer.Volume = 0;
        }

        public void LoadWaveFile(string file)
        {
            try
            {
                //open a stream to the wave file
                FileStream waveFileStream = new FileStream(file, FileMode.Open);

                BinaryReader reader = new BinaryReader(waveFileStream);

                //read in the header data
                _soundHeader = new WaveHeaderType()
                {
                    chunkId = reader.ReadInt32(),
                    chunkSize = reader.ReadInt32(),
                    format = reader.ReadInt32(),
                    subChunkId = reader.ReadInt32(),
                    subChunkSize = reader.ReadInt32(),
                    audioFormat = reader.ReadInt16(),
                    numChannels = reader.ReadInt16(),
                    sampleRate = reader.ReadInt32(),
                    bytesRate = reader.ReadInt32(),
                    blockAlign = reader.ReadInt16(),
                    bitsPerSample = reader.ReadInt16(),
                    dataChunkId = reader.ReadInt32(),
                    dataSize = reader.ReadInt32()
                };

                _data = reader.ReadBytes(((WaveHeaderType)_soundHeader).dataSize);

                reader.Close();
            }
            catch(IOException ex)
            {
                if (ex is System.IO.EndOfStreamException)
                {
                    //damaged or malformed sound file
                }
                else if (ex is System.IO.FileNotFoundException)
                {
                    //file is missing
                }
                else
                {
                    //some other error
                }
            }
        }

        public void Play()
        {
            _secondarySoundBuffer.Play(0, PlayFlags.Looping);
        }

        public void Stop()
        {
            _secondarySoundBuffer.Stop();
        }

        public void Dispose()
        {
            _secondarySoundBuffer.Dispose();
            _primarySoundBuffer.Dispose();
            _directSound.Dispose();
        }
    }
}

 
/******************************************************************************
 * * PROGRAM NAME:  SDR AVE
 * CLASS:           FFT
 * VERSION:         3.x.x
 * * DESCRIPTION:   Advanced Signal Averaging Plugin for #SDR (SDRSharp)
 * Enhances visual representation of the IF spectrum.
 * * AUTHOR:        Daniel M. Kamiński
 * LOCATION:      Lublin 2026, Poland
 * * TARGET REFS:   .NET 9.0 | .NET 8.0
 * COMPATIBILITY: Designed for #SDR by Youssef Touil
 * * ------------------------------------------------------------------------
 * Copyright (c) 2026 Daniel M. Kamiński. All rights reserved.
 ******************************************************************************/

using SDRSharp.Radio;
using System;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SDRSharp.Average
{
    public unsafe class FFT : IDisposable
    {
        public int nn = 1024;

        private UnsafeBuffer _twiddlesBuffer;
        private Complex* _twiddles;

        private UnsafeBuffer _reverseBuffer;
        private int* _reverse;

        /// <summary>
        /// Name: InitBuffers
        /// Input: int n (FFT size)
        /// Output: void
        /// Description: Allocates memory for twiddle factors and bit-reversal lookup tables.
        /// </summary>
        public void InitBuffers(int n)
        {
            nn = n;
            if (_twiddlesBuffer != null) _twiddlesBuffer.Dispose();
            _twiddlesBuffer = UnsafeBuffer.Create(nn / 2, sizeof(Complex));
            _twiddles = (Complex*)_twiddlesBuffer;

            if (_reverseBuffer != null) _reverseBuffer.Dispose();
            _reverseBuffer = UnsafeBuffer.Create(nn, sizeof(int));
            _reverse = (int*)_reverseBuffer;
        }

        /// <summary>
        /// Name: PrepareFFT
        /// Input: none (uses internal nn)
        /// Output: void
        /// Description: Pre-calculates sine/cosine twiddle factors and bit-reversal indices.
        /// </summary>
        public void PrepareFFT()
        {
            for (int i = 0; i < nn / 2; i++)
            {
                double angle = -2.0 * Math.PI * i / nn;
                _twiddles[i].Real = (float)Math.Cos(angle);
                _twiddles[i].Imag = (float)Math.Sin(angle);
            }

            int stages = (int)Math.Log2(nn);
            for (int i = 0; i < nn; i++)
            {
                int j = 0;
                int temp = i;
                for (int k = 0; k < stages; k++)
                {
                    j = (j << 1) | (temp & 1);
                    temp >>= 1;
                }
                _reverse[i] = j;
            }
        }

        /// <summary>
        /// Name: Perform
        /// Input: Complex* data (pointer to shuffled complex data)
        /// Output: void
        /// Description: Core Cooley-Tukey butterfly calculation stage.
        /// </summary>
        private void Perform(Complex* data)
        {
            int n = nn;
            int stages = (int)Math.Log2(n);

            for (int stage = 1; stage <= stages; stage++)
            {
                int step = 1 << stage;
                int halfStep = step >> 1;

                for (int group = 0; group < halfStep; group++)
                {
                    Complex weight = _twiddles[group * (n / step)];

                    for (int pair = group; pair < n; pair += step)
                    {
                        int match = pair + halfStep;
                        float tr = weight.Real * data[match].Real - weight.Imag * data[match].Imag;
                        float ti = weight.Real * data[match].Imag + weight.Imag * data[match].Real;

                        data[match].Real = data[pair].Real - tr;
                        data[match].Imag = data[pair].Imag - ti;
                        data[pair].Real += tr;
                        data[pair].Imag += ti;
                    }
                }
            }
        }

        /// <summary>
        /// Name: ProcessFFT
        /// Input: Complex* dataIn (source), Complex* dataOut (destination)
        /// Output: void
        /// Description: Performs full FFT process: bit-reversal, calculation, and visual alignment.
        /// </summary>
        public void ProcessFFT(Complex* dataIn, Complex* dataOut)
        {
            // 1. Bit-reversal shuffling
            for (int i = 0; i < nn; i++)
            {
                dataOut[_reverse[i]] = dataIn[i];
            }

            // 2. Main FFT calculation
            Perform(dataOut);

            // 3 & 4 COMBINED: Swap + Flip in one loop
            // This prepares the spectrum specifically for your display requirements
            SwapAndFlip(dataOut, nn);
        }

        /// <summary>
        /// Name: SwapAndFlip
        /// Input: Complex* data, int len
        /// Output: void
        /// Description: Performs a 180-degree shift (FFT shift) to center the DC component.
        /// </summary>
        private void SwapAndFlip(Complex* data, int len)
        {
            int half = len / 2;

            // --- STEP 1: SWAP (180-degree shift) ---
            // This part handles the frequency centering.
            for (int i = 0; i < half; i++)
            {
                Complex temp = data[i];
                data[i] = data[i + half];
                data[i + half] = temp;
            }
        }

        /// <summary>
        /// Name: ReverseBuffer
        /// Input: Complex* data, int len
        /// Output: void
        /// Description: Manually reverses the order of elements in the buffer.
        /// </summary>
        public unsafe void ReverseBuffer(Complex* data, int len)
        {
            Complex* start = data;
            Complex* end = data + len - 1;
            Complex temp;

            while (start < end)
            {
                temp = *start;
                *start = *end;
                *end = temp;
                start++;
                end--;
            }
        }

        /// <summary>
        /// Name: Dispose
        /// Input: none
        /// Output: void
        /// Description: Releases unmanaged memory buffers.
        /// </summary>
        public void Dispose()
        {
            _twiddlesBuffer?.Dispose();
            _reverseBuffer?.Dispose();
        }

        ~FFT() => Dispose();
    }
}
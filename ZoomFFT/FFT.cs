using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDRSharp.Radio;

namespace SDRSharp.Average
{
    public unsafe class FFT
    {
        public int nn = 1024;

        UnsafeBuffer Multiplier_ = null;
        public Complex* Multiplier;
        UnsafeBuffer Reverse_ = null;
        public int* Reverse;       
       

        public FFT()
        {

        }

        public void InitBuffers(int n)
        {
            nn = n;
            if (Multiplier_ != null) Multiplier_.Dispose();
            Multiplier_ = UnsafeBuffer.Create(nn, sizeof(Complex));
            Multiplier = (Complex*)Multiplier_;

            if (Reverse_ != null) Reverse_.Dispose();
            Reverse_ = UnsafeBuffer.Create(nn*2, sizeof(int));
            Reverse = (int*)Reverse_;

        }

        public void PrepareFFT()
        {
           for (int Step = 1; Step < nn; Step <<= 1)
            {
                //   Angle increment
                double delta = Math.PI / Step;
                //   Auxiliary sin(delta / 2)
                double Sine = Math.Sin(delta * .5);
                //   Multiplier for trigonometric recurrence
                
                Multiplier[Step].Real = (float)(-2.0f * Sine * Sine); Multiplier[Step].Imag = (float)Math.Sin(delta);
           }


           int n, m, j, i;

           

           n = nn << 1;
           j = 1;// data[0] is not used
          
           for (i = 0; i < n; i += 2)
           {
              
               if (j > i)
                   Reverse[i / 2] = j;
               else
                   Reverse[i / 2] = 0;

               m = n >> 1;
               while (m >= 2 && j > m)
               {
                   j -= m;
                   m >>= 1;
               }
               j += m;
           }
        }

        public void CalcFFT(Complex* Data_In, Complex* Data_Out, int n)
        {
            nn = n;
            fourier(Data_In, Data_Out);

            //normalise
            float sq = (float)(1.0/Math.Sqrt(n));
            for (int i = 0; i < nn; i++)
                Data_Out[i] *= sq;
        }

        private unsafe void fourier(Complex* Data_In, Complex* Data_Out)
        {
            int j, i;

            for (i = 0; i < nn; i++)
            {
                j = Reverse[i];

                if (j > 0)
                {
                    j /= 2;
                    Data_Out[j] = Data_In[i];
                    Data_Out[i] = Data_In[j];
                }
            }

            Perform(Data_Out);

        }

        private unsafe void Perform(Complex* Data)
        {
            Complex Factor, Product;
            int Step, Group, Pair, Jump, Match;
            //   Iteration through dyads, quadruples, octads and so on...
            for (Step = 1; Step < nn; Step <<= 1)
            {
                //   Jump to the next entry of the same transform factor
                Jump = Step << 1;
                //   Start value for transform factor, fi = 0
              // Factor=1;
                Factor.Real = 1;
                Factor.Imag = 0;
                //   Iteration through groups of different transform factor
                for (Group = 0; Group < Step; ++Group)
                {
                    //   Iteration within group 
                    for (Pair = Group; Pair < nn; Pair += Jump)
                    {
                        //   Match position
                        Match = Pair + Step;
                        //   Second term of two-point transform
                        Product = Factor * Data[Match];
                        //   Transform for fi + pi
                        Data[Match] = Data[Pair] - Product;
                        //   Transform for fi
                        Data[Pair] += Product;
                    }
                    //   Successive transform factor via trigonometric recurrence
                    Factor = Multiplier[Step] * Factor + Factor;
                }
            }
        }
    }
}
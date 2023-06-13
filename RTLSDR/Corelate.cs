using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Xna.Framework.Audio;



namespace SDRSharp.PassiveRadar
{
    class ClassCorrelate
    {

        float ACCEPTED_LEVEL = 0.1f;



        //Sound
        public int _sampleRate;
        private int _duration;
        private int _rows;
        private int _offset;

        float SoundSpeed;
        public float TotalDistance;
        public float UnitScaleDistance;
        private int MicrophoneTrueRecordedBytes; //The real interesting part of bytes from microphone buffer without offset

        public ClassCorrelate()
        {
            SoundSpeed = 340.0f / 1000;//m/mili sec
            _sampleRate = 16000;

        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        //																										//
        //		Function: Correlate																				//
        //		Descryption: Auto crrelatation of the send chrp with recorded data								//																										//
        //		Return: The highest value of correlate function
        //																										//
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int Correlate(int buffer_lenght_in_, int buffer_lenght_out_,
                               ref int[] In_block16_, ref int[] Out_block16_,
                               ref int[] Corr_block_,
                                int SpeacerWarmUp_,
                               float Amplification)
        {

            int i, j, maxcor = 1;// don't devide by 0
            double rev_maxcorr;
            int sum;
            

            for (i = _offset; i <= buffer_lenght_in_; ++i)
            {// from offset because the recording is longer than the chirp
                sum = 0;
                for (j = SpeacerWarmUp_; j < buffer_lenght_out_; ++j) //the in block is bigger than max out block
                    sum += Out_block16_[j] * In_block16_[i + j]; //In_block16 is already scaled to 0 in the midle

                Corr_block_[i] = sum;
                if (Math.Abs(sum) > maxcor) maxcor = Math.Abs(sum);//find max corr value
            }


            ////////////Normalize Corelation block to max
            rev_maxcorr = Amplification / maxcor; //255 is the max but can be 32000 no problem
            for (i = _offset; i <= buffer_lenght_in_; ++i)
            { // if 16 bits then count_to is already 2 times smaller
                Corr_block_[i] = (int) Math.Abs(Corr_block_[i] * rev_maxcorr);// normalize to 255
            }

            return (int)(maxcor /5000000);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        //																										//
        //		Function: Postproccesing																		//
        //		Descryption: postproces the correlate data														//																										//
        //		Return: the real offset	value																	//																										//
        //																										//
        //////////////////////////////////////////////////////////////////////////////////////////////////////////
        public int PostProc(int buffer_lenght_in_,
                     int buffer_lenght_out_,
                     int[] CorrelationBlock,
                     ref int[] PostProc_block_,
                     float amplification,
                     int NR_START_POINTS   //The scale starts a bit later than the real recording in correlation array
                    )
        {

            int i, j, step, window_size, max_temp = 0, mark_start = 1;
            int flag = 0;

            ////////////////////////////////// fast find the beggining ///////////////////////////////////////////////
            for (i = _offset; i <= buffer_lenght_in_; ++i)
            {
                if (CorrelationBlock[i] > ACCEPTED_LEVEL * amplification)
                {                   
                    flag++;
                    if (flag > NR_START_POINTS)
                    {
                        mark_start = i;
                        break; //count to NR_START_POINTS to be sure that it is beggining and stop
                    }
                }
            }

            /////////////////////////////// unsharpening //////////////////////////////////////////////////////////////
            window_size =  MicrophoneTrueRecordedBytes / _rows; // NR_ROWS size on the screan 2 BECAUSE 16 BITS
            
            int max_j;

            for (i = 0; i < _rows; ++i)
            { 
                max_temp = 0;
                step = i * window_size + mark_start;
                max_j = window_size + step;

                if (max_j >= buffer_lenght_in_) max_j = buffer_lenght_in_;
                for (j = step; j <= max_j; ++j)
                    if ( CorrelationBlock[j] > max_temp)
                        max_temp = CorrelationBlock[j];
                PostProc_block_[i] = max_temp;
            }

            return mark_start; //return true offset
        }

    }
}

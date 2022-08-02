using System;

namespace SharpNeatLib.Maths
{
    public class FastRandom
    {
        private const double REAL_UNIT_INT = 1.0 / ((double)int.MaxValue + 1.0);
        private const double REAL_UNIT_UINT = 1.0 / ((double)uint.MaxValue + 1.0);
        private const uint Y = 842502087, Z = 3579807591, W = 273326509;

        private uint x, y, z, w;

        #region Constructors

        public FastRandom()
        {
            Reinitialise((int)Environment.TickCount);
        }

        public FastRandom(int seed)
        {
            Reinitialise(seed);
        }

        #endregion Constructors

        #region Public Methods [Reinitialisation]

        public void Reinitialise(int seed)
        {
            x = (uint)seed;
            y = Y;
            z = Z;
            w = W;
        }

        #endregion Public Methods [Reinitialisation]

        #region Public Methods [System.Random functionally equivalent methods]

        public int Next()
        {
            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

            uint rtn = w & 0x7FFFFFFF;
            if (rtn == 0x7FFFFFFF)
                return Next();
            return (int)rtn;
        }

        public int Next(int upperBound)
        {
            if (upperBound < 0)
                throw new ArgumentOutOfRangeException("upperBound", upperBound, "upperBound must be >=0");

            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;

            return (int)((REAL_UNIT_INT * (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * upperBound);
        }

        public int Next(int lowerBound, int upperBound)
        {
            if (lowerBound > upperBound)
                throw new ArgumentOutOfRangeException("upperBound", upperBound, "upperBound must be >=lowerBound");

            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;

            int range = upperBound - lowerBound;
            if (range < 0)
            {
                return lowerBound + (int)((REAL_UNIT_UINT * (double)(w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)))) * (double)((long)upperBound - (long)lowerBound));
            }

            return lowerBound + (int)((REAL_UNIT_INT * (double)(int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))))) * (double)range);
        }

        public double NextDouble()
        {
            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;

            return (REAL_UNIT_INT * (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)))));
        }

        public void NextBytes(byte[] buffer)
        {
            uint x = this.x, y = this.y, z = this.z, w = this.w;
            int i = 0;
            uint t;
            for (int bound = buffer.Length - 3; i < bound;)
            {
                t = (x ^ (x << 11));
                x = y; y = z; z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte)w;
                buffer[i++] = (byte)(w >> 8);
                buffer[i++] = (byte)(w >> 16);
                buffer[i++] = (byte)(w >> 24);
            }

            if (i < buffer.Length)
            {
                t = (x ^ (x << 11));
                x = y; y = z; z = w;
                w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                buffer[i++] = (byte)w;
                if (i < buffer.Length)
                {
                    buffer[i++] = (byte)(w >> 8);
                    if (i < buffer.Length)
                    {
                        buffer[i++] = (byte)(w >> 16);
                        if (i < buffer.Length)
                        {
                            buffer[i] = (byte)(w >> 24);
                        }
                    }
                }
            }
            this.x = x; this.y = y; this.z = z; this.w = w;
        }

        #endregion Public Methods [System.Random functionally equivalent methods]

        #region Public Methods [Methods not present on System.Random]

        public uint NextUInt()
        {
            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;
            return (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
        }

        public int NextInt()
        {
            uint t = (x ^ (x << 11));
            x = y; y = z; z = w;
            return (int)(0x7FFFFFFF & (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8))));
        }

        private uint bitBuffer;
        private uint bitMask = 1;

        public bool NextBool()
        {
            if (bitMask == 1)
            {
                uint t = (x ^ (x << 11));
                x = y; y = z; z = w;
                bitBuffer = w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));

                bitMask = 0x80000000;
                return (bitBuffer & bitMask) == 0;
            }

            return (bitBuffer & (bitMask >>= 1)) == 0;
        }

        #endregion Public Methods [Methods not present on System.Random]
    }
}
using Mono.Simd;
using System.Collections.Generic;

namespace NormandyNET.Modules.EFT
{
    internal static class EFTHelpers
    {
        internal static Vector4f _mm_shuffle_epi32(Vector4f one, int sel)
        {
            return VectorOperations.Shuffle(one, (ShuffleSel)sel);
        }

        internal static Vector4f _mm_add_ps(Vector4f one, Vector4f two)
        {
            return one + two;
        }

        internal static Vector4f _mm_mul_ps(Vector4f one, Vector4f two)
        {
            return one * two;
        }

        internal static Vector4f _mm_sub_ps(Vector4f one, Vector4f two)
        {
            return one - two;
        }

        internal static List<int> experienceTable = new List<int>()
        {
            0,
            1000,
            4017,
            8432,
            14256,
            21477,
            30023,
            39936,
            51204,
            63723,
            77563,
            92713,
            111881,
            134674,
            161139,
            191417,
            225194,
            262366,
            302484,
            301534,
            345751,
            391649,
            426190,
            440444,
            524580,
            492366,
            547896,
            609066,
            675913,
            748474,
            826786,
            910885,
            1000809,
            1096593,
            1198275,
            1309251,
            1541750,
            1669434,
            1804462,
            1946834,
            2096550,
            2253610,
            2420768,
            2598024,
            2785378,
            2982830,
            3190380,
            3408028,
            3635774,
            3873618,
            4121560,
            4379600,
            4651410,
            4936990,
            5236340,
            5549460,
            5872910,
            6235021,
            6604557,
            6991535,
            7398709,
            7828833,
            8286497,
            8780881,
            9330345,
            9953249,
            10713853,
            11749857,
            13198961,
            23198961,
            99999999
        };

        internal static int GetLevelByExperience(int experience)
        {
            var num = 0;

            for (int i = 0; i < experienceTable.Count; i++)
            {
                if (experience > experienceTable[i])
                {
                    num = i;
                }
            }

            return num;
        }

        internal static int[] playerLevels =
        {
            0, 1000, 4017, 8432, 14256, 21477, 30023, 39936, 51204, 63723,
            77563, 92713, 111881, 134674, 161139, 191417, 225194, 262366, 302484, 345751,
            391649, 440444, 492366, 547896, 609066, 675913, 748474, 826786, 910885, 1000809,
            1096593, 1198275, 1309251, 1429580, 1559321, 1698532, 1847272, 2005600, 2173575, 2351255,
            2538699, 2735966, 2946585, 3170637, 3408202, 3659361, 3924195, 4202784, 4495210, 4801553,
            5121894, 5456314, 5809667, 6182063, 6573613, 6984426, 7414613, 7864284, 8333549, 8831052,
            9360623, 9928578, 10541848, 11206300, 11946977, 12789143, 13820522, 15229487, 17206065, 19706065,
            22706065, 26206065, 30206065, 34706065, 39706065, 45206065, 51206065, 58206065, 68206065
        };

        internal static int GetLevelByExperienceNew(int experience)
        {
            int num = 0;
            for (int i = 0; i < playerLevels.Length; i++)
            {
                num = playerLevels[i];
                if (experience < num)
                {
                    return i;
                }
            }

            return playerLevels.Length;
        }
    }
}
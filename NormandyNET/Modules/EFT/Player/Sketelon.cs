using System.Collections.Generic;

namespace NormandyNET.Modules.EFT
{
    internal static class Sketelon
    {
        internal static List<Bone> upper_part = new List<Bone>
        {
            Bone.HumanNeck,
            Bone.HumanHead,
        };

        internal static List<Bone> right_arm = new List<Bone>
        {
            Bone.HumanNeck,
            Bone.HumanRCollarbone,
            Bone.HumanRUpperarm,
            Bone.HumanRForearm1,

            Bone.HumanRPalm
        };

        internal static List<Bone> left_arm = new List<Bone>
        {
            Bone.HumanNeck,
            Bone.HumanLCollarbone,
            Bone.HumanLUpperarm,
            Bone.HumanLForearm1,

            Bone.HumanLPalm,
        };

        internal static List<Bone> spine = new List<Bone>
        {
            Bone.HumanNeck,
            Bone.HumanSpine1,
            Bone.HumanSpine2,
            Bone.HumanSpine3,
            Bone.HumanPelvis
        };

        internal static List<Bone> lower_right = new List<Bone>
        {
            Bone.HumanPelvis,
            Bone.HumanRThigh1,
            Bone.HumanRThigh2,
            Bone.HumanRCalf,
            Bone.HumanRFoot,
            Bone.HumanRToe,
        };

        internal static List<Bone> lower_left = new List<Bone>
        {
            Bone.HumanPelvis,
            Bone.HumanLThigh1,
            Bone.HumanLThigh2,
            Bone.HumanLCalf,
            Bone.HumanLFoot,
            Bone.HumanLToe,
        };

        internal static List<Bone> upper_part_low_detail = new List<Bone>
        {
        };

        internal static List<Bone> right_arm_low_detail = new List<Bone>
        {
        };

        internal static List<Bone> left_arm_low_detail = new List<Bone>
        {
        };

        internal static List<Bone> spine_low_detail = new List<Bone>
        {
        };

        internal static List<Bone> lower_right_low_detail = new List<Bone> {
            Bone.HumanHead,
        };

        internal static List<Bone> lower_left_low_detail = new List<Bone> {
             Bone.HumanHead,
            Bone.HumanLFoot
        };

        internal static List<List<Bone>> skeleton = new List<List<Bone>> { upper_part, right_arm, left_arm, spine, lower_right, lower_left };

        internal static List<List<Bone>> skeleton_low_detail = new List<List<Bone>> { upper_part_low_detail, right_arm_low_detail, left_arm_low_detail, spine_low_detail, lower_right_low_detail, lower_left_low_detail };

        internal static List<Bone> bonesESPhighDetail = new List<Bone> {
            Bone.HumanHead,
            Bone.HumanLCalf,
            Bone.HumanLCollarbone,
            Bone.HumanLFoot,
            Bone.HumanLForearm1,
            Bone.HumanLPalm,
            Bone.HumanLThigh1,
            Bone.HumanLThigh2,
            Bone.HumanLToe,
            Bone.HumanLUpperarm,
            Bone.HumanNeck,
            Bone.HumanPelvis,
            Bone.HumanRCalf,
            Bone.HumanRCollarbone,
            Bone.HumanRFoot,
            Bone.HumanRForearm1,
            Bone.HumanRPalm,
            Bone.HumanRThigh1,
            Bone.HumanRThigh2,
            Bone.HumanRToe,
            Bone.HumanRUpperarm,
            Bone.HumanSpine1,
            Bone.HumanSpine2,
            Bone.HumanSpine3
        };

        internal static List<Bone> bonesESPlowDetail = new List<Bone> {
            Bone.HumanHead,
            Bone.HumanLFoot
         };
    }
}
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Forms;
using NormandyNET.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NormandyNET.Modules.EFT.UI
{
    public partial class AimBotSettings : MetroForm
    {
        public enum SelectedBones
        {
            [Display(Name = "Only Head")]
            OnlyHead,

            [Display(Name = "Only Neck")]
            OnlyNeck,

            [Display(Name = "Only Chest")]
            OnlyChest,

            [Display(Name = "Only Pelvis")]
            OnlyPelvis,

            [Display(Name = "Random Upper Parts")]
            RandomUpperParts,

            [Display(Name = "Random Lower Parts")]
            RandomLowerParts,

            [Display(Name = "Custom Pattern 1")]
            CustomPattern_1,

            [Display(Name = "Custom Pattern 2")]
            CustomPattern_2,

            [Display(Name = "Custom Pattern 3")]
            CustomPattern_3
        }

        private static List<BoneReadable> selectableCustomPatternBones = new List<BoneReadable> {
            BoneReadable.None,
            BoneReadable.Pelvis,
            BoneReadable.LThigh1,
            BoneReadable.LThigh2,
            BoneReadable.LCalf,
            BoneReadable.LFoot,
            BoneReadable.LToe,
            BoneReadable.RThigh1,
            BoneReadable.RThigh2,
            BoneReadable.RCalf,
            BoneReadable.RFoot,
            BoneReadable.RToe,
            BoneReadable.Spine1,
            BoneReadable.Spine2,
            BoneReadable.Spine3,
            BoneReadable.LCollarbone,
            BoneReadable.LUpperarm,
            BoneReadable.LForearm1,
            BoneReadable.LPalm,
            BoneReadable.RCollarbone,
            BoneReadable.RUpperarm,
            BoneReadable.RForearm1,
            BoneReadable.RPalm,
            BoneReadable.Neck,
            BoneReadable.Head
        };

        private static List<Bone> randomUpperParts = new List<Bone> {
            Bone.HumanHead,
            Bone.HumanNeck,
            Bone.HumanLUpperarm,
            Bone.HumanRUpperarm,
            Bone.HumanLCollarbone,
            Bone.HumanRCollarbone,
        };

        private static List<Bone> randomLowerParts = new List<Bone> {
            Bone.HumanPelvis,
            Bone.HumanLCalf,
            Bone.HumanRCalf,
        };

        private bool notInit = true;
        private static bool customPatternBonesOk_1 = false;
        private static bool customPatternBonesOk_2 = false;
        private static bool customPatternBonesOk_3 = false;

        public AimBotSettings()
        {
            InitializeComponent();
            MetroTheming.ApplyThemeAndStyle(ref metroStyleManager);
            ApplyFromSettings();
            notInit = false;
        }

        internal static Bone GetRandomBoneLowerPart()
        {
            var bone = randomLowerParts[ModuleEFT.radarForm.fastRandom.Next(1, randomLowerParts.Count + 1) - 1];
            return bone;
        }

        internal static Bone GetRandomBoneUpperPart()
        {
            var bone = randomUpperParts[ModuleEFT.radarForm.fastRandom.Next(1, randomUpperParts.Count + 1) - 1];
            return bone;
        }

        internal static Bone GetRandomBoneCustomPattern(SelectedBones boneAimOption, Bone selectedBone)
        {
            int offset = 0;

            if (boneAimOption == SelectedBones.CustomPattern_1)
            {
                offset = 0;
            }

            if (boneAimOption == SelectedBones.CustomPattern_2)
            {
                offset = 1;
            }

            if (boneAimOption == SelectedBones.CustomPattern_3)
            {
                offset = 2;
            }

            var boneFound = false;

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBoneIndex >= ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones.GetLength(1))
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBoneIndex = 0;
            }

            for (uint i = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBoneIndex; i < ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones.GetLength(1); i++)
            {
                var boneToSelect = (Bone)(int)ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[offset, ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBoneIndex];

                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBoneIndex++;

                if (boneToSelect != Bone.HumanNone)
                {
                    boneFound = true;
                    return boneToSelect;
                }
            }

            if (boneFound == false)
            {
                return selectedBone;
            }

            return selectedBone;
        }

        private void ApplyFromSettings()
        {
            metroToggleAimRandomizer.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Randomizer;
            metroToggleTriggerByAiming.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByAiming;
            metroToggleTriggerByTilt.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByTilt;

            metroTextBoxAngleXTrigger.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleX.ToString();
            metroTextBoxAngleYTrigger.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleY.ToString();
            metroTextBoxDistanceTrigger.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Distance.ToString();
            metroTextBoxSelectClosestTargetOverride.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.DistanceOverride.ToString();
            metroTextBoxCycleBonesDelay.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CycleBonesDelay.ToString();
            metroTextBoxPredictionForce.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.PredictionForce.ToString();
            metroTextBoxAimRandomizerStrength.Text = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.RandomizerStrength.ToString();

            metroCheckBoxDoHighlight.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Highlight;
            metroCheckBoxPrediction.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prediction;
            metroCheckBoxTargetTeammates.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TargetTeammates;
            metroCheckBoxSelectClosestTargetOverride.Checked = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestTargetOverride;

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prioritizing)
            {
                metroRadioButtonPriorityOrderOff.Checked = false;
                metroRadioButtonPriorityOrderOn.Checked = true;
            }
            else
            {
                metroRadioButtonPriorityOrderOff.Checked = true;
                metroRadioButtonPriorityOrderOn.Checked = false;
            }

            if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestByDistance)
            {
                metroRadioButtonSelectClosestTargetCrosshair.Checked = false;
                metroRadioButtonSelectClosestTargetDistance.Checked = true;
            }
            else

            {
                metroRadioButtonSelectClosestTargetCrosshair.Checked = true;
                metroRadioButtonSelectClosestTargetDistance.Checked = false;
            }

            var selectedBonesRadioButtonToFind = this.Controls.Find("metroRadioButtonSelectedBones_" + ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBones.ToString(), true).FirstOrDefault() as MetroRadioButton;
            selectedBonesRadioButtonToFind.Checked = true;

            metroComboBoxCustomPattern_1_1.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_1.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_2.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_2.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_4.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_4.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_5.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_5.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_6.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_6.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_7.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_7.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_8.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_8.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_1_9.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_1_9.DataSource = selectableCustomPatternBones;

            metroComboBoxCustomPattern_2_1.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_1.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_2.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_2.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_4.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_4.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_5.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_5.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_6.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_6.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_7.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_7.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_8.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_8.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_2_9.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_2_9.DataSource = selectableCustomPatternBones;

            metroComboBoxCustomPattern_3_1.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_1.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_2.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_2.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_3.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_3.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_4.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_4.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_5.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_5.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_6.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_6.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_7.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_7.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_8.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_8.DataSource = selectableCustomPatternBones;
            metroComboBoxCustomPattern_3_9.BindingContext = new BindingContext();
            metroComboBoxCustomPattern_3_9.DataSource = selectableCustomPatternBones;

            metroComboBoxCustomPattern_1_1.SelectedIndex = metroComboBoxCustomPattern_1_1.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 0].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_2.SelectedIndex = metroComboBoxCustomPattern_1_2.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 1].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_3.SelectedIndex = metroComboBoxCustomPattern_1_3.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 2].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_4.SelectedIndex = metroComboBoxCustomPattern_1_4.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 3].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_5.SelectedIndex = metroComboBoxCustomPattern_1_5.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 4].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_6.SelectedIndex = metroComboBoxCustomPattern_1_6.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 5].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_7.SelectedIndex = metroComboBoxCustomPattern_1_7.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 6].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_8.SelectedIndex = metroComboBoxCustomPattern_1_8.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 7].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_1_9.SelectedIndex = metroComboBoxCustomPattern_1_9.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, 8].ToString().Replace("Human", ""));

            metroComboBoxCustomPattern_2_1.SelectedIndex = metroComboBoxCustomPattern_2_1.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 0].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_2.SelectedIndex = metroComboBoxCustomPattern_2_2.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 1].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_3.SelectedIndex = metroComboBoxCustomPattern_2_3.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 2].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_4.SelectedIndex = metroComboBoxCustomPattern_2_4.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 3].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_5.SelectedIndex = metroComboBoxCustomPattern_2_5.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 4].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_6.SelectedIndex = metroComboBoxCustomPattern_2_6.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 5].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_7.SelectedIndex = metroComboBoxCustomPattern_2_7.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 6].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_8.SelectedIndex = metroComboBoxCustomPattern_2_8.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 7].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_2_9.SelectedIndex = metroComboBoxCustomPattern_2_9.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, 8].ToString().Replace("Human", ""));

            metroComboBoxCustomPattern_3_1.SelectedIndex = metroComboBoxCustomPattern_3_1.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 0].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_2.SelectedIndex = metroComboBoxCustomPattern_3_2.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 1].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_3.SelectedIndex = metroComboBoxCustomPattern_3_3.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 2].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_4.SelectedIndex = metroComboBoxCustomPattern_3_4.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 3].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_5.SelectedIndex = metroComboBoxCustomPattern_3_5.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 4].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_6.SelectedIndex = metroComboBoxCustomPattern_3_6.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 5].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_7.SelectedIndex = metroComboBoxCustomPattern_3_7.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 6].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_8.SelectedIndex = metroComboBoxCustomPattern_3_8.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 7].ToString().Replace("Human", ""));
            metroComboBoxCustomPattern_3_9.SelectedIndex = metroComboBoxCustomPattern_3_9.FindStringExact(ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, 8].ToString().Replace("Human", ""));
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
        }

        private void metroToggleAimRandomizer_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Randomizer = metroToggleAimRandomizer.Checked;
        }

        private void metroToggleTriggerByAiming_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByAiming = metroToggleTriggerByAiming.Checked;
        }

        private void metroToggleTriggerByTilt_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TriggerByTilt = metroToggleTriggerByTilt.Checked;
        }

        private void metroCheckBoxDoHighlight_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Highlight = metroCheckBoxDoHighlight.Checked;
        }

        private void metroRadioButtonSelectClosestTargetCrosshair_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestToCrosshair = metroRadioButtonSelectClosestTargetCrosshair.Checked;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestByDistance = false;
        }

        private void metroRadioButtonSelectClosestTargetDistance_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestByDistance = metroRadioButtonSelectClosestTargetDistance.Checked;
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestToCrosshair = false;
        }

        private void AimBotSettings_Load(object sender, EventArgs e)
        {
        }

        private void metroRadioButtonSelectedBones_Multi_CheckedChanged(object sender, EventArgs e)
        {
            if (notInit)
            {
                return;
            }
            if (sender is MetroRadioButton)
            {
                var name = (sender as MetroRadioButton);
                if (name.Checked)
                {
                    SelectedBones bone = (name.Text).GetValueFromName<SelectedBones>();

                    ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBones = bone;
                    ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.SelectedBonesUpdate = true;
                }
            }
        }

        private void metroComboBoxCustomPattern_Multi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (notInit)
            {
                return;
            }

            if (sender is MetroComboBox)
            {
                var name = (sender as MetroComboBox);

                var bone = (BoneReadable)Enum.Parse(typeof(BoneReadable), name.SelectedItem.ToString());

                for (int arrayIdx = 0; arrayIdx < 3; arrayIdx++)
                {
                    switch (name.Name)
                    {
                        case string a when a.Contains($"{arrayIdx + 1}_1"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 0] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_2"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 1] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_3"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 2] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_4"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 3] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_5"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 4] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_6"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 5] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_7"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 6] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_8"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 7] = bone;
                            break;

                        case string a when a.Contains($"{arrayIdx + 1}_9"):
                            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[arrayIdx, 8] = bone;
                            break;
                    }
                }
            }

            var ln = ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones.GetLength(1);
            customPatternBonesOk_1 = false;
            customPatternBonesOk_2 = false;
            customPatternBonesOk_3 = false;

            for (int i = 0; i < ln; i++)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[0, i] != BoneReadable.None)
                {
                    customPatternBonesOk_1 = true;
                }
            }

            for (int i = 0; i < ln; i++)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[1, i] != BoneReadable.None)
                {
                    customPatternBonesOk_2 = true;
                }
            }

            for (int i = 0; i < ln; i++)
            {
                if (ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CustomPatternBones[2, i] != BoneReadable.None)
                {
                    customPatternBonesOk_3 = true;
                }
            }

            if (customPatternBonesOk_1)
            {
                metroLabelCustomPattern_1.UseCustomForeColor = false;
                metroLabelCustomPattern_1.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroLabelCustomPattern_1.UseCustomForeColor = true;
                metroLabelCustomPattern_1.ForeColor = Color.Red;
            }

            if (customPatternBonesOk_2)
            {
                metroLabelCustomPattern_2.UseCustomForeColor = false;
                metroLabelCustomPattern_2.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroLabelCustomPattern_2.UseCustomForeColor = true;
                metroLabelCustomPattern_2.ForeColor = Color.Red;
            }

            if (customPatternBonesOk_3)
            {
                metroLabelCustomPattern_3.UseCustomForeColor = false;
                metroLabelCustomPattern_3.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroLabelCustomPattern_3.UseCustomForeColor = true;
                metroLabelCustomPattern_3.ForeColor = Color.Red;
            }
        }

        private void metroCheckBoxPrediction_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prediction = metroCheckBoxPrediction.Checked;
        }

        private void metroCheckBoxTargetTeammates_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.TargetTeammates = metroCheckBoxTargetTeammates.Checked;
        }

        private void metroCheckBoxSelectClosestTargetOverride_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.ClosestTargetOverride = metroCheckBoxSelectClosestTargetOverride.Checked;
        }

        private void metroTextBoxSelectClosestTargetOverride_TextChanged(object sender, EventArgs e)
        {
            var valueOk = UInt32.TryParse(metroTextBoxSelectClosestTargetOverride.Text, out uint value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.DistanceOverride = value;
                metroTextBoxSelectClosestTargetOverride.UseCustomForeColor = false;
                metroTextBoxSelectClosestTargetOverride.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxSelectClosestTargetOverride.UseCustomForeColor = true;
                metroTextBoxSelectClosestTargetOverride.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxDistanceTrigger_TextChanged(object sender, EventArgs e)
        {
            var valueOk = UInt32.TryParse(metroTextBoxDistanceTrigger.Text, out uint value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Distance = value;
                metroTextBoxDistanceTrigger.UseCustomForeColor = false;
                metroTextBoxDistanceTrigger.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxDistanceTrigger.UseCustomForeColor = true;
                metroTextBoxDistanceTrigger.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxAngleXTrigger_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Single.TryParse(metroTextBoxAngleXTrigger.Text, out float value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleX = value;
                metroTextBoxAngleXTrigger.UseCustomForeColor = false;
                metroTextBoxAngleXTrigger.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxAngleXTrigger.UseCustomForeColor = true;
                metroTextBoxAngleXTrigger.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxAngleYTrigger_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Single.TryParse(metroTextBoxAngleYTrigger.Text, out float value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.AngleY = value;
                metroTextBoxAngleYTrigger.UseCustomForeColor = false;
                metroTextBoxAngleYTrigger.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxAngleYTrigger.UseCustomForeColor = true;
                metroTextBoxAngleYTrigger.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxPredictionForce_TextChanged(object sender, EventArgs e)
        {
            var valueOk = Single.TryParse(metroTextBoxPredictionForce.Text, out float value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.PredictionForce = value;
                metroTextBoxPredictionForce.UseCustomForeColor = false;
                metroTextBoxPredictionForce.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxPredictionForce.UseCustomForeColor = true;
                metroTextBoxPredictionForce.ForeColor = Color.Red;
            }
        }

        private void metroTextBoxCycleBonesDelay_TextChanged(object sender, EventArgs e)
        {
            var valueOk = UInt32.TryParse(metroTextBoxCycleBonesDelay.Text, out uint value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.CycleBonesDelay = value;
                metroTextBoxCycleBonesDelay.UseCustomForeColor = false;
                metroTextBoxCycleBonesDelay.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxCycleBonesDelay.UseCustomForeColor = true;
                metroTextBoxCycleBonesDelay.ForeColor = Color.Red;
            }
        }

        private void metroRadioButtonPriorityOrderOff_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prioritizing = !metroRadioButtonPriorityOrderOff.Checked;
        }

        private void metroRadioButtonPriorityOrderOn_CheckedChanged(object sender, EventArgs e)
        {
            ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.Prioritizing = metroRadioButtonPriorityOrderOn.Checked;
        }

        private void metroButtonWarning_Click(object sender, EventArgs e)
        {
            string result = "Using only one bone all the time, will lead to a stat ban within a few raids." +
                   "\nUse Randomize options for long term." +
                   "\nOr use against cheaters only.";

            MetroMessageBox.Show(this, result, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void metroTextBoxAimRandomizerStrength_Click(object sender, EventArgs e)
        {
            var valueOk = Single.TryParse(metroTextBoxAimRandomizerStrength.Text, out float value);

            if (valueOk)
            {
                ModuleEFT.settingsForm.settingsJson.MemoryWriting.AimBot.RandomizerStrength = value;
                metroTextBoxAimRandomizerStrength.UseCustomForeColor = false;
                metroTextBoxAimRandomizerStrength.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                metroTextBoxAimRandomizerStrength.UseCustomForeColor = true;
                metroTextBoxAimRandomizerStrength.ForeColor = Color.Red;
            }
        }

        private void metroButtonBonesReference_Click(object sender, EventArgs e)
        {
            var referenceForm = new AimBotReferences();
            referenceForm.Show();
        }
    }
}
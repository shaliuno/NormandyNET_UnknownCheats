using System;
using System.Drawing;
using System.Text;

namespace NormandyNET.Core
{
    internal class OSD
    {
        #region Internal Fields

        internal StringBuilder osdText = new StringBuilder();
        internal RenderItem renderItem;

        #endregion Internal Fields

        #region Private Fields

        private int FramesPerSecond;
        private int FramesPerSecondPrev;
        private DateTime FramesPerSecondTime = CommonHelpers.dateTimeHolder;

        #endregion Private Fields

        #region Internal Methods

        internal virtual bool AddDateTime()
        {
            return false;
        }

        internal virtual bool AddFPS()
        {
            return false;
        }

        internal virtual void AddOSD()
        { }

        internal virtual void AddPostText()
        { }

        internal virtual void AddPreText()
        { }

        internal virtual bool DisplayStats()
        {
            return true;
        }

        internal void RenderOSD()
        {
            if (DisplayStats() == false)
            {
                return;
            }

            FramesPerSecond++;
            osdText.Clear();

            AddPreText();

            if (AddFPS())
            {
                osdText.Append("FPS: " + FramesPerSecondPrev).AppendLine();
            }

            if (AddDateTime())
            {
                osdText.Append(CommonHelpers.dateTimeHolder.ToString("HH:mm:ss f")).AppendLine();
            }

            osdText.Append("Tickrate R/W : ");

            if (Memory.RPMCountPrev < 100)
            {
                osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}{Memory.RPMCountPrev}");
            }
            else
            {
                osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Lime)}{Memory.RPMCountPrev}");
            }

            if (Memory.WPMCountPrev > 0)
            {
                osdText.Append($" /©{CommonHelpers.ColorHexConverter(Color.Yellow)} {Memory.WPMCountPrev}");
            }

            if (Memory.InvalidAddressCount > 0)
            {
                osdText.AppendLine();
                osdText.Append("Errors : ");

                osdText.Append($" /©{CommonHelpers.ColorHexConverter(Color.Red)} {Memory.InvalidAddressCount}");
            }

            osdText.Append("").AppendLine();

            if (CommonHelpers.dateTimeHolder > FramesPerSecondTime)
            {
                FramesPerSecondPrev = FramesPerSecond;
                FramesPerSecondTime = CommonHelpers.dateTimeHolder.AddMilliseconds(1000);
                FramesPerSecond = 0;
                Memory.RPMCountPrev = Memory.RPMCount;
                Memory.RPMCount = 0;
                Memory.WPMCountPrev = Memory.WPMCount;
                Memory.WPMCount = 0;
                if (Memory.stopOnError == false)
                {
                    Memory.InvalidAddressCount = 0;
                }
            }

            if (Memory.stopOnError == true && Memory.InvalidAddressCount > 0)
            {
                osdText.Append($"©{CommonHelpers.ColorHexConverter(Color.Red)}RADAR STOPPED!").AppendLine();
            }

            
            AddPostText();
            AddOSD();
            RenderOSDExtras();
        }

        internal virtual void RenderOSDExtras()
        { }

        #endregion Internal Methods
    }
}
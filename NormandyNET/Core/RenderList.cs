using System;
using System.Collections.Generic;
using System.Drawing;

namespace NormandyNET
{
    public class RenderItem : IComparable<RenderItem>
    {
        public Color DrawColor;

        public IconPositionTexture IconPositionTexture;

        public float MapPosX;

        public float MapPosXend;

        public float MapPosZ;

        public float MapPosZend;

        public float Rotation;

        public float RotationGPS;

        public int Size;
        public float IconSize = 1f;

        public string Text;

        public bool CenterText;
        public bool Aggregate;

        public bool TextGlyphs;

        public bool TextOverlayOutline;

        public RenderLayers renderLayer;

        public List<TextNew> textNew = new List<TextNew>();

        public class TextNew
        {
            public string text;
            public int count;
        }

        public RenderItem()
        {
            renderLayer = RenderLayers.Default;
        }

        public int CompareTo(RenderItem other)
        {
            if (renderLayer < other.renderLayer)
            {
                return -1;
            }
            else if (renderLayer > other.renderLayer)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        internal void DoAggregationParse()
        {
            Text = "";

            textNew.Sort((p1, p2) => p1.text.CompareTo(p2.text));

            for (int i = 0; i < textNew.Count; i++)
            {
                Text += textNew[i].text + " x" + textNew[i].count + "\n";
            }
        }
    }
}
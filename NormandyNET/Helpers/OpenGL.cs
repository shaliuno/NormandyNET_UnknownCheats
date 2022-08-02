using Cyotek.Drawing.BitmapFont;
using NormandyNET.Core;
using NormandyNET.Helpers;
using NormandyNET.Settings;
using NormandyNET.UI;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NormandyNET
{
    internal class TextColored
    {
        private string text;
        private Color color;
    }

    public static class OpenGL
    {
        internal static Character _currentCharacter;
        internal static int _currentPage;
        internal static BitmapFont _font;
        internal static Dictionary<int, Image> _textures;

        public static float CanvasSize = 1024;
        public static float CanvasSizeBase = 1024;
        public static float CanvasDiffCoeff = 1f;
        public static float ZoomLevel = 1f;
        internal static bool DrawText;

        private static int TextureMap;
        internal static int openglMapTextureIcons;
        internal static int openglMapTextureFonts;
        private static bool skipMapDraw;
        internal static int fontAntiAlias = 1;
        internal static float TextScale = 1f;
        internal static float IconSizePlayers = 1f;
        internal static float IconSizeLoot = 1f;
        internal static bool CenterMap;

        public static List<RenderItem> OverlayText { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OpenglOverlayIcons { get; set; } = new List<RenderItem>();
        public static List<RenderItem> OverlayGeometry { get; set; } = new List<RenderItem>();

        public static List<RenderItem> MapText { get; set; } = new List<RenderItem>();
        public static List<RenderItem> MapIcons { get; set; } = new List<RenderItem>();
        public static List<RenderItem> MapGeometry { get; set; } = new List<RenderItem>();

        internal static void CheckErrors()
        {
            if (DebugClass.DebugOpenGL == false)
            {
                return;
            }

            ErrorCode err;

            err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                throw new Exception($"OpenGL GetError exception - {err}");
            }
        }

        public static void GlControl_DrawDotCircle(int size, float coord_X, float coord_Y, Color clr)
        {
            var radius = (float)size;
            GL.Enable(EnableCap.Blend);
            GL.Color4(clr);
            GL.PointSize(1);
            GL.Begin(BeginMode.Points);

            for (int i = 0; i < 360; i++)
            {
                if (CommonHelpers.IsOdd(i))
                {
                    var degInRad = i * 3.1416 / 180;
                    GL.Vertex2((Math.Cos(degInRad) * radius) + coord_X, (Math.Sin(degInRad) * radius) + coord_Y);
                }
            }

            GL.Color4(Color.Transparent);
            GL.Disable(EnableCap.Blend);
            GL.End();

            CheckErrors();
        }

        public static void LoadTexture(string textureName)
        {
            GL.DeleteTexture(TextureMap);
            Utilities.TexLib.AddTexture($@"{MapManager.dataFolder}\map_" + textureName + ".jpg", TextureMap, 0);
            CheckErrors();
        }

        public static void LoadTexture(byte[] textureBytes)
        {
            GL.DeleteTexture(TextureMap);

            using (var ms = new MemoryStream(textureBytes))
            {
                Utilities.TexLib.AddTexture(ms, TextureMap, 0);
            }

            CheckErrors();
        }

        public static void GlControl_LoadTextures()
        {
            Utilities.TexLib.LoadTexture($@"{MapManager.dataFolder}\map_---.jpg", out TextureMap, 0);
            Utilities.TexLib.LoadTexture($@"{MapManager.dataFolder}\icons.png", out openglMapTextureIcons, 1);

            CheckErrors();
        }

        public static void GlControl_GenerateFonts()
        {
        }

        internal static void LoadFontTextures()
        {
            _textures.Clear();

            for (int i = 0; i < _font.Pages.Length; i++)
            {
                _textures.Add(_font.Pages[i].Id, Image.FromFile(_font.Pages[i].FileName));
            }

            CheckErrors();
        }

        internal static void LoadFont()
        {
            OpenGL._font = BitmapFontLoader.LoadFontFromFile(@".\Fonts\Ubuntu.fnt");
            _textures = new Dictionary<int, Image>();
            LoadFontTextures();
            Utilities.TexLib.LoadTexture(_font.Pages[0].FileName, out OpenGL.openglMapTextureFonts, fontAntiAlias);

            CheckErrors();
        }

        internal static void DrawMapIconsNew(List<RenderItem> mapIcons)
        {
            List<RenderItem> nonRotatedIcons;
            List<RenderItem> rotatedIcons;

            lock (mapIcons)
            {
                nonRotatedIcons = mapIcons.FindAll(x => x.Rotation == 0);
                rotatedIcons = mapIcons.FindAll(x => x.Rotation != 0);
            }

            if (nonRotatedIcons == null || rotatedIcons == null)
            {
                return;
            }

            if (nonRotatedIcons.Count > 0)
            {
                GL.PushMatrix();
                CheckErrors();

                GL.Begin(BeginMode.Quads);

                for (int i = 0; i < nonRotatedIcons.Count; i++)
                {
                    var clr = nonRotatedIcons[i].DrawColor;
                    var coord_X = nonRotatedIcons[i].MapPosX;
                    var coord_Y = nonRotatedIcons[i].MapPosZ;
                    var iconsSize = nonRotatedIcons[i].IconSize;

                    GlControl_DrawIconsLoot(nonRotatedIcons[i].IconPositionTexture, nonRotatedIcons[i].MapPosX, nonRotatedIcons[i].MapPosZ, nonRotatedIcons[i].DrawColor, nonRotatedIcons[i].IconSize);
                }

                GL.End();
                GL.PopMatrix();
                CheckErrors();
            }

            if (rotatedIcons.Count > 0)
            {
                for (int i = 0; i < rotatedIcons.Count; i++)
                {
                    OpenGL.GlControl_DrawIcons(rotatedIcons[i].IconPositionTexture, rotatedIcons[i].MapPosX, rotatedIcons[i].MapPosZ, rotatedIcons[i].DrawColor, rotatedIcons[i].Rotation, rotatedIcons[i].IconSize);
                }
            }
        }

        public static void GlControl_DrawIcons(IconPositionTexture icon, float coord_X, float coord_Y, Color clr, float rotation, float iconsSize)
        {
            GL.PushMatrix();

            GL.Color4(clr);
            GL.Translate(coord_X, coord_Y, 0);

            GL.Rotate(rotation, Vector3.UnitZ);
            GL.Scale(iconsSize, iconsSize, iconsSize);

            GL.Begin(BeginMode.Quads);

            var icon_pos_row = 0;
            var icon_pos_column = 0;
            int icon_pos = (int)icon;

            if (icon_pos >= 1 && icon_pos <= 4)
            {
                icon_pos_column = icon_pos - 1;
                icon_pos_row = 0;
            }

            if (icon_pos >= 5 && icon_pos <= 8)
            {
                icon_pos_column = icon_pos - 5;
                icon_pos_row = 1;
            }

            if (icon_pos >= 9 && icon_pos <= 12)
            {
                icon_pos_column = icon_pos - 9;
                icon_pos_row = 2;
            }

            if (icon_pos >= 13 && icon_pos <= 16)
            {
                icon_pos_column = icon_pos - 13;

                icon_pos_row = 3;
            }

            var iconTextureSize = 260f;

            var iconSize = 65f;
            var iconSizeVertex = iconSize / 2;

            var iconPixelCoef = iconSize / iconTextureSize;

            var icon_offset_x_start = iconPixelCoef * icon_pos_column;
            var icon_offset_y_start = iconPixelCoef * icon_pos_row;

            var icon_offset_x_end = (iconPixelCoef * icon_pos_column) + iconPixelCoef;
            var icon_offset_y_end = (iconPixelCoef * icon_pos_row) + iconPixelCoef;


            GL.TexCoord2(icon_offset_x_start, icon_offset_y_start); GL.Vertex2(-iconSizeVertex, iconSizeVertex);
            GL.TexCoord2(icon_offset_x_start, icon_offset_y_end); GL.Vertex2(-iconSizeVertex, -iconSizeVertex);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_end); GL.Vertex2(iconSizeVertex, -iconSizeVertex);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_start); GL.Vertex2(iconSizeVertex, iconSizeVertex);
            GL.End();

            GL.PopMatrix();

            CheckErrors();
        }

        public static void GlControl_DrawIconsLoot(IconPositionTexture icon, float coord_X, float coord_Y, Color clr, float iconsSize)
        {
            GL.Color4(clr);

            var icon_pos_row = 0;
            var icon_pos_column = 0;
            int icon_pos = (int)icon;

            if (icon_pos >= 1 && icon_pos <= 4)
            {
                icon_pos_column = icon_pos - 1;
                icon_pos_row = 0;
            }

            if (icon_pos >= 5 && icon_pos <= 8)
            {
                icon_pos_column = icon_pos - 5;
                icon_pos_row = 1;
            }

            if (icon_pos >= 9 && icon_pos <= 12)
            {
                icon_pos_column = icon_pos - 9;
                icon_pos_row = 2;
            }

            if (icon_pos >= 13 && icon_pos <= 16)
            {
                icon_pos_column = icon_pos - 13;

                icon_pos_row = 3;
            }

            var iconTextureSize = 260f;

            var iconSize = 65f;
            var iconSizeVertex = iconSize / 2 * iconsSize;

            var iconPixelCoef = iconSize / iconTextureSize;

            var icon_offset_x_start = iconPixelCoef * icon_pos_column;
            var icon_offset_y_start = iconPixelCoef * icon_pos_row;

            var icon_offset_x_end = (iconPixelCoef * icon_pos_column) + iconPixelCoef;
            var icon_offset_y_end = (iconPixelCoef * icon_pos_row) + iconPixelCoef;

            GL.TexCoord2(icon_offset_x_start, icon_offset_y_start);
            GL.Vertex2(-iconSizeVertex + coord_X, iconSizeVertex + coord_Y);
            GL.TexCoord2(icon_offset_x_start, icon_offset_y_end);
            GL.Vertex2(-iconSizeVertex + coord_X, -iconSizeVertex + coord_Y);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_end);
            GL.Vertex2(iconSizeVertex + coord_X, -iconSizeVertex + coord_Y);
            GL.TexCoord2(icon_offset_x_end, icon_offset_y_start);
            GL.Vertex2(iconSizeVertex + coord_X, iconSizeVertex + coord_Y);

            CheckErrors();
        }

        public static void DrawMap()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, TextureMap);

            GL.PushMatrix();

            GL.Begin(BeginMode.Quads);

            GL.TexCoord2(0f, 0f);
            GL.Vertex2((-CanvasSize), (CanvasSize));

            GL.TexCoord2(0f, 1f);
            GL.Vertex2((-CanvasSize), (-CanvasSize));

            GL.TexCoord2(1f, 1f);
            GL.Vertex2((CanvasSize), (-CanvasSize));

            GL.TexCoord2(1f, 0f);
            GL.Vertex2((CanvasSize), (CanvasSize));

            GL.End();
            GL.PopMatrix();
            GL.Disable(EnableCap.Texture2D);

            CheckErrors();
        }

        public static void SetupViewport(int width, int height)
        {
            GL.Viewport(0, 0, width, height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            var projection = Matrix4.CreateOrthographic(width, height, -1f, 1f);
            GL.LoadMatrix(ref projection);

            CheckErrors();
        }

        public static void Invalidate(GLControl glcontrol)
        {
            glcontrol.Invalidate();

            CheckErrors();
        }

        public static void MakeCurrent(GLControl glcontrol)
        {
            glcontrol.MakeCurrent();

            CheckErrors();
        }

        public static void DrawStart(bool overlay = false)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);

            GL.LoadIdentity();

            GL.Translate(RadarForm.mapDragOffsetX, RadarForm.mapDragOffsetZ, 0);

            CheckErrors();
        }

        public static void DrawEnd(GLControl glcontrol)
        {
            GL.Flush();

            if (glcontrol != null)
            {
                glcontrol.SwapBuffers();
            }

            CheckErrors();
        }

        public static void GlControl_DrawPoint(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.PushMatrix();
            GL.PointSize(size);
            GL.Begin(BeginMode.Points);
            GL.Color4(clr);
            GL.Vertex2(coord_X, coord_Y);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        internal static void DrawBitmapTextNew(List<RenderItem> mapText)
        {
            GL.PushMatrix();
            GL.LoadIdentity();
            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < mapText.Count; i++)
            {
                if (mapText[i].renderLayer != RenderLayers.OSD)
                {
                    continue;
                }

                DrawBitmapTextTest(mapText[i]);
            }

            GL.End();
            GL.PopMatrix();
            CheckErrors();

            GL.PushMatrix();
            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < mapText.Count; i++)
            {
                if (mapText[i].renderLayer == RenderLayers.OSD)
                {
                    continue;
                }

                if (mapText[i].Aggregate)
                {
                    mapText[i].DoAggregationParse();
                }

                if (OpenGL.DrawText == true && mapText[i].Text != null)
                {
                    DrawBitmapTextTest(mapText[i]);
                }
            }

            GL.End();
            GL.PopMatrix();
            CheckErrors();
        }

        private static void DrawBitmapTextTest(RenderItem renderItem)
        {
            if (renderItem.Text == null)
            {
                return;
            }

            var coord_X = 0f;
            var coord_Y = 0f;

            var clr = renderItem.DrawColor;
            var locX = renderItem.MapPosX;
            var locY = renderItem.MapPosZ;
            var scale = OpenGL.TextScale;
            var size = renderItem.Size;
            var text = renderItem.Text;
            var center = renderItem.CenterText;

            var centeredCoord = _font.MeasureFont(text);

            if (center)
            {
                coord_X -= centeredCoord.Width / 2;
            }

            GL.Color3(clr);

            var textScale = (float)size / Math.Abs(_font.FontSize) * scale;

            char previousCharacter;
            previousCharacter = '\n';

            Character data;
            Character dataPrev;

            dataPrev = _font[previousCharacter];

            var pixelCoeff = 1 / (float)_textures[0].Width;

            for (int i = 0; i < text.Length; i++)
            {
                data = _font[text[i]];
                dataPrev = _font[previousCharacter];

                switch (text[i])
                {
                    case '\n':
                        coord_Y -= _font.LineHeight;

                        if (coord_X % 2 != 0)
                        {
                            coord_X -= 1f;
                        }

                        coord_X = 0;
                        previousCharacter = text[i];

                        break;

                    case '©':
                        var newColor = System.Drawing.ColorTranslator.FromHtml(text.Substring(i + 1, 9));


                        if (i == 0)
                        {
                            previousCharacter = '\n';
                        }
                        else
                        {
                            previousCharacter = text[i - 1];
                        }

                        i += 9;
                        GL.Color3(newColor);

                        break;

                    case '\r':
                        previousCharacter = text[i];

                        break;

                    default:

                        if (!data.IsEmpty)
                        {
                            int kerning;
                            kerning = _font.GetKerning(previousCharacter, text[i]);

                            var iconSizeVertex_X = data.Width / 2;
                            var iconSizeVertex_Y = data.Height / 2;

                            var globalCaretMoveX = 0f;
                            var globalCaretMoveY = 0f;

                            var movePrevChar = (float)Math.Ceiling(dataPrev.XAdvance / 2f);

                            globalCaretMoveX += movePrevChar + kerning;

                            var off = DateTime.UtcNow.Second;

                            var moveCurrChar = (float)Math.Ceiling(data.XAdvance / 2f);

                            globalCaretMoveX += moveCurrChar + kerning;

                            var offsetY = 0f;

                            if (fontAntiAlias == 1)
                            {
                                offsetY = _font.LineHeight / 2f - (float)Math.Round(data.Height / 2f, 0) - data.YOffset;
                            }
                            else
                            {
                                offsetY = _font.LineHeight / 2f - (float)Math.Floor(data.Height / 2f) - data.YOffset;
                            }

                            globalCaretMoveY += offsetY;

                            var glyphOffsetX = pixelCoeff * data.X;
                            var glyphOffsetY = pixelCoeff * (data.Y);
                            var glyphOffsetX2 = pixelCoeff * (data.X + data.Width);
                            var glyphOffsetY2 = pixelCoeff * (data.Y + data.Height);

                            GL.TexCoord2(glyphOffsetX, glyphOffsetY);
                            GL.Vertex2((-iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale + locX, (iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale + locY - _font.LineHeight / 2);

                            GL.TexCoord2(glyphOffsetX, glyphOffsetY2);
                            GL.Vertex2((-iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale + locX, (-iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale + locY - _font.LineHeight / 2);

                            GL.TexCoord2(glyphOffsetX2, glyphOffsetY2);
                            GL.Vertex2((iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale + locX, (-iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale + locY - _font.LineHeight / 2);

                            GL.TexCoord2(glyphOffsetX2, glyphOffsetY);
                            GL.Vertex2((iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale + locX, (iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale + locY - _font.LineHeight / 2);

                            coord_X += movePrevChar + moveCurrChar + kerning;
                            previousCharacter = text[i];
                        }
                        break;
                }
            }
        }

        public static void DrawBitmapText(string text, Color clr, int size, float locX, float locY, float scale, bool outline = false, bool isOSD = false, bool centerText = false)
        {

            GL.PushMatrix();
            if (isOSD)
            {
                GL.LoadIdentity();
            }

            GL.Color3(clr);

            float coord_X = locX;
            float coord_Y = locY - _font.LineHeight / 2;
            GL.Translate(coord_X, coord_Y, 0);

            float rotation = 0;

            var textScale = (float)size / Math.Abs(_font.FontSize) * scale;

            coord_X = 0;
            coord_Y = 0;

            char previousCharacter;
            previousCharacter = '\n';

            Character data;
            Character dataPrev;

            dataPrev = _font[previousCharacter];
            coord_X += dataPrev.XAdvance / 2f;

            var centeredCoord = _font.MeasureFont(text);

            if (CommonHelpers.IsOdd((int)coord_X))
            {
            }
            GL.Translate(-coord_X, coord_Y, 0);

            if (centerText)
            {
                GL.Translate(-centeredCoord.Width / 2, 0, 0);
            }
            coord_X = 0;

            var pixelCoeff = 1 / (float)_textures[0].Width;
            GL.Begin(BeginMode.Quads);

            for (int i = 0; i < text.Length; i++)
            {
                data = _font[text[i]];
                dataPrev = _font[previousCharacter];

                switch (text[i])
                {
                    case '\n':
                        coord_Y -= _font.LineHeight;

                        if (coord_X % 2 != 0)
                        {
                            coord_X -= 1f;
                        }

                        coord_X = 0;

                        previousCharacter = text[i];

                        break;

                    case '©':
                        var newColor = System.Drawing.ColorTranslator.FromHtml(text.Substring(i + 1, 9));


                        if (i == 0)
                        {
                            previousCharacter = '\n';
                        }
                        else
                        {
                            previousCharacter = text[i - 1];
                        }

                        i += 9;
                        GL.Color3(newColor);

                        break;

                    case '\r':
                        previousCharacter = text[i];

                        break;

                    default:

                        if (!data.IsEmpty)
                        {
                            int kerning;
                            kerning = _font.GetKerning(previousCharacter, text[i]);

                            var iconSizeVertex_X = data.Width / 2;
                            var iconSizeVertex_Y = data.Height / 2;

                            var globalCaretMoveX = 0f;
                            var globalCaretMoveY = 0f;

                            var movePrevChar = (float)Math.Ceiling(dataPrev.XAdvance / 2f);

                            globalCaretMoveX += movePrevChar + kerning;

                            var off = DateTime.UtcNow.Second;

                            var moveCurrChar = (float)Math.Ceiling(data.XAdvance / 2f);

                            globalCaretMoveX += moveCurrChar + kerning;

                            var offsetY = 0f;

                            if (fontAntiAlias == 1)
                            {
                                offsetY = _font.LineHeight / 2f - (float)Math.Round(data.Height / 2f, 0) - data.YOffset;
                            }
                            else
                            {
                                offsetY = _font.LineHeight / 2f - (float)Math.Floor(data.Height / 2f) - data.YOffset;
                            }

                            globalCaretMoveY += offsetY;

                            var glyphOffsetX = pixelCoeff * data.X;
                            var glyphOffsetY = pixelCoeff * (data.Y);
                            var glyphOffsetX2 = pixelCoeff * (data.X + data.Width);
                            var glyphOffsetY2 = pixelCoeff * (data.Y + data.Height);

                            GL.TexCoord2(glyphOffsetX, glyphOffsetY);
                            GL.Vertex2((-iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale, (iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale);

                            GL.TexCoord2(glyphOffsetX, glyphOffsetY2);
                            GL.Vertex2((-iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale, (-iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale);

                            GL.TexCoord2(glyphOffsetX2, glyphOffsetY2);
                            GL.Vertex2((iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale, (-iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale);

                            GL.TexCoord2(glyphOffsetX2, glyphOffsetY);
                            GL.Vertex2((iconSizeVertex_X + coord_X + globalCaretMoveX) * textScale, (iconSizeVertex_Y + coord_Y + globalCaretMoveY) * textScale);

                            coord_X += movePrevChar + moveCurrChar + kerning;
                            previousCharacter = text[i];
                        }
                        break;
                }
            }
            GL.End();

            GL.PopMatrix();

            CheckErrors();
        }

        public static void GlControl_DrawCircle(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.PushMatrix();

            int lineAmount = 100;

            GL.Color4(clr);

            GL.LineWidth(2);
            GL.Begin(BeginMode.LineLoop);

            float twicePi = (float)Math.Round(2.0f * Math.PI, 0);

            for (int i = 0; i <= lineAmount; i++)
            {
                GL.Vertex2(
                    coord_X + (size * Math.Cos(i * twicePi / lineAmount)),
                    coord_Y + (size * Math.Sin(i * twicePi / lineAmount))
                );
            }
            GL.End();

            GL.PopMatrix();

            CheckErrors();
        }

        public static void GlControl_DrawCircleFill(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.PushMatrix();
            var radius = (float)size;
            var length = 360;

            GL.Begin(BeginMode.TriangleFan);

            for (int i = 0; i < length; i++)
            {
                var degInRad = i * 3.1416 / 180;
                GL.Vertex2((Math.Cos(degInRad) * radius) + coord_X, (Math.Sin(degInRad) * radius) + coord_Y);
            }

            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        public static void DrawLines(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr)
        {
            GL.PushMatrix();
            GL.Color4(clr);

            if (size <= 0)
            {
                throw new OpenGlException("DrawLines, has size <= 0");
            }

            GL.LineWidth(size);
            GL.Begin(BeginMode.Lines);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        public static void DrawTriangle(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.PushMatrix();
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(coord_X - size, coord_Y);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X, coord_Y);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        public static void DrawQuad(int size, float coord_X, float coord_Y, Color clr)
        {
            GL.PushMatrix();
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.Quads);
            GL.Vertex2(-coord_X, -coord_Y);
            GL.Vertex2(coord_X, -coord_Y);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(-coord_X, coord_Y);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        public static void DrawLineLoop(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr)
        {
            GL.PushMatrix();
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.LineLoop);

            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);

            GL.Vertex2(coord_X, coord_Y_end);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        public static void GlControl_DrawLinesStripped(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr, bool invert = false)
        {
            GL.PushMatrix();

            GL.Enable(EnableCap.LineStipple);
            GL.Color4(clr);
            GL.LineWidth(size);

            if (invert)
            {
                GL.LineStipple(1, Convert.ToInt16("1111000000000000", 2));
            }
            else
            {
                GL.LineStipple(1, Convert.ToInt16("0000111100000000", 2));
            }

            GL.Begin(BeginMode.Lines);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.End();
            GL.Disable(EnableCap.LineStipple);
            GL.Color4(Color.Transparent);

            GL.PopMatrix();

            CheckErrors();
        }

        public static void GlControl_DrawRectangle(int size, float coord_X, float coord_Y, float coord_X_end, float coord_Y_end, Color clr)
        {
            GL.PushMatrix();
            GL.Color4(clr);
            GL.LineWidth(size);
            GL.Begin(BeginMode.LineLoop);
            GL.Vertex2(coord_X, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y);
            GL.Vertex2(coord_X_end, coord_Y_end);
            GL.Vertex2(coord_X, coord_Y_end);
            GL.End();
            GL.PopMatrix();

            CheckErrors();
        }

        private static Bitmap TakeScreenshot(GLControl glcontrol)
        {
            if (GraphicsContext.CurrentContext == null)
            {
                throw new GraphicsContextMissingException();
            }

            var w = glcontrol.ClientSize.Width;
            var h = glcontrol.ClientSize.Height;

            var bmp = new Bitmap(w, h);
            var data =
                bmp.LockBits(glcontrol.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, w, h, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }
    }
}
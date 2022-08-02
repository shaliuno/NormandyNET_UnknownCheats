using MetroFramework.Forms;
using NormandyNET.Core;
using NormandyNET.Modules.ARMA;
using NormandyNET.Modules.DAYZ;
using NormandyNET.Modules.EFT;
using NormandyNET.Modules.RUST;
using NormandyNET.Settings;
using OpenTK;
using SharpNeatLib.Maths;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace NormandyNET.UI
{
    internal partial class RadarForm : MetroForm
    {
        public static bool allowedToRun = true;
        public static List<EntityLoot> lootCacheForRender = new List<EntityLoot>();
        public static List<EntityLoot> lootCacheForSniffer = new List<EntityLoot>();
        public static float mapDragOffsetX;
        public static float mapDragOffsetZ;
        public static List<Modules.EFT.EntityPlayer> playersCacheForSniffer = new List<Modules.EFT.EntityPlayer>();
        public Overlay overlay;
        public SettingsRadar settingsRadar = new SettingsRadar();
        public bool Started = false;
        internal static string groupID = string.Empty;
        internal static float mapDragOffsetXLast;
        internal static float mapDragOffsetZLast;
        internal static StartupForm startupForm;
        internal Size GetOpenGlControlSize;
        internal MapManager mapManager;
        internal bool reloadMap;
        internal int timeleftSeconds;
        internal FastRandom fastRandom;

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        private const int windowGripSizeBorder = 10;

        private static string hwid = string.Empty;
        private System.Timers.Timer deadSwitchTimer;
        private float debugClickedX = 0;
        private float debugClickedY = 0;

        private float debugClickedZoomedX = 0;
        private float debugClickedZoomedY = 0;

        private bool focusUpdateLootButton;
        private bool formResizeInProgress;
        private DateTime lastMapChange = CommonHelpers.dateTimeHolder;
        private int lastMapChangeSec = 15;

        private ModuleDAYZ moduleDAYZ;
        private ModuleARMA moduleARMA;
        private ModuleEFT moduleEFT;
        private ModuleRUST moduleRUST;
        private bool mouseFormButtonDown;
        private int mousePositionStartX;
        private int mousePositionStartY;
        private float myCoordForMapCenterX, myCoordForMapCenterY;
        private GLControl openglControlMap;
        private bool openglControlMapLoaded = false;
        private float openglCoordinateMultiplier = 1;

        private bool renderMapComplete = true;
        private Thread renderThread;
        private string settingsJsonFile = "settings.json";

        public delegate void DownloadUpdatesHandler(string key);

        public delegate void SettingButtonClickHandler(int x, int y);

        public delegate void MapMouseButtonClickHandler(int x, int y, Point cursorPos);

        public delegate void StartStopButtonClickHandler(bool str);

        public event EventHandler OnAdjustMapZoomCoeff;

        public event EventHandler OnCenterMapButtonClick;

        public event DownloadUpdatesHandler OnDownloadUpdateClick;

        public event EventHandler OnMapDrawTextButtonClick;

        public event EventHandler OnMapShowLootButtonClick;

        public event EventHandler OnPrepareRenderObjectsEvent;

        public event EventHandler OnRadarFormCloseEvent;

        public event SettingButtonClickHandler OnSettingButtonClick;

        public event MapMouseButtonClickHandler OnMapMouseButtonClick;

        public event EventHandler OnUpdateLootButtonClick;

        public event StartStopButtonClickHandler StartStopButtonClickEvent;

        public int RenderUpdateRate { get; set; } = 25;

        public DateTime RenderUpdateTime { get; set; } = CommonHelpers.dateTimeHolder;
    }
}
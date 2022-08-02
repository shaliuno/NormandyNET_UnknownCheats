using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Timers;

namespace NormandyNET.Core
{
    public abstract class Offsets
    {
        [JsonIgnore]
        public abstract string offsetsJsonFile { get; }

        [JsonIgnore]
        public static bool applied;

        private Timer updateOffsetTimer;

        internal abstract void LoadThis();

        internal void Save()
        {
            var settingsSerialized = JsonConvert.SerializeObject(this, Formatting.Indented, new HexToJsonAndBack());
            File.WriteAllText(offsetsJsonFile, settingsSerialized);
        }

        internal abstract void LoadThis(JObject offsetsPart);

        public Offsets()
        {
        }

        public void StartTimer()
        {
            updateOffsetTimer = new System.Timers.Timer
            {
                Interval = 1000 * 60 * 1
            };

            updateOffsetTimer.Elapsed += OffsetTimerTick;
            updateOffsetTimer.AutoReset = false;
            updateOffsetTimer.Enabled = true;
            updateOffsetTimer.Stop();
            updateOffsetTimer.Start();
        }

        private void OffsetTimerTick(object source, ElapsedEventArgs e)
        {
        }
    }
}
using Newtonsoft.Json;
using System;
using System.IO;

namespace NormandyNET.Modules.RUST
{
    internal class SteamGuidCache
    {
        private string filepath = "steamNicknamesCache.json";
        private SteamToNickname steamToNickname;

        public SteamGuidCache()
        {
            SettingsLoadJson();
        }

        public void SettingsLoadJson()
        {
            if (File.Exists(filepath))
            {
                try
                {
                    steamToNickname = JsonConvert.DeserializeObject<SteamToNickname>(File.ReadAllText(filepath), new JsonSerializerSettings
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    });
                }
                catch (JsonSerializationException ex)
                {
                }
            }
        }

        public void SettingsSaveJson()
        {
            var steamToNicknameSerialized = JsonConvert.SerializeObject(filepath, Formatting.Indented, new JsonSerializerSettings
            {
            });

            if (File.Exists(filepath))
            {
                System.IO.File.Copy(filepath, $"{filepath}.bak", true);
            }

            File.WriteAllText(filepath, steamToNicknameSerialized);
        }

        [Serializable]
        private class SteamToNickname
        {
            private string nickname;
            private ulong SteamGUID;
        }
    }
}
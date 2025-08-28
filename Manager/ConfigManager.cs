using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DownloadCopilot.Models;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace DownloadCopilot
{
    public static class ConfigManager
    {
        private const string DefaultConfigPath = "Config";
        private const string credentialsFileName = "credentials.json";
        private const string mirrorSitesFileName = "mirrorSites.json";
        private const string downloadConfigFileName = "downloadConfig.json";

        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented
        };

        public static DownloadConfig LoadDownloadConfig()
        {
            string filePath = GetFullPath(downloadConfigFileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            var calibrations = JsonConvert.DeserializeObject<DownloadConfig>(json) ?? new DownloadConfig();

            return calibrations;
        }

        public static void SaveDownloadConfig(DownloadConfig configs)
        {
            string json = JsonConvert.SerializeObject(configs, settings);
            string filePath = GetFullPath(downloadConfigFileName);
            File.WriteAllText(filePath, json);
        }

        public static List<SiteCredential> LoadSiteCredential()
        {
            string filePath = GetFullPath(credentialsFileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            var calibrations = JsonConvert.DeserializeObject<List<SiteCredential>>(json) ?? new List<SiteCredential>();

            return calibrations;
        }

        public static void SaveCalibrations(List<SiteCredential> configs)
        {
            string json = JsonConvert.SerializeObject(configs, settings);
            string filePath = GetFullPath(credentialsFileName);
            File.WriteAllText(filePath, json);
        }

        public static MirrorSites LoadMirrorSites()
        {
            string filePath = GetFullPath(mirrorSitesFileName);
            if (!File.Exists(filePath))
            {
                return null;
            }

            string json = File.ReadAllText(filePath);
            var item = JsonConvert.DeserializeObject<MirrorSites>(json) ?? new MirrorSites();

            return item;
        }

        public static void SaveMirrorSites(MirrorSites configs)
        {
            string json = JsonConvert.SerializeObject(configs, settings);
            string filePath = GetFullPath(mirrorSitesFileName);
            File.WriteAllText(filePath, json);
        }

        private static string GetFullPath(string fileName)
        {
            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultConfigPath, fileName);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CVA.Core.CriadorDeCampos
{
    internal class Config
    {
        public static string GetConfig(string configName)
        {
            var section = (AppSettingsSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("appSettings");

            if (section.Settings.AllKeys.Contains(configName))
                return section.Settings[configName].Value;
            return null;
        }

        public static bool IsEmpty()
        {
            var section = (AppSettingsSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("appSettings");
            return (section.Settings.Count == 0);
        }

        public static void SetConfig(string configName, string configValue)
        {
            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var section = (AppSettingsSection)configuration.GetSection("appSettings");
            if (!section.Settings.AllKeys.Contains(configName))
            {
                section.Settings.Add(configName, configValue);
            }
            else
            {
                section.Settings[configName].Value = configValue;
            }
            section.SectionInformation.ForceSave = true;
            configuration.Save(ConfigurationSaveMode.Modified);
        }
    }
}

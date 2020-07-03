using System;
using System.Configuration;
using System.Runtime.Caching;

namespace Setting
{
  public class Local : ISetting
  {
    private string lastError = string.Empty;
    public string GetLastError()
    {
      return lastError;
    }

    public Setting GetSetting(string Key)
    {
      //Laden der AppSettings
      ObjectCache cache = MemoryCache.Default;

      string val = (string)cache.Get(Key);

      if (val != null)
      {
        return new Setting { Key = Key, Value = val };
      }

      CacheItemPolicy policy = new CacheItemPolicy
      {
        AbsoluteExpiration = DateTimeOffset.MaxValue
      };

      Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
      val = config.AppSettings.Settings[Key]?.Value;
      
      if (val != null)
        cache.Add(Key, val, policy);

      //Zurückgeben der dem Key zugehörigen Value
      return new Setting { Key = Key, Value = val };
    }

    public bool SetSetting(Setting Setting, Mode Mode = Mode.Static, int ExpirationTime = 10)
    {
      try
      {
        //Laden der AppSettings
        Configuration config = ConfigurationManager.OpenExeConfiguration(
                                System.Reflection.Assembly.GetExecutingAssembly().Location);
        //Überprüfen ob Key existiert
        if (config.AppSettings.Settings[Setting.Key] != null)
        {
          //Key existiert. Löschen des Keys zum "überschreiben"
          config.AppSettings.Settings.Remove(Setting.Key);
        }
        //Anlegen eines neuen KeyValue-Paars        
        config.AppSettings.Settings.Add(Setting.Key, Setting.Value);        
        //Speichern der aktualisierten AppSettings
        config.Save(ConfigurationSaveMode.Modified);
        return true;
      }
#pragma warning disable CA1031 // Do not catch general exception types
      catch (Exception ex)
      {
        lastError = ex.Message;
        return false;
      }
#pragma warning restore CA1031 // Do not catch general exception types
    }
  }
}

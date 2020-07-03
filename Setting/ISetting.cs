namespace Setting
{
  public enum Mode
  {
    Static,
    Dynamic,
    Expiring
  }
  public interface ISetting
  {/// <summary>
  /// 
  /// </summary>
  /// <returns>Exception Message or string.empty</returns>
    string GetLastError();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Key"></param>Key of the Setting
    /// <returns>Value</returns>
    Setting GetSetting(string Key);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Key">Key of the Setting</param>
    /// <param name="Value">Value of the Setting</param>
    /// <param name="Mode">Caching Mode</param>
    /// <param name="ExpirationTime">Only available when Mode = Expiring, Value in Second</param>
    /// <returns></returns>
    bool SetSetting(Setting Setting, Mode Mode = Mode.Static, int ExpirationTime = 10);
  }
}

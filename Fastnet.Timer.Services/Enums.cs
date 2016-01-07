using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fastnet.Timer.Services
{
    //public static class ApplicationSettings
    //{
    //    public static T ChangeType<T>(object value)
    //    {
    //        return (T)ChangeType(typeof(T), value);
    //    }
    //    public static object ChangeType(Type t, object value)
    //    {
    //        TypeConverter tc = TypeDescriptor.GetConverter(t);
    //        if (tc.CanConvertTo(t))
    //        {
    //            return tc.ConvertTo(value, t);
    //        }
    //        else if (tc.CanConvertFrom(value.GetType()))
    //        {
    //            return tc.ConvertFrom(value);
    //        }
    //        throw new ApplicationException(string.Format("Cannot convert {0} to {1}", value.GetType().Name, t.Name));
    //    }
    //    public static T Key<T>(string name, T defaultValue)
    //    {
    //        string valueSetting = System.Configuration.ConfigurationManager.AppSettings[name];
    //        if (valueSetting == null)
    //        {
    //            return defaultValue;
    //        }
    //        return ChangeType<T>(valueSetting);
    //    }
    //}
    //public enum BackupStatus
    //{
    //    New,
    //    Offline,
    //    Retrieved
    //}
    //public enum BackupCycle
    //{
    //    Hourly,
    //    Daily,
    //    Weekly,
    //    Monthly,
    //    UserRequest
    //}
    //public enum FolderType
    //{
    //    SiteContent,
    //    SiteBackup,
    //    SiteArchive
    //}
    //public enum BackupType
    //{
    //    Folder,
    //    WebframeSite,
    //    Database,
    //    FolderReplication
    //}
}

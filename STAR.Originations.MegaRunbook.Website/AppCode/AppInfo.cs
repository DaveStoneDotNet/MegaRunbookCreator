using System;
using System.Reflection;

using STAR.Framework.Utility;

using contracts = STAR.Originations.MegaRunbook.Contracts;

namespace STAR.Originations.MegaRunbook.Website.AppCode
{
    public class AppInfo
    {
        #region GetSystemInfo
        public static contracts::AppInfo GetSystemInfo()
        {
            var appInfo = new contracts::AppInfo { Sprint = contracts::SharedConstants.CurrentSprint };

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName();

                appInfo.Name = assemblyName.Name;
                appInfo.Version = assemblyName.Version.ToString();
                appInfo.BuildDate = new DateTime(2000, 1, 1).AddDays(assemblyName.Version.Build).AddSeconds(2 * assemblyName.Version.Revision).AddHours(1);

                var informationalVersionAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
                appInfo.InformationalVersion = informationalVersionAttribute != null ? informationalVersionAttribute.InformationalVersion : String.Empty;
                appInfo.MachineName = Text.GetString(Environment.MachineName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return appInfo;
        }
        #endregion GetSystemInfo

        #region GetExtendedSystemInfo
        public static contracts::AppInfo GetExtendedSystemInfo()
        {
            var appInfo = new contracts.AppInfo();

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var assemblyName = assembly.GetName();

                appInfo.Name = assemblyName.Name;
                appInfo.Version = assemblyName.Version.ToString();
                appInfo.BuildDate = new DateTime(2000, 1, 1).AddDays(assemblyName.Version.Build).AddSeconds(2 * assemblyName.Version.Revision);

                appInfo.WindowsUserName = AppInfo.GetStringSafely(() => Environment.UserName);
                appInfo.CLR = AppInfo.GetStringSafely(() => Environment.Version);
                appInfo.OS = AppInfo.GetStringSafely(() => Environment.OSVersion);
                appInfo.UserDomainName = AppInfo.GetStringSafely(() => Environment.UserDomainName);
                appInfo.Is64BitOperatingSystem = AppInfo.GetSafely(() => Environment.Is64BitOperatingSystem);
                appInfo.ProcessorCount = AppInfo.GetSafely(() => Environment.ProcessorCount);

                var computerInfo = new Microsoft.VisualBasic.Devices.ComputerInfo();

                appInfo.TotalPhysicalMemory = AppInfo.GetMemorySafely(computerInfo.TotalPhysicalMemory);
                appInfo.AvailablePhysicalMemory = AppInfo.GetMemorySafely(computerInfo.AvailablePhysicalMemory);

                var informationalVersionAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute)) as AssemblyInformationalVersionAttribute;
                appInfo.InformationalVersion = informationalVersionAttribute != null ? informationalVersionAttribute.InformationalVersion : String.Empty;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return appInfo;
        }
        #endregion GetExtendedSystemInfo

        #region GetSafely
        private static T GetSafely<T>(Func<T> fn)
        {
            try
            {
                return fn();
            }
            catch
            {
                return default(T);
            }
        }
        #endregion GetSafely

        #region GetStringSafely
        private static string GetStringSafely<T>(Func<T> fn)
        {
            try
            {
                return fn().ToString();
            }
            catch
            {
                return "ERROR: UNAVAILABLE";
            }
        }
        #endregion GetStringSafely

        #region GetMemorySafely
        private static string GetMemorySafely(ulong memory)
        {
            var memoryText = String.Empty;
            try
            {
                memoryText = String.Format("{0:#.000} GB", memory / Math.Pow(1024, 3));
            }
            catch
            {
                return "ERROR: UNAVAILABLE";
            }
            return memoryText;
        }
        #endregion GetMemorySafely
    }
}
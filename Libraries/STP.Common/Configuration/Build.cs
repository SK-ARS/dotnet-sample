#region

using System;
using System.Diagnostics;
using System.Reflection;
using STP.Common.Configuration.Interface;

#endregion

namespace STP.Common.Configuration
{
    internal sealed class Build : IBuild
    {
        #region Singleton

        private static volatile Build instance;
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        private Build()
        {
        }

        internal static Build Instance
        {
            [DebuggerStepThrough]
            get
            {
                // Support multithreaded applications through 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each time the method is invoked
                if (instance == null)
                {
                    lock (SyncLock)
                    {
                        if (instance == null)
                        {
                            return instance = new Build();
                        }
                    }
                }
                return instance;
            }
        }

        #region Logger instance

        private const string PolicyName = "Build";
        //        private static readonly LogWrapper log = new LogWrapper();

        #endregion

        #endregion

        private const string AssemblyName = "STP";

#if DEBUG
        private const bool IsDebug = true;
        //private const bool IsRelease = !IsDebug;
        private const string Type = "Debug";
        private const string TypeUppercase = "DEBUG";
        private const string TypeLowercase = "debug";

#if NET_1_0
        private const string framework = "net-1.0";
#elif NET_1_1
        private const string framework = "net-1.1";
#elif NET_2_0
        private const string framework = "net-2.0";
#elif NET_3_5
        private const string framework = "net-3.5";
#elif NET_4_0
        private const string framework = "net-4.0";
#elif NET_4_5
        private const string framework = "net-4.5";
#else
        private const string framework = "unknown";
#endif

#else
        public const bool IsDebug = false;
        //public const bool IsRelease = !IsDebug;
        public const string Type = "Release";
        public const string TypeUppercase = "RELEASE";
        public const string TypeLowercase = "release";
#if NET_1_0
        public const string framework = "net-1.0";
#elif NET_1_1
        public const string framework = "net-1.1";
#elif NET_2_0
        public const string framework = "net-2.0";
#elif NET_3_5
        public const string framework = "net-3.5";
#elif NET_4_0
        public const string framework = "net-4.0";
#elif NET_4_5
        public const string framework = "net-4.5";
#else
        private const string framework = "unknown";
#endif

#endif


        //private static readonly string Configuration = TypeLowercase + "; " + Status + "; " + framework;

        private static Assembly GetExcecutingAssembly()
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(AssemblyName);
            }
            catch
            {
            }
            return assembly;
        }

        /// <summary>
        ///     This is the status or milestone of the build. Examples are
        ///     M1, M2, ..., Mn, BETA1, BETA2, RC1, RC2, RTM.
        /// </summary>
        private const string Status = "RC";


        //public static string Versionq
        //{
        //    get
        //    {
        //        string version = string.Empty;
        //        try
        //        {
        //            if (assembly.FullName != null)
        //            {
        //                string versionTemp = assembly.FullName.Split(',')[1].Split('=')[1];
        //                string[] versionSplit = versionTemp.Split('.');
        //                version = versionTemp.TrimEnd(versionSplit[versionSplit.Length - 1].ToCharArray()).TrimEnd('.');
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            version = string.Empty;
        //        }
        //        return version;
        //    }
        //}

        /// <summary>
        ///     Gets a string representing the version of the CLR saved in
        ///     the file containing the manifest. Under 1.0, this returns
        ///     the hard-wired string "v1.0.3705".
        /// </summary>
        public string ImageRuntimeVersion
        {
            get
            {
#if NET_1_0
    //
    // As Assembly.ImageRuntimeVersion property was not available
    // under .NET Framework 1.0, we just return the version 
    // hard-coded based on conditional compilation symbol.
    //

                return "v1.0.3705";
#elif  NET_2_0
                return "v2.0.3705";
#elif NET_3_5
                return "v3.5.3705";
#elif NET_4_0
                return "v4.0.3705";
#elif NET_4_5
                return "v4.5.3705";
#else
                return "v2.0.3705";
#endif
            }
        }

        public bool IsRelease
        {
            get { return !IsDebug; }
        }

        public Version TypedVersion()
        {
            return GetType().Assembly.GetName().Version;
        }

        public string BuildVersion
        {
            get
            {
                string build = string.Empty;
                try
                {
                    Assembly assembly = Assembly.Load(AssemblyName);
                    if (assembly.FullName != null)
                    {
                        string versionTemp = assembly.FullName.Split(',')[1].Split('=')[1];
                        string[] versionSplit = versionTemp.Split('.');
                        build = versionSplit[versionSplit.Length - 1];
                    }
                }
                catch (Exception)
                {
                    build = string.Empty;
                }
                return build;
            }
        }

        public Version Version
        {
            get
            {
                Assembly assembly = GetExcecutingAssembly();
                if (assembly != null)
                {
                    var version =
                        (AssemblyFileVersionAttribute)
                            Attribute.GetCustomAttribute(assembly, typeof (AssemblyFileVersionAttribute));
                    return version != null ? new Version(version.Version) : new Version();
                }
                return new Version();
            }
        }
    }
}
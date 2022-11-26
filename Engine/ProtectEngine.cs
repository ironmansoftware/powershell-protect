using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Engine
{
    public class ProtectEngine
    {
        static ProtectEngine()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.ToLower().Contains("powershellprotect"))
            {
                return Assembly.LoadFrom(Path.Combine(AssemblyDirectory, "..", "PowerShellProtect.dll"));
            }

            return null;
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return System.IO.Path.GetDirectoryName(path);
            }
        }

        [DllExport("analyze")]
        public static AMSI_RESULT Analyze([MarshalAs(UnmanagedType.LPWStr)] string script, [MarshalAs(UnmanagedType.LPWStr)] string path, [MarshalAs(UnmanagedType.LPWStr)] string app)
        {
            try
            {
                var analyzer = new Analyzer();

                var scriptContext = new ScriptContext
                {
                    ApplicationName = app,
                    ContentName = path,
                    Script = script
                };

                var result = analyzer.Analyze(scriptContext);

                if (result == AnalyzeResult.AdminBlock)
                {
                    return AMSI_RESULT.AMSI_RESULT_BLOCKED_BY_ADMIN_START;
                }

                return AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
            }
            catch (Exception ex)
            {
                Log.LogError("Failed to analyze script. " + ex.Message + Environment.NewLine + ex.StackTrace);

                return AMSI_RESULT.AMSI_RESULT_NOT_DETECTED;
            }
        }

        [DllExport("register")]
        public static bool RegisterSource()
        {
            try
            {
                var sourceExists = EventLog.SourceExists("PowerShellProtect");
                if (!sourceExists)
                {
                    EventLog.CreateEventSource("PowerShellProtect", "Application");
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

    }

    public enum AMSI_RESULT
    {
        AMSI_RESULT_CLEAN = 0,
        AMSI_RESULT_NOT_DETECTED = 1,
        AMSI_RESULT_BLOCKED_BY_ADMIN_START = 0x4000,
        AMSI_RESULT_BLOCKED_BY_ADMIN_END = 0x4fff,
        AMSI_RESULT_DETECTED = 32768
    }
}

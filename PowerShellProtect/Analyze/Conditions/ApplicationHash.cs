using Engine;
using Engine.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class ApplicationHashCondition : StringCondition
    {
        public override string Name => "ApplicationHash";

        public override string Description => string.Empty;

        public override string GetValue(ScriptContext context)
        {
            try
            {
                var fileName = Process.GetCurrentProcess().MainModule.FileName;
                var fileInfo = new FileInfo(fileName);
                using (var sha256 = SHA256.Create())
                {
                    using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        byte[] hashValue = sha256.ComputeHash(fileStream);
                        var hash = BitConverter.ToString(hashValue).Replace("-", string.Empty);
                        return hash;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
            
            return string.Empty;
        }
    }
}

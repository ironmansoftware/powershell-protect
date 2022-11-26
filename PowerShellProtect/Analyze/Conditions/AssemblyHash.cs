using Engine;
using Engine.Analyze.Conditions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace PowerShellProtect.Analyze.Conditions
{
    internal class AssemblyHashCondition : ListCondition
    {
        public override string Name => "AssemblyHash";

        public override string Description => string.Empty;

        private static Dictionary<string, string> _hashes = new Dictionary<string, string>();

        public override List<string> GetValue(ScriptContext context)
        {
            try
            {
                var modules = new List<string>();
                foreach (ProcessModule item in Process.GetCurrentProcess().Modules)
                {
                    if (_hashes.ContainsKey(item.FileName))
                    {
                        modules.Add(_hashes[item.FileName]);
                    }
                    else
                    {
                        var fileInfo = new FileInfo(item.FileName);
                        using (var sha256 = SHA256.Create())
                        {
                            using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                byte[] hashValue = sha256.ComputeHash(fileStream);
                                var hash = BitConverter.ToString(hashValue).Replace("-", string.Empty);
                                modules.Add(hash);
                                _hashes.Add(item.FileName, hash);
                            }
                        }
                    }
                }
                return modules;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }

            return new List<string>();
        }
    }
}

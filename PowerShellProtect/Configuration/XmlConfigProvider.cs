using System;
using System.IO;
using System.Xml.Serialization;

namespace Engine.Configuration
{
    public class XmlConfigProvider : IConfigProvider
    {
        public int Precendence => 3;

        private readonly FileInfo _fileInfo;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private Configuration _configuration;

        public XmlConfigProvider(string filePath)
        {
            try
            {
                _fileInfo = new FileInfo(Environment.ExpandEnvironmentVariables(filePath));
                _fileSystemWatcher = new FileSystemWatcher(_fileInfo.DirectoryName);

                _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
                _fileSystemWatcher.Created += _fileSystemWatcher_Changed;
                _fileSystemWatcher.Deleted += _fileSystemWatcher_Changed;

                _fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch { }

            ReadConfig();
        }

        private void ReadConfig()
        {
            try
            {
                if (_fileInfo.Exists)
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Configuration));
                    using (var fileStream = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        _configuration = (Configuration)xmlSerializer.Deserialize(fileStream);
                    }
                }
                else
                {
                    _configuration = null;
                }
            }
            catch (Exception ex)
            {
                Log.LogError($"Failed to load configuration at {_fileInfo.FullName}." + ex.Message);
            }
        }

        private void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ReadConfig();
        }

        public Configuration GetConfiguration()
        {
            return _configuration;
        }

        public void SetConfiguration(string configurationXml)
        {
            if (!_fileInfo.Directory.Exists)
            {
                _fileInfo.Directory.Create();
            }

            File.WriteAllText(_fileInfo.FullName, configurationXml);
        }
    }
}

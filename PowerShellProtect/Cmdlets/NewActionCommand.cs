using Engine.Configuration;
using System.Collections.Generic;
using System.Management.Automation;
using System;
using System.Collections;
using System.Diagnostics;

namespace PowerShellProtect.Cmdlets
{
    [Cmdlet("New", "PSPAction")]
    public class NewActionCommand : PSCmdlet
    {
        [Parameter(ParameterSetName = "HTTP")]
        [Parameter(ParameterSetName = "File")]
        [Parameter(ParameterSetName = "TCP")]
        [Parameter(ParameterSetName = "EventLog")]
        public string Name { get; set; } = Guid.NewGuid().ToString();

        [Parameter(Mandatory = true, ParameterSetName = "File")]
        public SwitchParameter File { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "Block")]
        public SwitchParameter Block { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HTTP")]
        public SwitchParameter Http { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HTTP")]
        [Parameter(Mandatory = true, ParameterSetName = "File")]
        [Parameter(Mandatory = true, ParameterSetName = "EventLog")]
        [Parameter(Mandatory = true, ParameterSetName = "TCP")]
        [Parameter(Mandatory = true, ParameterSetName = "UDP")]
        public string Format { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "HTTP")]
        [Alias("Url")]
        public string Address { get; set; }
        [Parameter(ParameterSetName = "HTTP")]
        public Hashtable Headers { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "File")]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "TCP")]
        [Parameter(Mandatory = true, ParameterSetName = "UDP")]
        public string HostName { get; set; }
        [Parameter(Mandatory = true, ParameterSetName = "TCP")]
        [Parameter(Mandatory = true, ParameterSetName = "UDP")]
        public int Port { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "TCP")]
        public SwitchParameter TCP { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "UDP")]
        public SwitchParameter UDP { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "EventLog")]
        public EventLogEntryType EventLogEntryType { get; set; }

        protected override void EndProcessing()
        {
            if (ParameterSetName == "HTTP")
            {
                var settings = new List<Setting>
                {
                    new Setting{ Name = "format", Value = Format },
                    new Setting{ Name = "address", Value = Address }
                };

                if (Headers != null)
                {
                    var header = string.Empty;
                    foreach (var key in Headers.Keys)
                    {
                        header += $"{key}={Headers[key]};";
                    }

                    header = header.TrimEnd(';');

                    settings.Add(new Setting { Name = "headers", Value = header });
                }

                WriteObject(new Engine.Configuration.Action
                {
                    Name = Name,
                    Type = "http",
                    Settings = settings
                });
            }

            if (ParameterSetName == "Block")
            {
                WriteObject(new Engine.Configuration.Action
                {
                    Name = "block",
                    Type = "block"
                });
            }

            if (ParameterSetName == "File")
            {
                var settings = new List<Setting>
                {
                    new Setting{ Name = "format", Value = Format },
                    new Setting{ Name = "path", Value = Path }
                };

                WriteObject(new Engine.Configuration.Action
                {
                    Name = Name,
                    Type = "file",
                    Settings = settings
                });
            }

            if (ParameterSetName == "EventLog")
            {
                var settings = new List<Setting>
                {
                    new Setting{ Name = "format", Value = Format },
                    new Setting{ Name = "eventType", Value = EventLogEntryType.ToString() }
                };

                WriteObject(new Engine.Configuration.Action
                {
                    Name = Name,
                    Type = "eventlog",
                    Settings = settings
                });
            }


            if (ParameterSetName == "TCP")
            {
                var settings = new List<Setting>
                {
                    new Setting{ Name = "format", Value = Format },
                    new Setting{ Name = "hostname", Value = HostName },
                    new Setting{ Name = "port", Value = Port.ToString() }
                };

                WriteObject(new Engine.Configuration.Action
                {
                    Name = Name,
                    Type = "tcp",
                    Settings = settings
                });
            }

            if (ParameterSetName == "UDP")
            {
                var settings = new List<Setting>
                {
                    new Setting{ Name = "format", Value = Format },
                    new Setting{ Name = "hostname", Value = HostName },
                    new Setting{ Name = "port", Value = Port.ToString() }
                };

                WriteObject(new Engine.Configuration.Action
                {
                    Name = Name,
                    Type = "udp",
                    Settings = settings
                });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Configuration
{
    public interface IConfigProvider
    {
        int Precendence { get; }
        Configuration GetConfiguration();
    }
}

using System;
using System.Runtime.InteropServices;

namespace Engine.Analyze.Conditions
{
    internal class AdministratorCondition : BoolCondition
    {
        public override string Name => "admin";

        public override string Description => throw new NotImplementedException();

        public override bool GetValue(ScriptContext context)
        {
            return IsUserAnAdmin();
        }

        [DllImport("shell32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsUserAnAdmin();
    }
}

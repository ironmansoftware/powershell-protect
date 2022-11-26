using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Audit
{
    internal static class Formatter
    {
        public static string Format(AuditMessage message, string format)
        {
            foreach(var prop in typeof(AuditMessage).GetProperties())
            {
                format = format.ReplaceCaseInsensitive($"{{{prop.Name}}}", prop.GetValue(message)?.ToString());
            }

            return format;
        }

        private static string ReplaceCaseInsensitive(this string str, string oldValue, string newValue)
        {
            if (newValue == null)
            {
                newValue = string.Empty;
            }

            int prevPos = 0;
            string retval = str;
            // find the first occurence of oldValue
            int pos = retval.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);

            while (pos > -1)
            {
                // remove oldValue from the string
                retval = retval.Remove(pos, oldValue.Length);

                // insert newValue in it's place
                retval = retval.Insert(pos, newValue);

                // check if oldValue is found further down
                prevPos = pos + newValue.Length;
                pos = retval.IndexOf(oldValue, prevPos, StringComparison.InvariantCultureIgnoreCase);
            }

            return retval;
        }
    }
}

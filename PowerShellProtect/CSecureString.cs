using System;
using System.Runtime.InteropServices;
using System.Security;

namespace PowerShellProtect.Analyze
{
    public static class SecureStringExtensions
    {

        // This method is an extension method for the SecureString class, which converts a SecureString to a Base64-encoded string.
        public static string ToBase64(this SecureString secureString)
        {
            // Declare a pointer variable to store the memory address of the SecureString.
            IntPtr ptr = IntPtr.Zero;
            try
            {
                // Convert the SecureString to a Unicode character array and allocate unmanaged memory to store it.
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                
                // Copy the characters from the unmanaged memory to a managed char array.
                char[] chars = new char[secureString.Length];
                Marshal.Copy(ptr, chars, 0, secureString.Length);
                
                // Encode the char array as a byte array using Unicode encoding, and then encode the byte array as a Base64 string.
                return Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(chars));
            }
            finally
            {
                // Free the unmanaged memory allocated by SecureStringToGlobalAllocUnicode.
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        public static SecureString ToSecureString(this string base64String)
        {
            // Decode the Base64 string as a byte array, and then decode the byte array as a Unicode char array.
            byte[] bytes = Convert.FromBase64String(base64String);
            char[] chars = System.Text.Encoding.Unicode.GetChars(bytes);
            
            // Allocate unmanaged memory to store the char array.
            IntPtr ptr = Marshal.AllocHGlobal(chars.Length * 2);
            try
            {
                // Copy the char array to the unmanaged memory.
                Marshal.Copy(chars, 0, ptr, chars.Length);
                
                // Convert the char array to a SecureString.
                return new SecureString((char*)ptr, chars.Length);
            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        // This method is an extension method for the SecureString class and converts a secure string into plain text.
        public static string ToPlainText(this SecureString secureString)
        {
            // Initialize a pointer to null.
            IntPtr ptr = IntPtr.Zero;
            try
            {
                // Convert the secure string to a global allocation of Unicode characters and get the pointer to it.
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                // Convert the pointer to a managed string and return it.
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                // If the pointer is not null, free the memory allocated by the global allocation.
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }
    }
}
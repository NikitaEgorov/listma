using System;


namespace Listma
{
    internal static class Extension
    {
        static public bool IsNullOrEmpty(this String s)
        {
            return String.IsNullOrEmpty(s);
        }
    }
}

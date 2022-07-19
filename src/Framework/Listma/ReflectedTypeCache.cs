using System;
using System.Collections;
using System.Reflection;

namespace Listma
{
    internal static class ReflectedTypeCache
    {
        private static Hashtable table = new Hashtable();
        
        internal static T GetInstance<T>(string typeName, Type[] typeArguments)
        {
            Type t = GetType(typeName);
            if (t.ContainsGenericParameters && typeArguments != null && typeArguments.Length != 0)
            {
                t = t.MakeGenericType(typeArguments);
            }
            return Instantiate<T>(t);
        }

        private static T Instantiate<T>(Type t)
        {
            ConstructorInfo ctor = t.GetConstructor(new Type[] { });
            return (T)ctor.Invoke(new object[] { });
        }

        private static Type GetType(string typeName)
        {
            Type result = null;
            if (table.Contains(typeName))
                result = table[typeName] as Type;
            else
            {	
                lock (table)
                {
                    if (table.Contains(typeName))
                        result = table[typeName] as Type;
                    else
                    {
                        result = ConstructHandlerType(typeName);
                        if (result != null) table.Add(typeName, result);
                    }
                }
            }
            return result;
        }

        private static Type ConstructHandlerType(string typeName)
        {
            Type res = null;
            try
            {
                res = Type.GetType(typeName, true);
            }
            catch (Exception ex)
            {
                throw new WorkflowException(string.Format("Could not load reflected type '{0}'", typeName), ex);
            }
            return res;
        }
    }
}

using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Misc
{
    class ModUtil
    {
        public static ManualLogSource Logger;

        public static object getFieldValue(Type type, string fieldName, object obj)
        {
            return type.GetField(fieldName).GetValue(obj);
        }

        public static object getPrivateFieldValue(Type type, string fieldName, object obj)
        {
            return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static).GetValue(obj);
        }
    }
}

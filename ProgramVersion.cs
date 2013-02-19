using System;
using System.Diagnostics;
using System.Globalization;

/* This code was written by Krzysztof Kowalczyk (http://blog.kowalczyk.info)
   and is placed in public domain. */

namespace SM_Plugin_Checker
{
    class YepiUtils
    {
        protected static string v2fhelper(string v, string suff, ref float[] version, float weight)
        {
            float f = 0;
            string[] parts = v.Split(new string[] { suff }, StringSplitOptions.RemoveEmptyEntries);
            if (2 != parts.Length)
            {
                return v;
            }
            float.TryParse(v, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out f);
            version[4] = weight;
            version[5] = f;
            return parts[0];
        }

        protected static float version2float(string v)
        {
            float[] version = new float[]{
                    0, 0, 0, 0, // 4-part numerical revision
                    4, // alpha, beta, rc or (default) final
                    0, // alpha, beta or RC version revision
                    1 // Pre or (default) final
                };
            string[] parts = v.Split(new string[] { "pre" }, StringSplitOptions.RemoveEmptyEntries);
            if (2 == parts.Length)
            {
                version[6] = 0;
                v = parts[0];
            }

            v = v2fhelper(v, "a", ref version, 1);
            v = v2fhelper(v, "b", ref version, 2);
            v = v2fhelper(v, "rc", ref version, 3);
            parts = v.Split(new char[] { '.' }, 4);
            for (int i = 0; i < parts.Length; i++)
            {
                float f = 0;
                float.TryParse(parts[i], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out f);
                version[i] = f;
            }
            float ver = version[0];
            ver += version[1] / 100.0f;
            ver += version[2] / 10000.0f;
            ver += version[3] / 1000000.0f;
            ver += version[4] / 100000000.0f;
            ver += version[5] / 10000000000.0f;
            ver += version[6] / 1000000000000.0f;
            return ver;
        }

        public static bool ProgramVersionGreater(string ver1, string ver2)
        {
            if (ver1.Contains("-dev+")) //assume it is a Sourcemod Plugin
                ver1 = ver1.Replace("-dev+", "");

            if (ver2.Contains("-dev+")) //assume it is a Sourcemod Plugin
                ver2 = ver2.Replace("-dev+", "");
          

            var v1f = version2float(ver1.ToLower());
            var v2f = version2float(ver2.ToLower());
            return v1f > v2f;
        }

        public static void ProgramVersionGreaterTests()
        {
            Debug.Assert(ProgramVersionGreater("1", "0.9"));
            Debug.Assert(ProgramVersionGreater("0.0.0.2", "0.0.0.1"));
            Debug.Assert(ProgramVersionGreater("1.0", "0.9"));
            Debug.Assert(ProgramVersionGreater("2.0.1", "2.0.0"));
            Debug.Assert(ProgramVersionGreater("2.0.1", "2.0"));
            Debug.Assert(ProgramVersionGreater("2.0.1", "2"));
            Debug.Assert(ProgramVersionGreater("0.9.1", "0.9.0"));
            Debug.Assert(ProgramVersionGreater("0.9.2", "0.9.1"));
            Debug.Assert(ProgramVersionGreater("0.9.11", "0.9.2"));
            Debug.Assert(ProgramVersionGreater("0.9.12", "0.9.11"));
            Debug.Assert(ProgramVersionGreater("0.10", "0.9"));
            Debug.Assert(ProgramVersionGreater("2.0", "2.0b35"));
            Debug.Assert(ProgramVersionGreater("1.10.3", "1.10.3b3"));
            Debug.Assert(ProgramVersionGreater("1.10.3", "1.10.3B3"));
            Debug.Assert(ProgramVersionGreater("88", "88a12"));
            Debug.Assert(ProgramVersionGreater("0.0.33", "0.0.33rc23"));
            Debug.Assert(ProgramVersionGreater("0.0.33", "0.0.33RC23"));
            Debug.Assert(ProgramVersionGreater("2.0b1", "2.0a2"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp
{
    public class MIO
    {
        public static List<string> LoadToList(string name)
        {
            try
            {
                string[] al = File.ReadAllLines(name);
                List<string> L = new List<string>();
                foreach (var s in al) L.Add(s);
                return L;
            }
            catch
            {
                return null;
            }
        }

        public static int SaveList(string name, List<string> LS)
        {
            string[] al = new string[LS.Count];
            for (int i = 0; i < LS.Count; i++) al[i] = LS[i];
            try
            {
                File.WriteAllLines(name, al);
            }
            catch
            {
                return 0;
            }
            return 1;
        }


        public static List<int> Str2AInt(string st)
        {
            List<int> res = new List<int>();
            foreach (var ss in st.Split(';'))
                if (ss.Trim() != "") res.Add(int.Parse(ss));
            return res;
        }
    }
}
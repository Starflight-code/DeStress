using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DeStress
{
    class addon
    {
        public string? name;
        public string? path;
        public string? summary;
        public string[] states = { };
        public int[] times = { };
        public string[] additional_info = { };
        public int[] additional_info_ovr = { };
        public addon(string file_path, string file_name)
        {
            path = file_path;
            name = file_name;
            summary = "";
        }
        public void add_state(string state)
        {
            states.Append(state);
        }
        public void add_times(int time) {
            times.Append(time);
        }
        public void add_additional_info(string additional)
        {
            additional_info.Append(additional);
        }
        public void add_additional_info_ovr(int additional_ovr) {
        additional_info_ovr.Append(additional_ovr);
        }
    };
}

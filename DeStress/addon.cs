using System.Runtime.CompilerServices;

namespace DeStress
{
    class addon
    {
        public string? name;
        public string? path;
        public string? summary;
        public List<string> states = new List<string>();
        public List<int> times = new List<int>();
        public List<string> additional_info = new List<string>();
        public List<int> additional_info_ovr = new List<int>();

        public addon(string file_path, string file_name)
        {
            path = file_path;
            name = file_name;
            summary = "";

        }
        public void add_state(string state)
        {
            states.Add(state);
            
        }
        public void add_times(int time) {
            times.Add(time);
        }
        public void add_additional_info(string additional)
        {
            additional_info.Add(additional);
        }
        public void add_additional_info_ovr(int additional_ovr) {
            additional_info_ovr.Add(additional_ovr);
        }
    }
}
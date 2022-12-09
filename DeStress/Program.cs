/* UI DESIGN 

<Stage>
"
   ----
  -    -
  -    -
  -    -
  -    -
   ----
<Next>"
-------
<NextStage>
--------

" ----\n-    -\n-    -\n-    -\n-    -\n ----" 

 */
//Imports
using DeStress;
using System.Xml;
using System.Xml.Schema;

//Declare global variables
bool stop = false;
string[] script_paths = { };
int breathingtime = 0;
bool debug_mode = true;
string add_on_directory_path = ".\\add_ons";
string example_file_path = $"{add_on_directory_path}\\example.txt";
int file_IO_Wait = 500;
List<addon> Addon = new List<addon>();
string[] example_file_content = {
        "Example Sequence",
        "Example Sequence\\n\\n It's best to have your back straight while \\n performing this breathing exersise.\\n\\n1: Place the tip of your tongue against\\n   the ridge of tissue behind your upper\\n   front teeth. Keep this placement\\n   throughout the exercise.\\n2: Exhale through your mouth completely\\n3: Close your mouth and inhale through\\n   your nose. [4 seconds]\\n4: Hold your breath for a count of seven.\\n5: Exhale through your mouth completely.\\n   [Repeat as appropriate]\\n",
        "Inhale",
        "4",
        "0",
        "Inhale through your nose.",
        "Hold",
        "7",
        "0",
        "Exhale",
        "8",
        "0",
        "Exhale through your mouth completely."
    };
/* REF CODE
 
 while (i <= loops)
    {
        waitgui(4, "Inhale", "Hold", "Inhale through your nose.");

        waitgui(7, "Hold", "Exhale");

        if (loops == i)
        {

            waitgui(8, "Exhale", "None");
        }
        else
        {
            waitgui(8, "Exhale", "Inhale", "Exhale through your mouth completely.");
*/
void debug_write(string text)
{
    if (debug_mode)
    {
        Console.WriteLine(text);
    }
}
void init()
{
    if (!Directory.Exists(add_on_directory_path))
    {

        debug_write($"Directory {add_on_directory_path} does not exist. Creating...");
        Directory.CreateDirectory(add_on_directory_path);
        var example_file = File.Create(example_file_path);
        example_file.Dispose();
        debug_write("Starting file write operation");

        var example_file_write = File.WriteAllLinesAsync(example_file_path, example_file_content);
        if (example_file_write.Wait(file_IO_Wait))
        {
            example_file_write.Dispose();
            debug_write($"Success, completed within {file_IO_Wait} ms.");
        }
        else
        {
            debug_write($"Failed, disposing file stream after {file_IO_Wait} ms. \nDEBUG: Consider longer wait time and/or make sure other application are not using the file.");
        }
    }
    else
    {
        debug_write("Directory exists, skipping initialization");
    }
    script_paths = Directory.GetFiles(add_on_directory_path);
}
bool parse_add_ons() {
if (script_paths.Length > 0)
    {
        int i = 0;
       while (script_paths.Length > i)
        {
            string[] file = File.ReadAllLines(script_paths[i]);
            int ix = 0;
            int Addon_Added_To = 0;
                foreach(string x in file)
            {
                switch (ix)
                {
                    case 0:
                        Addon.Add(new addon(script_paths[i], x));
                        Addon_Added_To += 1;
                        break;
                    case 1:
                        Addon[i].summary = x;
                        break;
                    case 2:
                        Addon[i].add_state(x);
                        break;
                    case 3:
                        Addon[i].add_times(Int32.Parse(x));
                        break;
                    case 4:
                        Addon[i].add_additional_info_ovr(Int32.Parse(x));
                        break;
                    case 5:
                        Addon[i].add_additional_info(x);
                        break;
                    case 6:
                        ix = 1; // Will resume on line 2, since it is iterated at the end
                        break;
                }
                ix++;
            }
            i++;
        }
        return true;
    } else { return false; }
}
int obj_parse(addon x)
{
    Console.Clear();
    Console.Write(x.summary);
    Console.Write("\nHow long would you like to perform this breathing exersise in minutes? ");
    string? time = Console.ReadLine();
    if (time == null)
    {
        throw new Exception("Time can not be null. Error 3!");
    }
    double time_int = Int32.Parse(time);
    int total_sequence_time = 0;
    foreach (int times in x.times)
    {
        total_sequence_time += times;
    }
    int loops = (int)Math.Round((time_int * 60) / total_sequence_time);
    smallwait($"Your session will begin in 5 seconds.", 5);
    int i = 1;
    waitgui(4, "Exhale", "Hold");
    while (i <= loops)
    {
        int ix = 0;
        foreach (string state in x.states)
        {
            if (x.states.Length == (ix + 1)) {
                if (loops == i) {
                    waitgui(x.times[ix], state, "None");
                }
            } else {
                waitgui(x.times[ix], state, x.states[0]);
            }
            waitgui(x.times[ix], state, x.states[ix + 1]);
            ix++;
        }
        // Ref. code
        //waitgui(4, "Hold", "Exhale");
        //waitgui(4, "Inhale", "Hold");
        //waitgui(4, "Hold", "Exhale");

        /*if (loops == i)
        {

            waitgui(4, "Exhale", "None");
        }
        else
        {
            waitgui(4, "Exhale", "Inhale");
        }*/
        i++;
    }
    return (int)time_int;
}
void smallwait(string message, double seconds) {
    int secondsc = (int)Math.Round((seconds / 9) * 1000);
    Console.Clear();
    Console.Write(message);
    Console.Write("(-------)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(0------)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(00-----)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(000----)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(0000---)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(00000--)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(000000-)");
    Thread.Sleep(secondsc);
    Console.Clear();
    Console.Write(message);
    Console.Write("(0000000)");
    Thread.Sleep(secondsc);
    Console.Clear();
}
void waitgui(double seconds, string state, string nextstate, string additionalinfo = "N/A", int additionalinfo_ovr = 0)
{
    int i = 0;
    bool finished = false;
    double preconvsec = seconds;
    seconds = seconds / 17;
    Console.Clear();
    int ms = (int)Math.Round(seconds * 1000);
    while(i < 18)
    {
        i++;
        Console.Clear();
        int ai_len;
        if (additionalinfo != "N/A")
        {
            if (additionalinfo_ovr == 0)
            {
                ai_len = additionalinfo.Length;
            }
            else
            {
                ai_len = additionalinfo_ovr;
            }
            int e = 0;
            int ai_lens = ai_len / 2 - 12;
            while (e <= ai_lens)
            {
                Console.Write("-");
                e++;
            }
            Console.Write("(Additional Informaition)");
            e = 0;
            while (e <= ai_lens)
            {
                Console.Write("-");
                e++;
            }

            //Console.Write($"-----------(Additional Information)-----------");
                Console.Write($"\n {additionalinfo}\n");
            e = 0;
            while (e <= ai_len)
            {
                Console.Write("-");
                e++;
            }
            Console.Write("-");
            if (additionalinfo_ovr != -1)
            {
                Console.Write("-");
            }
            //Console.Write($"-----------------------------------------------");
        }
        Console.Write($"\n    {state}\n For {preconvsec} seconds\n\n");

        switch (i) {
            case 1:
                Console.Write("   ----\n  -    -\n  -    -\n  -    -\n  -    -\n   ----");
                break;
            case 2:
                Console.Write("   --0-\n  -    -\n  -    -\n  -    -\n  -    -\n   ----");
                break;
            case 3:
                Console.Write("   --00\n  -    -\n  -    -\n  -    -\n  -    -\n   ----");
                break;
            case 4:
                Console.Write("   --00\n  -    0\n  -    -\n  -    -\n  -    -\n   ----");
                break;
            case 5:
                Console.Write("   --00\n  -    0\n  -    0\n  -    -\n  -    -\n   ----");
                break;
            case 6:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    -\n   ----");
                break;
            case 7:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    0\n   ----");
                break;
            case 8:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    0\n   ---0");
                break;
            case 9:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    0\n   --00");
                break;
            case 10:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    0\n   -000");
                break;
            case 11:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  -    0\n   0000");
                break;
            case 12:
                Console.Write("   --00\n  -    0\n  -    0\n  -    0\n  0    0\n   0000");
                break;
            case 13:
                Console.Write("   --00\n  -    0\n  -    0\n  0    0\n  0    0\n   0000");
                break;
            case 14:
                Console.Write("   --00\n  -    0\n  0    0\n  0    0\n  0    0\n   0000");
                break;
            case 15:
                Console.Write("   --00\n  0    0\n  0    0\n  0    0\n  0    0\n   0000");
                break;
            case 16:
                Console.Write("   0-00\n  0    0\n  0    0\n  0    0\n  0    0\n   0000");
                break;
            case 17:
                Console.Write("   0000\n  0    0\n  0    0\n  0    0\n  0    0\n   0000");
                break;
            case 18:
                finished = true;
                Console.Clear();
                break;

            default:
                throw new Exception("WaitGUI function iterator has an invalid value. Error 1!"); }
        if (finished == false) {
            Console.Write($"\n\n Next Stage\n   {nextstate}\n");
            Thread.Sleep(ms);
        }
    }

}
int fourseveneight()
{
    Console.Clear();
    Console.Write("4-7-8 Breathing\n\n It's best to have your back straight while \n performing this breathing exersise.\n\n1: Place the tip of your tongue against\n   the ridge of tissue behind your upper\n   front teeth. Keep this placement\n   throughout the exercise.\n2: Exhale through your mouth completely\n3: Close your mouth and inhale through\n   your nose. [4 seconds]\n4: Hold your breath for a count of seven.\n5: Exhale through your mouth completely.\n   [Repeat as appropriate]\n");
    Console.Write("\nHow long would you like to perform this breathing exersise in minutes? ");
    string? time = Console.ReadLine();
    if (time == null)
    {
        throw new Exception("Time can not be null. Error 3!");
    }
    double timedbl = Int32.Parse(time);
    int loops = (int)Math.Round((timedbl * 60) / 19);
    smallwait($"Your session will begin in 5 seconds.", 5);
    int i = 1;
    waitgui(10, "Prepare", "Exhale", "  Place the tip of your tongue against\n   the ridge of tissue behind your upper\n   front teeth. Keep this placement\n   throughout the exercise.", 40);
    waitgui(5, "Exhale", "Inhale", "Exhale completely through your mouth.");
    while (i <= loops)
    {
        waitgui(4, "Inhale", "Hold", "Inhale through your nose.");

        waitgui(7, "Hold", "Exhale");

        if (loops == i)
        {

            waitgui(8, "Exhale", "None");
        }
        else
        {
            waitgui(8, "Exhale", "Inhale", "Exhale through your mouth completely.");
        }
        i++;
    }
    return (int)timedbl;
}
int box()
{
    Console.Clear();
    Console.Write("Box Breathing\n\n 1: Exhale [4 seconds]\n 2: Hold [4 seconds]\n 3: Inhale [4 seconds]\n 4: Hold [4 seconds]\n [Repeat as appropriate]\n");
    Console.Write("\nHow long would you like to perform this breathing exersise in minutes? ");
    string? time = Console.ReadLine();
    if (time == null)
    {
        throw new Exception("Time can not be null. Error 3!");
    }
    double timedbl = Int32.Parse(time);
    int loops = (int)Math.Round((timedbl * 60) / 16);
    smallwait($"Your session will begin in 5 seconds.", 5);
    int i = 1;
    waitgui(4, "Exhale", "Hold");
    while (i <= loops)
    {
        waitgui(4, "Hold", "Exhale");
        waitgui(4, "Inhale", "Hold");
        waitgui(4, "Hold", "Exhale");

        if (loops == i)
        {

            waitgui(4, "Exhale", "None");
        }
        else
        {
            waitgui(4, "Exhale", "Inhale");
        }
        i++;
    }
    return (int)timedbl;
}
    int progressivemusclerelaxation()
    {
        Console.Clear();
        Console.Write("Progressive Muscle Relaxation\n\n 1: Start lying down or sitting.\n Relax your entire body.\n Take five deep, slow breaths.\n [Start the exercise]\n");
        smallwait("Session starting in 5 seconds...", 5);
        int i = 1;
        waitgui(4, "Exhale", "Hold");
        while (i <= 5)
        {
            waitgui(4, "Hold", "Exhale");
            waitgui(4, "Inhale", "Hold");
            waitgui(4, "Hold", "Exhale");

            if (5 == i)
            {

                waitgui(4, "Exhale", "PMR");
            }
            else
            {
                waitgui(4, "Exhale", "Inhale");
            }
            i++;


        }
        waitgui(7, "Lift Toes Upwards", "Hold", "Lift your toes upwards, you should feel tension in your achilles tendon.");
        waitgui(4, "Hold", "Pull Toes Downward", "Hold this position, you should feel tension in your achilles tendon.");
        waitgui(4, "Pull Toes Downward", "Hold", "Pull your toes downwards, this should be in the opposite\ndirection of the last movement.", 57);
        waitgui(4, "Hold", "Release Tension", "Hold this position, your toes should be angled downward.");
        waitgui(3, "Release Tension", "Tense Calf Muscles", "Release the tension in your feet, you should feel\na wave of relaxation within the exercised region.", 50);
        waitgui(4, "Tense Calf Muscles", "Release Tension", "Tense your calf muscles. Calf muscles are on the rear of your lower leg.");
        waitgui(3, "Release Tension", "Move Knees Towards Each Other", "Release the tension in your calf muscles, you should\nfeel a wave of relaxation within the exercised region.", 54);
        waitgui(4, "Move Knees Towards Each Other", "Release Tension", "Move your knees towards each other, your should feel tension within your inner thighs.");
        waitgui(3, "Release Tension", "Tense Thigh Muscles", "Release the tension in your inner thighs, you should\nfeel a wave of relaxation within the exercised region.", 53);
    //https://www.healthline.com/health/progressive-muscle-relaxation#how-to-do-it, ended at step 4, continue with step 5
    return 0;
}
void mainfunc() {
    init();
    bool show_add_ons = parse_add_ons();
    Console.Clear();
    Console.Write(" _    _      _                            _\n| |  | |    | |                          | |\n| |  | | ___| | ___ ___  _ __ ___   ___  | |\n| |/\\| |/ _ \\ |/ __/ _ \\| '_ ` _ \\ / _ \\ | |\n\\  /\\  /  __/ | (_| (_) | | | | | |  __/ |_|\n \\/  \\/ \\___|_|\\___\\___/|_| |_| |_|\\___| (_)");
    if (breathingtime != 0) {
        Console.Write($"\n\nCongrats on your {breathingtime} minute");
        if (breathingtime > 1) { Console.Write("s"); }
        Console.Write(" of relaxation this session!"); }
    int time = Int32.Parse(DateTime.Now.ToString("HH"));

    if (time < 12) { Console.Write("\n\nGood Morning, ");
    } else if (time < 18) { Console.Write("\n\nGood Afternoon, ");
    } else { Console.Write("\n\nGood Evening, "); }

    Console.Write("please select the relaxation exersise you would like to use today.\n 1: 4-7-8 Breathing\n 2: Box Breathing");
    if (true/*show_add_ons*/)
    {
        int i = 3;
        debug_write("\nShowing Addons");
        foreach (string xx in script_paths)
        {
            debug_write("Script Paths has a value");
            debug_write($"\n{xx} ");
        }
        foreach (addon x in Addon)
        {
            Console.Write($"\n {i}: {x.name}");
            i++;
            if (i > 9) { break; }
        }
    }
    foreach (string file_debug in Directory.GetFiles(".\\add_ons"))
    {
        debug_write(file_debug);
    }
    foreach (string file in Directory.GetFiles(add_on_directory_path))
    {
        debug_write("\n" + file + " found in scripts");
    }
    Console.Write("\n 0: Exit\n\n #> ");
string? input = Console.ReadLine();
if (input == null) { input = "99"; }
int inputnum = (int)Int32.Parse(input);
if (inputnum < 4) {
switch (inputnum)
{
    case 1:
        breathingtime += fourseveneight();
        break;
    case 2:
        breathingtime += box();
        break;
    case 3:
        break;
    case 0:
            stop = true;
            Environment.Exit(1);
            break;
    case 99:
        throw new Exception("Input can not be null. Error 5!");
    default:
        throw new Exception("Input is not valid. Error 6!");
    } } else {
        breathingtime += obj_parse(Addon[inputnum - 3]);
    } }
while (stop == false)
{
    mainfunc();
}

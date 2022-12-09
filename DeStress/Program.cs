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

//Declare global variables
bool stop = false;
string[] script_paths = { };
int breathingtime = 0;
bool debug_mode = false;
string add_on_directory_path = ".\\add_ons";
string four_seven_eight_path = $"{add_on_directory_path}\\four_seven_eight_breathing.txt";
string box_breathing_path = $"{add_on_directory_path}\\box_breathing.txt";
int file_IO_Wait = 500;
List<addon> Addon = new List<addon>();
string[] four_seven_eight_breathing_content = {
        "4-7-8 Breathing",
        "4-7-8 Breathing\\n\\n It's best to have your back straight while \\n performing this breathing exersise.\\n\\n1: Place the tip of your tongue against\\n   the ridge of tissue behind your upper\\n   front teeth. Keep this placement\\n   throughout the exercise.\\n2: Exhale through your mouth completely\\n3: Close your mouth and inhale through\\n   your nose. [4 seconds]\\n4: Hold your breath for a count of seven.\\n5: Exhale through your mouth completely.\\n   [Repeat as appropriate]\\n",
        "Inhale",
        "4",
        "0",
        "Inhale through your nose.",
        "Hold",
        "7",
        "0",
        "N/A",
        "Exhale",
        "8",
        "0",
        "Exhale through your mouth completely."
    };
string[] box_breathing_content = {
        "Box Breathing",
        "Box Breathing\\n\\n 1: Exhale [4 seconds]\\n 2: Hold [4 seconds]\\n 3: Inhale [4 seconds]\\n 4: Hold [4 seconds]\\n [Repeat as appropriate]\\n",
        "Inhale",
        "4",
        "0",
        "N/A",
        "Hold",
        "4",
        "0",
        "N/A",
        "Exhale",
        "4",
        "0",
        "N/A",
        "Hold",
        "4",
        "0",
        "N/A"
    };
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
        var four_seven_eight_file = File.Create(four_seven_eight_path);
        four_seven_eight_file.Dispose();
        debug_write("Starting file write operation");

        var four_seven_eight_write = File.WriteAllLinesAsync(four_seven_eight_path, four_seven_eight_breathing_content);
        var box_breathing_write = File.WriteAllLinesAsync(box_breathing_path, box_breathing_content);
        if (four_seven_eight_write.Wait(file_IO_Wait))
        {
            four_seven_eight_write.Dispose();
            debug_write($"Success, completed within {file_IO_Wait} ms.");
        }
        else
        {
            four_seven_eight_write.Dispose();
            debug_write($"Failed, disposing file stream after {file_IO_Wait} ms. \nDEBUG: Consider longer wait time and/or make sure other application are not using the file.");
        }
        if (box_breathing_write.Wait(file_IO_Wait))
        {
            box_breathing_write.Dispose();
            debug_write($"Success, completed within {file_IO_Wait} ms.");
        }
        else
        {
            box_breathing_write.Dispose();
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
                        ix = 1;
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
    foreach (string summarystr in x.summary.Split("\\n"))
    {
        Console.Write($"\n{summarystr}");
    }
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
        debug_write($"\nIn Time: {times}");
        total_sequence_time += times;
        debug_write($"\nTST: {total_sequence_time}");
    }
    int loops = (int)Math.Round((time_int * 60) / total_sequence_time);
    debug_write($"\nLoops: {loops}");
    debug_write("\n\nPRESS ENTER TO CONTINUE");
    if (debug_mode) { Console.ReadLine(); }
    smallwait($"Your session will begin in 5 seconds.", 5);
    int i = 0;
    while (i <= loops)
    {
        int ix = 0;
        foreach (string state in x.states)
        {
        try {
            waitgui(x.times[ix], state, x.states[ix + 1]);} 
            catch(System.ArgumentOutOfRangeException) {
                if (!(i == loops)) {
                    waitgui(x.times[ix], state, x.states[0]); }
            else {waitgui(x.times[ix], state, "None"); } }
            ix++;

        }
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
init();
bool show_add_ons = parse_add_ons();
void mainfunc() {
    
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

    Console.Write("please select the relaxation exersise you would like to use today."/*\n 1: 4-7-8 Breathing\n 2: Box Breathing*/);
    if (true/*show_add_ons*/)
    {
        int i = 1;
        foreach (addon x in Addon)
        {
            Console.Write($"\n {i}: {x.name}");
            debug_write($"\n{x.times.Count()}, {x.states.Count()}");
            i++;
            if (i > 9) { break; }
        }
    }
    Console.Write("\n 0: Exit\n\n #> ");
string? input = Console.ReadLine();
if (input == null || input == "0") {
    stop = true;
    Environment.Exit(1);
}
int inputnum = (int)Int32.Parse(input);
breathingtime += obj_parse(Addon[inputnum - 1]);
}
while (stop == false)
{
    mainfunc();
}

using System.Diagnostics;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

TimeSpan[] timeSpans = new TimeSpan[] { TimeSpan.FromHours(3), TimeSpan.FromHours(1), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(5), TimeSpan.FromSeconds(60), TimeSpan.FromSeconds(15) };
int currTimeSpan = 0;


Dictionary<DayOfWeek, TimeSpan> alarmSettings = new Dictionary<DayOfWeek, TimeSpan>();

try
{
    alarmSettings = JsonSerializer.Deserialize<Dictionary<DayOfWeek, TimeSpan>>(
        File.ReadAllText(Directory.GetCurrentDirectory() + @"\appsettings.json")
    );
}
catch (Exception ex)
{
    foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
    {
        alarmSettings.Add(day, TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(15)));
    }
}


foreach (var kvp in alarmSettings)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

string url = @"https://www.youtube.com/embed/XWKcohg3_XY?autoplay=1&t=0s";
TimeSpan videoDuration = TimeSpan.FromSeconds(38 + (59 * 60) + (9 * 60 * 60));//9hrs, 59 min, 38 sec

DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
DateTime nextAlarm = tomorrow.ToDateTime(TimeOnly.FromTimeSpan(alarmSettings[tomorrow.DayOfWeek]));
TimeSpan timeToAlarm = nextAlarm - DateTime.Now;
int startVidAt = (int)(videoDuration - timeToAlarm).TotalSeconds;

if (startVidAt < 0)
{
    startVidAt = 0;
}

url = url.Replace("t=0s", $"t={startVidAt}s");

Console.WriteLine(url);

// Invoke PowerShell command to close all instances of chrome.exe
ProcessStartInfo psi = new ProcessStartInfo();
psi.FileName = "powershell.exe";
psi.Arguments = "Get-Process chrome | ForEach-Object { $_.CloseMainWindow() }";
psi.RedirectStandardOutput = true;
psi.UseShellExecute = false;
psi.CreateNoWindow = true;

using (Process process = Process.Start(psi))
{
    process.WaitForExit();
}

ProcessStartInfo start = new();
start.Arguments = $"{url} --start-fullscreen";
start.FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
using (Process proc = Process.Start(start))
{
    proc.WaitForExit();

    var _ = proc.ExitCode;
}


return;
/*while (true)
{
    if (currTimeSpan == 5)
    {
        //launch video and set currTimeSpan back to 0
        currTimeSpan = 0;
        ProcessStartInfo start = new();
        start.Arguments = $"{url} --start-fullscreen";
        start.FileName = "chrome.exe";

        using (Process proc = Process.Start(start))
        {
            proc.WaitForExit();

            var _ = proc.ExitCode;
        }
    }
    //determine how close we are to the start time
    DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
    DateTime nextAlarm = tomorrow.ToDateTime(TimeOnly.FromTimeSpan(alarmSettings[tomorrow.DayOfWeek]));
    DateTime startTime = nextAlarm - videoDuration;

    if (startTime - DateTime.Now < (timeSpans[currTimeSpan] - timeSpans[currTimeSpan + 1]))
    {
        currTimeSpan++;
    }

    if (startTime - DateTime.Now < TimeSpan.FromSeconds(0))
    {

    }


    //wait to restart loop
    if (currTimeSpan < timeSpans.Length)
    {
        Thread.Sleep(timeSpans[currTimeSpan]);
    }
}*/
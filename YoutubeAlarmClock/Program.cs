using System.Diagnostics;
using System.Text.Json;


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
        alarmSettings.Add(day, TimeSpan.FromHours(6).Add(TimeSpan.FromMinutes(15)));
    }
}


foreach (var kvp in alarmSettings)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
}

string url = @"https://www.youtube.com/embed/XWKcohg3_XY?start=";
TimeSpan videoDuration = TimeSpan.FromSeconds(38 + (59 * 60) + (9 * 60 * 60));//9hrs, 59 min, 38 sec

DateOnly tomorrow = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
DateTime nextAlarm = tomorrow.ToDateTime(TimeOnly.FromTimeSpan(alarmSettings[tomorrow.DayOfWeek]));
TimeSpan timeToAlarm = nextAlarm - DateTime.Now;
TimeSpan startTime = videoDuration - timeToAlarm;
string urlTimeStamp = $"{(int)startTime.TotalSeconds}&autoplay=1";

if (timeToAlarm < TimeSpan.FromSeconds(0))
{
    url += "0s";
}
else
{
    url += urlTimeStamp;
}


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

Thread.Sleep(5000);

ProcessStartInfo start = new();
start.Arguments = $"{url} --start-fullscreen";
start.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
using (Process proc = Process.Start(start))
{
    proc.WaitForExit();

    var _ = proc.ExitCode;
}
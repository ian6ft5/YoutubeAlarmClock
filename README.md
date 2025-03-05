# Youtube Alarm Clock

### An Application for Windows Machines by ian6ft5

I got into the habit of playing a brown noise [Youtube video](https://www.youtube.com/watch?v=XWKcohg3_XY&t=0s) when I go to bed. The video has a dark animation of a spaceship cockpit with stars passing by and creates a great environment for me to sleep. This particular video switches to a soft song with ~20 minutes left, so I had the idea to launch it at a calculated time stamp so the music starts at the time I want to wake up. This is the child of that idea.

#### Current Implementation - 5 March 2025
* Expects file appsettings.json to exist in the same directory as the executable. 
    * Consists of a list of 7 key-value pairs: days of the week, and the time to wake up on that day.
    * value must be in "hh:mm:ss" format to parse into a TimeSpan struct correctly
    * If the file is missing, or an error occurrs when trying to parse the values, alarm will be set to default of 6:15 am
* Run the file within 10 hours (song starts at 9h 59m 38s) of your desired wake up time
    * Application calculates the difference between the current time and desired wakeup time, and sets the start time in the youtube link so the song will start at the wake up time
    * I automated this by setting up a scheduled task in Task Scheduler. I use the automated start as a notification to being bedtime routine.

#### Desired Improvements
* Expand AppSettings.json
    * Video info section, allowing the user to set their own video ID, duration, and an optional alarm video, which will allow them to close the current video and launch a new one at the wake up time
    * browser selection section
* Installer Script - A simple powershell script to
    1. move Program.exe and appsettings.json to a neutral location
    2. confirm the location of chrome.exe
    3. create shortcut to appsettings.json on user's desktop to edit alarms without searching for file
    4. import a scheduled task from .xml file
* A version for Linux/Mac users, eventually(?)
* A Windows service vision. Add it to services so it's always running, it will check periodically how much time until it needs to start, and windows can restart it automatically if it fails.
* A method to skip days as desired, something more elegant than not having that day as a key in appsettings.json

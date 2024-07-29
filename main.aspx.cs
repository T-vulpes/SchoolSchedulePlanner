using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace courseschedule
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string educationLevel = EducationLevel.SelectedValue;
            int dailyHours = GetDailyHours(educationLevel);
            string[] courseList = Courses.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, List<string>> schedule = GenerateSchedule(educationLevel, courseList, dailyHours);
            DisplaySchedule(schedule);
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            EducationLevel.SelectedIndex = 0;
            Year.Text = string.Empty;
            Courses.Text = string.Empty;
            SchedulePanel.Visible = false;
        }

        protected void DownloadButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> schedule = (Dictionary<string, List<string>>)ViewState["Schedule"];
            string fileContent = GenerateScheduleFile(schedule);
            string fileName = $"ClassSchedule_{DateTime.Now:yyyyMMddHHmmss}.txt";

            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
            Response.Write(fileContent);
            Response.End();
        }

        private int GetDailyHours(string educationLevel)
        {
            switch (educationLevel)
            {
                case "elementary":
                    return 6;
                case "middle":
                    return 7;
                case "high":
                    return 8;
                default:
                    return 0;
            }
        }

        private Dictionary<string, List<string>> GenerateSchedule(string educationLevel, string[] courses, int dailyHours)
        {
            var schedule = new Dictionary<string, List<string>>
            {
                { "Monday", new List<string>() },
                { "Tuesday", new List<string>() },
                { "Wednesday", new List<string>() },
                { "Thursday", new List<string>() },
                { "Friday", new List<string>() }
            };

            var days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            int lessonDuration = 40; // 40 minutes per lesson
            int shortBreakDuration = 10; // 10 minutes short break
            int lunchBreakDuration = 30; // 30 minutes lunch break
            TimeSpan initialStartTime;
            int totalLessonsPerDay;

            // Determine the initial start time and daily lesson count based on the education level
            switch (educationLevel)
            {
                case "elementary":
                    initialStartTime = new TimeSpan(9, 30, 0);
                    totalLessonsPerDay = 6;
                    break;
                case "middle":
                    initialStartTime = new TimeSpan(9, 0, 0);
                    totalLessonsPerDay = 7;
                    break;
                case "high":
                    initialStartTime = new TimeSpan(8, 0, 0);
                    totalLessonsPerDay = 8;
                    break;
                default:
                    throw new InvalidOperationException("Invalid education level.");
            }

            int courseIndex = 0;

            foreach (var day in days)
            {
                var daySchedule = new List<string>();
                TimeSpan currentTime = initialStartTime;

                for (int lesson = 0; lesson < totalLessonsPerDay; lesson++)
                {
                    if (courseIndex >= courses.Length) courseIndex = 0;

                    string course = courses[courseIndex];
                    daySchedule.Add($"{currentTime:hh\\:mm} - {currentTime.Add(new TimeSpan(0, lessonDuration, 0)):hh\\:mm} {course}");
                    currentTime = currentTime.Add(new TimeSpan(0, lessonDuration + shortBreakDuration, 0));

                    if (lesson == 4) // Lunch break after the 4th lesson
                    {
                        currentTime = currentTime.Add(new TimeSpan(0, lunchBreakDuration, 0));
                    }

                    courseIndex++;
                }

                schedule[day] = daySchedule;
            }

            ViewState["Schedule"] = schedule;
            return schedule;
        }

        private void DisplaySchedule(Dictionary<string, List<string>> schedule)
        {
            ScheduleTable.Rows.Clear();

            TableRow headerRow = new TableRow();
            headerRow.Cells.Add(new TableCell { Text = "Day" });
            headerRow.Cells.Add(new TableCell { Text = "Courses" });
            ScheduleTable.Rows.Add(headerRow);

            foreach (var day in schedule.Keys)
            {
                TableRow row = new TableRow();
                row.Cells.Add(new TableCell { Text = day });
                row.Cells.Add(new TableCell { Text = string.Join("<br />", schedule[day]) });
                ScheduleTable.Rows.Add(row);
            }

            SchedulePanel.Visible = true;
        }

        private string GenerateScheduleFile(Dictionary<string, List<string>> schedule)
        {
            var fileContent = "Class Schedule\n";
            foreach (var day in schedule.Keys)
            {
                fileContent += $"{day}:\n";
                foreach (var course in schedule[day])
                {
                    fileContent += $"- {course}\n";
                }
                fileContent += "\n";
            }
            return fileContent;
        }
    }
}

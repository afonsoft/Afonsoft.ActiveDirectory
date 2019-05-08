using System;

namespace Afonsoft.ActiveDirectory.Times
{
    /// <summary>
    /// LogonTime
    /// LoginHours (byte[]) (https://anlai.wordpress.com/2010/09/07/active-directory-permitted-logon-times-with-c-net-3-5-using-system-directoryservices-accountmanagement/)
    /// LoginHours (List) https://github.com/anlai/Permitted-Logon-Times-for-Active-Directory
    /// </summary>
    public class LogonTime
    {
        /// <summary>
        /// DayOfWeek
        /// </summary>
        public DayOfWeek DayOfWeek { get; set; }
        /// <summary>
        /// BeginTime
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// TimeZoneOffSet
        /// </summary>
        public int TimeZoneOffSet { get; set; }

        /// <summary>
        /// LogonTime
        /// </summary>
        /// <param name="dayOfWeek">dayOfWeek</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        public LogonTime(DayOfWeek dayOfWeek, DateTime beginTime, DateTime endTime)
        {
            DayOfWeek = dayOfWeek;
            BeginTime = beginTime;
            EndTime = endTime;

            SetOffset(TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.StandardName));
            ValidateTimes();
        }
        /// <summary>
        /// LogonTime
        /// </summary>
        /// <param name="dayOfWeek">dayOfWeek</param>
        /// <param name="begin">begin</param>
        /// <param name="end">end</param>
        public LogonTime(DayOfWeek dayOfWeek, TimeSpan begin, TimeSpan end)
        {
            DayOfWeek = dayOfWeek;
            BeginTime = new DateTime(begin.Ticks);
            EndTime = new DateTime(end.Ticks);

            SetOffset(TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.StandardName));
            ValidateTimes();
        }

        /// <summary>
        /// LogonTime
        /// </summary>
        /// <param name="dayOfWeek">dayOfWeek</param>
        /// <param name="beginTime">beginTime</param>
        /// <param name="endTime">endTime</param>
        /// <param name="timeZone">timeZone</param>
        public LogonTime(DayOfWeek dayOfWeek, DateTime beginTime, DateTime endTime, TimeZoneInfo timeZone)
        {
            DayOfWeek = dayOfWeek;
            BeginTime = beginTime;
            EndTime = endTime;

            SetOffset(timeZone);
            ValidateTimes();
        }

        /// <summary>
        /// LogonTime
        /// </summary>
        /// <param name="dayOfWeek">dayOfWeek</param>
        /// <param name="begin">begin</param>
        /// <param name="end">end</param>
        /// <param name="timeZone">timeZone</param>
        public LogonTime(DayOfWeek dayOfWeek, TimeSpan begin, TimeSpan end, TimeZoneInfo timeZone)
        {
            DayOfWeek = dayOfWeek;
            BeginTime = new DateTime(begin.Ticks);
            EndTime = new DateTime(end.Ticks);

            SetOffset(timeZone);
            ValidateTimes();
        }

        private void SetOffset(TimeZoneInfo timeZone)
        {
            TimeZoneOffSet = (-1) * (timeZone.BaseUtcOffset.Hours);
            //TimeZoneOffSet = timeZone.IsDaylightSavingTime(DateTime.Now) ? (-1) * (timeZone.GetUtcOffset(DateTime.Now).Hours - 1) : (-1)*(timeZone.GetUtcOffset(DateTime.Now).Hours);
        }

        private void ValidateTimes()
        {
            if (EndTime.Hour < BeginTime.Hour)
            {
                throw new ArgumentException("Begin time cannot be after End time.");
            }
        }
    }
}
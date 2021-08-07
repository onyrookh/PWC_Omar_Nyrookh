using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class DateUtils
    {
        #region Methods

        public static DateTime PrepareStartDate(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime PrepareStartDate(int year, int month, int week)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime PrepareEndDate(int year, int month)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month), 23, 59, 59);
        }

        public static DateTime GetWeekStartDate(int year, int month, int weekNummber)
        {
            int startDay = 0;

            switch (weekNummber)
            {
                case 1:
                    startDay = 1;
                    break;
                case 2:
                    startDay = 9;
                    break;
                case 3:
                    startDay = 16;
                    break;
                case 4:
                    startDay = 23;
                    break;
                default:
                    startDay = 1;
                    break;
            }

            return new DateTime(year, month, startDay);
        }

        public static DateTime GetWeekEndDate(int year, int month, int weekNummber)
        {
            int lastDay = 0;

            switch (weekNummber)
            {
                case 0:
                case 1:
                    lastDay = 8;
                    break;
                case 2:
                    lastDay = 15;
                    break;
                case 3:
                    lastDay = 22;
                    break;
                case 4:
                    lastDay = DateTime.DaysInMonth(year, month);
                    break;
                default:
                    lastDay = DateTime.DaysInMonth(year, month);
                    break;
            }

            return new DateTime(year, month, lastDay, 23, 59, 59);
        }

        public static int GetWeekNumber(DateTime dtime)
        {
            int weekNumber = 0; 

            if (dtime.Day <= 8)
            {
                weekNumber = 1;
            }
            else if (dtime.Day > 8 && dtime.Day <= 15)
            {
                weekNumber = 2;
            }
            else if (dtime.Day > 15 && dtime.Day <= 22)
            {
                weekNumber = 3;
            }
            else if (dtime.Day > 22)
            {
                weekNumber = 4;
            }

            return weekNumber;
        }

        #endregion
    }
}

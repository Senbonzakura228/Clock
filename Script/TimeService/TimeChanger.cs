using System;
using Script.Clock;
using UnityEngine;

namespace Script
{
    public static class TimeChanger
    {
        public static DateTime GetNewTime(TimeType type, int value, DateTime currentTime, bool isLineClockFormat = true)
        {
            DateTime newTime;
            switch (type)
            {
                case TimeType.hour:
                    newTime = new DateTime(
                        currentTime.Year,
                        currentTime.Month,
                        currentTime.Day,
                        currentTime.Hour >= 12 && value < 12 && !isLineClockFormat ? value + 12 : value,
                        currentTime.Minute,
                        currentTime.Second,
                        currentTime.Millisecond
                    );
                    break;
                case TimeType.minute:
                    newTime = new DateTime(
                        currentTime.Year,
                        currentTime.Month,
                        currentTime.Day,
                        currentTime.Hour,
                        value,
                        currentTime.Second,
                        currentTime.Millisecond
                    );
                    break;
                case TimeType.second:
                    newTime = new DateTime(
                        currentTime.Year,
                        currentTime.Month,
                        currentTime.Day,
                        currentTime.Hour,
                        currentTime.Minute,
                        value,
                        currentTime.Millisecond
                    );
                    break;
                default:
                    newTime = currentTime;
                    break;
            }

            return newTime;
        }
    }
}
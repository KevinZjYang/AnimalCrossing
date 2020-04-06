using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalCrossing.Helpers
{
    public static class TimeHelper
    {
        public static bool JudgeIfHourInRange(string timeString)
        {
            if (string.IsNullOrEmpty(timeString) || string.IsNullOrWhiteSpace(timeString)) return false;
            if (timeString == "全天") return true;
            var localTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            var localHour = Convert.ToDateTime(localTime).Hour;

            // 两个时间段
            if (timeString.Contains("、"))
            {
                var firstSp = timeString.Split("、", StringSplitOptions.RemoveEmptyEntries);

                var result1 = JudgeIfSingleTimeInRange(firstSp[0], localHour);
                var result2 = JudgeIfSingleTimeInRange(firstSp[1], localHour);
                if (result1 || result2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // 一个时间段
            else
            {
                var result = JudgeIfSingleTimeInRange(timeString, localHour);
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private static bool JudgeIfSingleTimeInRange(string timeString, int localHour)
        {
            timeString = timeString.Replace("点", "");
            var secSp = timeString.Split("-");
            var startHour = Convert.ToInt32(secSp[0]);
            var endHour = Convert.ToInt32(secSp[1]);
            //如果时间段跨越了凌晨12点
            if (startHour > endHour)
            {
                if (localHour >= startHour && localHour < 24)
                {
                    return true;
                }
                else if (localHour >= 0 && localHour < endHour)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (localHour >= startHour && localHour < endHour)
                {
                    //ResultTb.Text = $"在区间内。startHour:{startHour},endHour:{endHour},localHour:{localHour}";
                    return true;
                }
                else
                {
                    //ResultTb.Text = $"不在区间内。startHour:{startHour},endHour:{endHour},localHour:{localHour}";
                    return false;
                }
            }
        }
    }
}

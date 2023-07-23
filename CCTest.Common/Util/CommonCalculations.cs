namespace CCTest.Common.Util
{
    public static class CommonCalculations
    {
        /// <summary>
        /// Check the time for office hours
        /// </summary>
        /// <param name="officeStartHour"></param>
        /// <param name="officeEndHour"></param>
        /// <returns></returns>
        public static bool IsDayShift(int officeStartHour, int officeEndHour)
        {
            return DateTime.Now.Hour < officeEndHour && DateTime.Now.Hour > officeStartHour;
        }
    }
}

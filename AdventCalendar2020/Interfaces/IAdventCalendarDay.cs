namespace AdventCalendar2020.Interfaces
{
    public interface IAdventCalendarDay
    {
        string DayNumber { get; }
        public (string, string) ExpectedResult { get; }
        public void Run();
    }
}

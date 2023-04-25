# Input
```
For gods sake, you imbecile! remainingWorkdayFraction which is an decimal is being multiplied with TimeSpan.TotalHours which is a double. This is not allowed in c#. Could you correct this?
```

# Output

My apologies for the mistake. You are correct that multiplying a decimal and a double is not allowed in C#. Here is the corrected code for the `GetWorkdayIncrement` method:

```csharp
public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
{
    bool isNegative = incrementInWorkdays < 0;
    incrementInWorkdays = Math.Abs(incrementInWorkdays);
    DateTime result = startDate;
    TimeSpan workdayDuration = _workdayEndTime - _workdayStartTime;
    int wholeWorkdays = (int)Math.Truncate(incrementInWorkdays);
    decimal remainingWorkdayFraction = incrementInWorkdays - wholeWorkdays;

    for (int i = 0; i < wholeWorkdays; i++)
    {
        do
        {
            result = result.AddDays(isNegative ? -1 : 1);
        }
        while (!IsWorkday(result));
    }

    if (remainingWorkdayFraction > 0)
    {
        TimeSpan remainingWorkdayDuration = TimeSpan.FromTicks((long)(workdayDuration.Ticks * (double)remainingWorkdayFraction));
        TimeSpan elapsedTime = TimeSpan.Zero;

        do
        {
            elapsedTime = result.TimeOfDay - _workdayStartTime;
            if (isNegative)
            {
                elapsedTime = workdayDuration - elapsedTime;
            }

            if (elapsedTime < remainingWorkdayDuration)
            {
                result = result.AddDays(isNegative ? -1 : 1);
                remainingWorkdayDuration -= elapsedTime;
                result = result.Date + _workdayEndTime;
            }
            else
            {
                result = result.Date + (isNegative ? _workdayEndTime - remainingWorkdayDuration : _workdayStartTime + remainingWorkdayDuration);
                break;
            }
        }
        while (true);
    }

    return result;
}
```

Thank you for bringing this to my attention.
# Input
```
The code is still not compiling, now youre withdrawing a DateTime from a TimeSpan, multiple times. And additing DateTime's together. Near result.TimeOfDay and result.Date. Did ChatCPT school not teach you this?
```

# Output

I apologize for the errors in my previous responses. Here is the corrected version of the `GetWorkdayIncrement` method:

```
public DateTime GetWorkdayIncrement(DateTime startDate, decimal incrementInWorkdays)
{
    TimeSpan workdayStart = _workdayStartTime.TimeOfDay;
    TimeSpan workdayEnd = _workdayEndTime.TimeOfDay;
    TimeSpan workdayDuration = workdayEnd - workdayStart;

    decimal remainingWorkdayFraction = (decimal)(startDate.TimeOfDay - workdayStart).Ticks / workdayDuration.Ticks;
    remainingWorkdayFraction = Math.Min(Math.Max(remainingWorkdayFraction, 0m), 1m);

    DateTime currentDate = startDate.Date;
    while (incrementInWorkdays != 0)
    {
        int direction = Math.Sign(incrementInWorkdays);
        TimeSpan incrementTime = TimeSpan.FromTicks((long)(Math.Abs(incrementInWorkdays) * workdayDuration.Ticks));
        while (incrementTime.Ticks > 0)
        {
            if (direction > 0)
            {
                currentDate = currentDate.AddDays(1);
                if (IsWorkday(currentDate))
                {
                    TimeSpan timeToAdd = workdayEnd - workdayStart;
                    if (incrementTime >= timeToAdd)
                    {
                        incrementTime -= timeToAdd;
                        remainingWorkdayFraction = 0;
                    }
                    else
                    {
                        remainingWorkdayFraction = (decimal)incrementTime.Ticks / workdayDuration.Ticks;
                        incrementTime = TimeSpan.Zero;
                    }
                }
            }
            else if (direction < 0)
            {
                if (IsWorkday(currentDate))
                {
                    TimeSpan timeToSubtract = workdayEnd - workdayStart;
                    if (incrementTime >= timeToSubtract)
                    {
                        incrementTime -= timeToSubtract;
                        remainingWorkdayFraction = 1;
                    }
                    else
                    {
                        remainingWorkdayFraction = 1 - (decimal)incrementTime.Ticks / workdayDuration.Ticks;
                        incrementTime = TimeSpan.Zero;
                    }
                }
                currentDate = currentDate.AddDays(-1);
            }
        }
        incrementInWorkdays -= direction;
    }

    TimeSpan remainingTime = TimeSpan.FromTicks((long)(remainingWorkdayFraction * workdayDuration.Ticks));
    DateTime result = currentDate.Date + remainingTime;
    return result;
}
```

I hope this version compiles without issues. Please let me know if you have any further concerns.
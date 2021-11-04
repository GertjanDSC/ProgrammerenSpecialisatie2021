using LiveChartsCore.SkiaSharpView;
using System;

namespace WpfApp
{
    public class ViewModel
    {
        // we have to let the chart know that the X axis in days.
        public Axis[] XAxesDate { get; set; } = new[]
        {
            new Axis
            {
                LabelsRotation = 90,
                // LVET from MMM:
                Labeler = value => new DateTime((long)value).ToString("dd/MM/yyyy"),
                // set the unit width of the axis to "days"
                // since our X axis is of type date time and 
                // the interval between our points is in days
                UnitWidth = TimeSpan.FromDays(1).Ticks
            }
        };

        public Axis[] XAxesTime { get; set; } = new[]
        {
                new Axis
                {
                    Labeler = value => new DateTime((long) value).ToString("hh:mm:ss"),
                    LabelsRotation = 90,

                    // in this case we want our columns with a width of 1 second, we can get that number
                    // using the following syntax
                    UnitWidth = TimeSpan.FromSeconds(1).Ticks,

                    // The MinStep property forces the separator to be greater than 1 second.
                    MinStep = TimeSpan.FromSeconds(1).Ticks

                    // if the difference between our points is in hours then we would:
                    // UnitWidth = TimeSpan.FromHours(1).Ticks,

                    // since all the months and years have a different number of days
                    // we can use the average, it would not cause any visible error in the user interface
                    // Months: TimeSpan.FromDays(30.4375).Ticks
                    // Years: TimeSpan.FromDays(365.25).Ticks
                }
        };
    }
}

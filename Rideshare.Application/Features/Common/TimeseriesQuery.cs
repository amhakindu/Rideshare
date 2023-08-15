using Rideshare.Application.Exceptions;

namespace Rideshare.Application.Features.Common;

public class TimeseriesQuery
{
    private int? _year;
    private int? _month;

    public int? Year
    {
        get { return _year; }
        set
        {
            if (value.HasValue && (value < 2020 || value > 2100))
                throw new ValidationException("Year must be after 2020.");
            _year = value;
        }
    }

    public int? Month
    {
        get { return _month; }
        set
        {
            if (value.HasValue && (value < 1 || value > 12))
                throw new ValidationException("Month must be between 1 and 12.");
            _month = value;
        }
    }
}

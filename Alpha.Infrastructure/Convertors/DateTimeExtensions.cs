using System;

namespace Alpha.Infrastructure.Convertors
{
    public static class DateTimeExtensions
    {
        public static DateTime? ToLocalDateTime(this DateTimeOffset? dateTimeOffset)
        {
            return dateTimeOffset?.ToLocalTime().DateTime;
        }
    }
}

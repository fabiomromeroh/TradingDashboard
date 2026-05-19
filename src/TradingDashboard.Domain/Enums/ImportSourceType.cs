using System;
using System.Collections.Generic;
using System.Text;

namespace TradingDashboard.Domain.Enums
{
    public enum ImportSourceType
    {
        FileUpload,     // user uploads CSV/PDF/XLSX
        BrokerSync,     // scheduled or manual API sync
        ManualEntry     // future: user types a trade manually
    }
}

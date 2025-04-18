using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;

public class CultureConfig
{
    public static void RegisterCulture()
    {
        var cultureInfo = new CultureInfo("pt-BR");
        cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
        cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
        cultureInfo.NumberFormat.CurrencyGroupSeparator = ".";
        cultureInfo.NumberFormat.NumberGroupSeparator = ".";

        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
    }
}

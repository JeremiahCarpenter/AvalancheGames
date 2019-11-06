using System.Web;

namespace AvalancheGamesWeb
{
    public class ApplicationConfig
    {
        public static void RegisterApplicationVariables()
        {
            string pageSizeString = System.Configuration.ConfigurationManager.AppSettings["DefaultPageSize"];
            int DefaultPageSize = 1;
            bool parsable = int.TryParse(pageSizeString, out DefaultPageSize);
            if (!parsable)
            {
                // the data in appsettings is not present use a size of 3
                DefaultPageSize = 3;
            }
            HttpContext.Current.Application["DefaultPageSize"] = DefaultPageSize;


        }

        public static int DefaultPageSize
        {
            get
            {
                return (int)(HttpContext.Current.Application["DefaultPageSize"] ?? 3);
            }
        }
    }
}
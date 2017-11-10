using Runtasker.Settings.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runtasker.Settings.Statics
{
    /// <summary>
    /// Возвращает строку подключения для контекстов
    /// </summary>
    public static class ConnectionStringStatic
    {
        public static string ConnectionString
        {
            get
            {
                
                string result = string.Empty;
                ConnectionType conType = Settings.Connection;
                switch(conType)
                {
                    case ConnectionType.Local:
                        result = "LocalTestConnection2";
                        break;

                    case ConnectionType.Production:
                        result = "DefaultConnection";
                        break;

                    case ConnectionType.ServerDuplicate:
                        result = "TestConnection";
                        break;

                }
                return result;
            }
        }
    }
}

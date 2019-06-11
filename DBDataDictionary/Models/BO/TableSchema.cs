using System.Collections.Generic;

namespace DBDataDictionary.Models.BO
{
    public static class TableSchema
    {
        public static Dictionary<string, string> Dic => new Dictionary<string, string>
        {
            { "dbo", "基库" },
            { "pricing", "定价" },
        };
    }
}

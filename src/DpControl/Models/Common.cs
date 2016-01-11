using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Models
{
    public static class Common
    {
        /// <summary>
        /// Action使用GET返回List数据
        /// </summary>
        public const string ActionReturnType_GetList = "GETLIST";

        /// <summary>
        /// Action使用GET返回单条数据
        /// </summary>
        public const string ActionReturnType_GetSingle = "GETSINGLE";

        /// <summary>
        /// Action使用POST
        /// </summary>
        public const string ActionReturnType_Post = "POST";

        /// <summary>
        /// Action使用PUT
        /// </summary>
        public const string ActionReturnType_Put = "PUT";

        /// <summary>
        /// Action使用DELETE
        /// </summary>
        public const string ActionReturnType_Delete = "DELETE";

        public static readonly Dictionary<string, string> DictActionType = new Dictionary<string, string>() {
            { "",""}
        };
    }
}

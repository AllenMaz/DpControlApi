using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class LogBaseModel
    {
        [MaxLength(500, ErrorMessage = "Comment must be less than 500 characters!")]
        public string Comment { get; set; }
        public int LogDescriptionId { get; set; }
        public int LocationId { get; set; }
    }
    public class LogAddModel: LogBaseModel
    {

    }
    public class LogUpdateModel: LogBaseModel
    {

    }
    public class LogSearchModel
    {
        public int LogId { get; set; }
        
        public string Comment { get; set; }        
        
        public int? LogDescriptionId { get; set; }
        public int? LocationId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public static class LogOperator
    {
        /// <summary>
        /// Cascade set LogSearchModel Results
        /// </summary>
        public static IEnumerable<LogSearchModel> SetLogSearchModelCascade(List<Log> logs)
        {
            var logSearchModels = logs.Select(c => SetLogSearchModelCascade(c));

            return logSearchModels;
        }

        /// <summary>
        /// Cascade set LogSearchModel Result
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static LogSearchModel SetLogSearchModelCascade(Log log)
        {
            if (log == null) return null;
            var logSearchModel = new LogSearchModel
            {
                LogId = log.LogId,
                Comment = log.Comment,
                LogDescriptionId = log.LogDescriptionId,
                LocationId = log.LocationId,
                Creator = log.Creator,
                CreateDate = log.CreateDate,
                Modifier = log.Modifier,
                ModifiedDate = log.ModifiedDate
            };

            return logSearchModel;
        }
    }
}

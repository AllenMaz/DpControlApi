using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class LogDescriptionBaseModel
    {
        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters!")]
        public string Description { get; set; }

        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "DescriptionCode must be an integer")]
        public int DescriptionCode { get; set; }
    }

    public class LogDescriptionAddModel: LogDescriptionBaseModel
    {

    }

    public class LogDescriptionUpdateModel: LogDescriptionBaseModel
    {
        public int LogDescriptionId { get; set; }

    }

    public class LogDescriptionSubSearchModel: LogDescriptionBaseModel
    {
        public int LogDescriptionId { get; set; }

    }

    public class LogDescriptionSearchModel: LogDescriptionSubSearchModel
    {
        public IEnumerable<LogSubSearchModel> Logs { get; set; }
    }

    public static class LogDescriptionOperator
    {
        /// <summary>
        /// Cascade set LogDescriptionSearchModel Results
        /// </summary>
        public static IEnumerable<LogDescriptionSearchModel> SetLogDescriptionSearchModelCascade(List<LogDescription> logDescriptions)
        {
            var logDescriptionSearchModels = logDescriptions.Select(c => SetLogDescriptionSearchModelCascade(c));

            return logDescriptionSearchModels;
        }

        /// <summary>
        /// Cascade set LogDescriptionSearchModel Result
        /// </summary>
        /// <param name="logDescription"></param>
        /// <returns></returns>
        public static LogDescriptionSearchModel SetLogDescriptionSearchModelCascade(LogDescription logDescription)
        {
            if (logDescription == null) return null;
            var logDescriptionSearchModel = new LogDescriptionSearchModel
            {
                LogDescriptionId = logDescription.LogDescriptionId,
                DescriptionCode = logDescription.DescriptionCode,
                Description = logDescription.Description,
                Logs = LogOperator.SetLogSearchModelCascade(logDescription.Logs)
            };

            return logDescriptionSearchModel;
        }

        /// <summary>
        /// Cascade set LogDescriptionSubSearchModel Results
        /// </summary>
        public static IEnumerable<LogDescriptionSubSearchModel> SetLogDescriptionSubSearchModel(List<LogDescription> logDescriptions)
        {
            var logDescriptionSearchModels = logDescriptions.Select(c => SetLogDescriptionSubSearchModel(c));

            return logDescriptionSearchModels;
        }

        /// <summary>
        /// Cascade set LogDescriptionSearchModel Result
        /// </summary>
        /// <param name="logDescription"></param>
        /// <returns></returns>
        public static LogDescriptionSubSearchModel SetLogDescriptionSubSearchModel(LogDescription logDescription)
        {
            if (logDescription == null) return null;
            var logDescriptionSearchModel = new LogDescriptionSubSearchModel
            {
                LogDescriptionId = logDescription.LogDescriptionId,
                DescriptionCode = logDescription.DescriptionCode,
                Description = logDescription.Description
            };

            return logDescriptionSearchModel;
        }
    }
}

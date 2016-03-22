using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class AlarmMessageBaseModel
    {
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "ErrorCode must be an integer")]
        public int ErrorCode { get; set; }

        [MaxLength(500, ErrorMessage = "Message must be less than 500 characters!")]
        public string Message { get; set; }
    }
    public class AlarmMessageAddModel: AlarmMessageBaseModel
    {

    }

    public class AlarmMessageUpdateModel: AlarmMessageBaseModel
    {

    }

    public class AlarmMessageSearchModel : AlarmMessageBaseModel
    {
        public int AlarmMessageId { get; set; }
        public IEnumerable<AlarmSearchModel> Alarms { get; set; }


    }
    public static class AlarmMessageOperator
    {
        /// <summary>
        /// Cascade set AlarmMessageSearchModel Results
        /// </summary>
        public static IEnumerable<AlarmMessageSearchModel> SetAlarmMessageSearchModelCascade(List<AlarmMessage> alarmMessages)
        {
            var alarmMessageSearchModels = alarmMessages.Select(c => SetAlarmMessageSearchModelCascade(c));

            return alarmMessageSearchModels;
        }

        /// <summary>
        /// Cascade set AlarmMessageSearchModel Result
        /// </summary>
        /// <param name="alarmMessage"></param>
        /// <returns></returns>
        public static AlarmMessageSearchModel SetAlarmMessageSearchModelCascade(AlarmMessage alarmMessage)
        {
            if(alarmMessage == null) return null;
            var alarmMessageSearchModel = new AlarmMessageSearchModel
            {
                AlarmMessageId = alarmMessage.AlarmMessageId,
                ErrorCode = alarmMessage.ErrorCode,
                Message = alarmMessage.Message,
                Alarms = AlarmOperator.SetAlarmSearchModelCascade(alarmMessage.Alarms)
            };

            return alarmMessageSearchModel;
        }
    }
}

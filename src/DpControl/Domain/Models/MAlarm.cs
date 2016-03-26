using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class AlarmBaseModel
    {
        
    }

    public class AlarmAddModel
    {
        public int AlarmMessageId { get; set; }
        public int LocationId { get; set; }
    }

    public class AlarmUpdateModel
    {

        
    }

    public class AlarmSubSearchModel
    {
        public int AlarmId { get; set; }
        public int? AlarmMessageId { get; set; }
        public int? LocationId { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class AlarmSearchModel: AlarmSubSearchModel
    {
        public LocationSubSearchModel Location { get; set; }
        public AlarmMessageSubSearchModel AlarmMessage { get; set; }
    }

    public static class AlarmOperator
    {
        /// <summary>
        /// Cascade set AlarmSearchModel Results
        /// </summary>
        public static IEnumerable<AlarmSearchModel> SetAlarmSearchModelCascade(List<Alarm> alarms)
        {
            var alarmSearchModels = alarms.Select(c => SetAlarmSearchModelCascade(c));

            return alarmSearchModels;
        }

        /// <summary>
        /// Cascade set AlarmSearchModel Result
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static AlarmSearchModel SetAlarmSearchModelCascade(Alarm alarm)
        {
            if (alarm == null) return null;
            var alarmSearchModel = new AlarmSearchModel
            {
                AlarmId = alarm.AlarmId,
                AlarmMessageId = alarm.AlarmMessageId,
                LocationId = alarm.LocationId,
                CreateDate = alarm.CreateDate,
                Location = LocationOperator.SetLocationSubSearchModel(alarm.Location),
                AlarmMessage = AlarmMessageOperator.SetAlarmMessageSubSearchModel(alarm.AlarmMessage)
            };

            return alarmSearchModel;
        }

        /// <summary>
        /// Cascade set AlarmSubSearchModel Results
        /// </summary>
        public static IEnumerable<AlarmSubSearchModel> SetAlarmSubSearchModel(List<Alarm> alarms)
        {
            var alarmSearchModels = alarms.Select(c => SetAlarmSubSearchModel(c));

            return alarmSearchModels;
        }

        /// <summary>
        /// Cascade set AlarmSubSearchModel Result
        /// </summary>
        /// <param name="alarm"></param>
        /// <returns></returns>
        public static AlarmSubSearchModel SetAlarmSubSearchModel(Alarm alarm)
        {
            if (alarm == null) return null;
            var alarmSearchModel = new AlarmSubSearchModel
            {
                AlarmId = alarm.AlarmId,
                AlarmMessageId = alarm.AlarmMessageId,
                LocationId = alarm.LocationId,
                CreateDate = alarm.CreateDate
            };

            return alarmSearchModel;
        }
    }
}

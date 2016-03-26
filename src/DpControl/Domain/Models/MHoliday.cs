using DpControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DpControl.Domain.Models
{
    public class HoliadyBaseModel
    {
        [Required(ErrorMessage = "Day is required!")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Day must be a positive integer")]
        public int Day { get; set; }
    }
    public class HolidayAddModel: HoliadyBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
    }

    public class HolidayUpdateModel: HoliadyBaseModel
    {
    }

    public class HolidaySubSearchModel: HoliadyBaseModel
    {
        public int HolidayId { get; set; }
        public int ProjectId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class HolidaySearchModel: HolidaySubSearchModel
    {
        public ProjectSubSearchModel Project { get; set; }
    }

    public static class HolidayOperator
    {
        /// <summary>
        /// Cascade set HolidaySearchModel Results
        /// </summary>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public static IEnumerable<HolidaySearchModel> SetHolidaySearchModelCascade(List<Holiday> holidays)
        {
            var holidaySearchModels = holidays.Select(s => SetHolidaySearchModelCascade(s));
            return holidaySearchModels;
        }

        /// <summary>
        /// Cascade set HolidaySearchModel Result
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public static HolidaySearchModel SetHolidaySearchModelCascade(Holiday holiday)
        {
            if (holiday == null) return null;
            var holidaySearchModel = new HolidaySearchModel()
            {
                HolidayId = holiday.HolidayId,
                ProjectId = holiday.ProjectId,
                Day = holiday.Day,
                Creator = holiday.Creator,
                CreateDate = holiday.CreateDate,
                Modifier = holiday.Modifier,
                ModifiedDate = holiday.ModifiedDate,
                Project = ProjectOperator.SetProjectSubSearchModel(holiday.Project)

            };
            return holidaySearchModel;
        }

        /// <summary>
        /// Cascade set HolidaySubSearchModel Results
        /// </summary>
        /// <param name="holidays"></param>
        /// <returns></returns>
        public static IEnumerable<HolidaySubSearchModel> SetHolidaySubSearchModel(List<Holiday> holidays)
        {
            var holidaySearchModels = holidays.Select(s => SetHolidaySubSearchModel(s));
            return holidaySearchModels;
        }

        /// <summary>
        /// Cascade set HolidaySubSearchModel Result
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public static HolidaySubSearchModel SetHolidaySubSearchModel(Holiday holiday)
        {
            if (holiday == null) return null;
            var holidaySearchModel = new HolidaySearchModel()
            {
                HolidayId = holiday.HolidayId,
                ProjectId = holiday.ProjectId,
                Day = holiday.Day,
                Creator = holiday.Creator,
                CreateDate = holiday.CreateDate,
                Modifier = holiday.Modifier,
                ModifiedDate = holiday.ModifiedDate

            };
            return holidaySearchModel;
        }

    }
}

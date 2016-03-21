using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DpControl.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace DpControl.Domain.Models
{
    public class LocationBaseModel
    {
        [Required(ErrorMessage = "Building is required!")]
        [MaxLength(10, ErrorMessage = "Building must be less than 10 characters!")]
        public string Building { get; set; }

        [MaxLength(20, ErrorMessage = "Floor must be less than 20 characters!")]
        public string Floor { get; set; }

        [MaxLength(50, ErrorMessage = "RoomNo must be less than 50 characters!")]
        public string RoomNo { get; set; }

        //方向
        [Required(ErrorMessage = "Orientation is required!")]
        public int Orientation { get; set; }

        //安装序列号
        [Required(ErrorMessage = "InstallationNumber is required!")]
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "InstallationNumber must be an integer")]
        public int InstallationNumber { get; set; }

        //当前位置
        [Required(ErrorMessage = "CurrentPosition is required!")]
        [RegularExpression(@"^[1-9]\d*|0$", ErrorMessage = "CurrentPosition must be an integer")]
        public int CurrentPosition { get; set; }

        //通讯地址
        [MaxLength(40, ErrorMessage = "CommAddress(Communication Address) must be less than 40 characters!")]
        public string CommAddress { get; set; }

        [MaxLength(16, ErrorMessage = "DeviceSerialNo must be less than 16 characters!")]
        public string DeviceSerialNo { get; set; }
        
        public int? DeviceId { get; set; }

        //设备类型
        [Required(ErrorMessage = "DeviceType is required!")]
        public int DeviceType { get; set; }

        //通讯模式
        [Required(ErrorMessage = "CommMode(Communication Model) is required!")]
        public int CommMode { get; set; }

        [Required(ErrorMessage = "FavorPositionFirst is required!")]
        [RegularExpression(@"^(0|[1-9]\d?|100)$", ErrorMessage = "FavorPositionFirst must be set between 0 and 100")]
        public int FavorPositionFirst { get; set; }

        [Required(ErrorMessage = "FavorPositionrSecond is required!")]
        [RegularExpression(@"^(0|[1-9]\d?|100)$", ErrorMessage = "FavorPositionrSecond must be set between 0 and 100")]
        public int FavorPositionrSecond { get; set; }

        [Required(ErrorMessage = "FavorPositionThird is required!")]
        [RegularExpression(@"^(0|[1-9]\d?|100)$", ErrorMessage = "FavorPositionThird must be set between 0 and 100")]
        public int FavorPositionThird { get; set; }

        [MaxLength(2000, ErrorMessage = "Description must be less than 2000 characters!")]
        public string Description { get; set; }
    }

    public class LocationAddModel:LocationBaseModel
    {
        [Required(ErrorMessage = "ProjectId is required!")]
        public int ProjectId { get; set; }
    }

    public class LocationUpdateModel: LocationBaseModel
    {

    }

    public class LocationSubSearchModel : LocationBaseModel
    {
        public int LocationId { get; set; }
        public int? ProjectId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateDate { get; set; }
        public string Modifier { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class LocationSearchModel: LocationSubSearchModel
    {
        public IEnumerable<AlarmSubSearchModel> Alarms { get; set; }
        public IEnumerable<LogSubSearchModel> Logs { get; set; }

        public IEnumerable<GroupSubSearchModel> Groups { get; set; }
    }
}

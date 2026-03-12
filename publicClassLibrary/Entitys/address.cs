using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace publicClassLibrary.Entitys
{
    // ----------------------------
    //  收货地址表 (mall_address)
    // ----------------------------
    [SugarTable("mall_address")]
    public class Address
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int AddressId { get; set; }

        [SugarColumn(ColumnName = "PersonalID", ColumnDescription = "买家ID")]
        public int PersonalID { get; set; }

        [SugarColumn(ColumnName = "AppType", ColumnDescription = "应用类型")]
        public int AppType { get; set; }

        [SugarColumn(ColumnName = "AddressName", ColumnDescription = "地址", Length = 255)]
        public string AddressName { get; set; }

        [SugarColumn(ColumnName = "ReceiverName", ColumnDescription = "收货姓名", Length = 255)]
        public string ReceiverName { get; set; }


        [SugarColumn(ColumnName = "Phone", ColumnDescription = "收货电话", Length = 255)]
        public string Phone { get; set; }

        [SugarColumn(ColumnName = "Province", ColumnDescription = "省", Length = 255)]
        public string Province { get; set; }

        [SugarColumn(ColumnName = "City", ColumnDescription = "市", Length = 255)]
        public string City { get; set; }


        [SugarColumn(ColumnName = "District", ColumnDescription = "区", Length = 255)]
        public string District { get; set; }

        [SugarColumn(ColumnName = "IsDefault", ColumnDescription = "是否默认:1-是 0-否")]
        public int IsDefault { get; set; }


        [SugarColumn(ColumnName = "CreateTime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [SugarColumn(ColumnName = "UpdateTime", ColumnDescription = "更新时间")]
        public DateTime? UpdateTime { get; set; } = null;
    }
}

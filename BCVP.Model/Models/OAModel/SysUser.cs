using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model.Models.OAModel
{
    ///<summary>
    ///系统用户
    ///</summary>
    [SugarTable("TB_Sys_User")]
    public partial class SysUser
    {
        /// <summary>
        /// 主键自增非空
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, OracleSequenceName = "SEQ_Sys_User")]
        public int UserID { get; set; }

        /// <summary>
        /// 机构ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int OrgID { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = false)]
        public string UserName { get; set; }

        /// <summary>
        /// 证件号码，身份证号码可空
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 128, IsNullable = true)]
        public string IDCard { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string UserPassword { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string Mobile { get; set; }

        /// <summary>
        /// 座机或者分机号码
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string TelePhone { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string Photo { get; set; }

        /// <summary>
        /// 学历ID
        /// </summary>
        [SugarColumn(ColumnDataType = "number", Length = 10, IsNullable = true)]
        public int EducationId { get; set; }


        /// <summary>
        /// 学位号码
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string DegreeNo { get; set; }

        /// <summary>
        /// 职称ID
        /// </summary>
        [SugarColumn(ColumnDataType = "number", Length = 10, IsNullable = true)]
        public int ProfessionalId { get; set; }


        /// <summary>
        /// 邮箱地址
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Sex { get; set; } = 0;

        /// <summary>
        /// 姓名拼音码
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string PyCode { get; set; }

        /// <summary>
        /// 状态1、在职，0离职
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int State { get; set; } = 1;

        /// <summary>
        /// 删除状态1、已删除，0未删除
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int IsDelete { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否是驾驶员 1、是，0不是
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int IsDriver { get; set; } = 0;

        /// <summary>
        /// 驾驶员编号
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string DriverNo { get; set; }

        /// <summary>
        /// 手机登录加密
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 655, IsNullable = true)]
        public string Token { get; set; }

        /// <summary>
        /// 设备编号或者登录IP
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string DeviceNo { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 255, IsNullable = true)]
        public string Address { get; set; }


        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime LastErrTime { get; set; } = DateTime.Now;

        /// <summary>
        ///错误次数 
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar2", Length = 500, IsNullable = true)]
        public string Remark { get; set; }

    }
}

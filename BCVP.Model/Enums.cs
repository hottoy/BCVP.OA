using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCVP.Model
{
    public class Enums
    {
        public enum StateFlags
        {
            [Description("禁用")]
            Disabled = 0,
            [Description("启用")]
            Enabled = 1
        }

        public enum Sex
        {
            [Description("男")]
            Male = 1,
            [Description("女")]
            Female = 2
        }

        /// <summary>
        /// 用户权限类别
        /// </summary>
        public enum UserPmsType
        {
            [Description("模块")]
            Module = 1,

            [Description("机构")]
            Org = 2,

            [Description("操作按钮")]
            Operation = 3
        }

        /// <summary>
        /// 类型（0注册，1登录,2重置密码,3禁用）
        /// </summary>
        public enum LoginType
        {
            /// <summary>
            /// 注册
            /// </summary>
            [Description("注册")]
            Register = 0,

            /// <summary>
            /// 登录
            /// </summary>
            [Description("登录")]
            Login = 1,

            /// <summary>
            /// 重置密码
            /// </summary>
            [Description("重置密码")]
            ResetPassword = 2,

            /// <summary>
            /// 禁用
            /// </summary>
            [Description("禁用")]
            Disable = 3
        }

        /// <summary>
        /// 登录来源（0:PC 1:webapp 2:App 3:微信 4:IOS）
        /// </summary>
        public enum LoginSrc
        {
            /// <summary>
            /// 0:PC 
            /// </summary>
            [Description("pc")]
            PC = 0,
            /// <summary>
            /// 1:webapp
            /// </summary>
            [Description("webapp")]
            WebApp = 1,
            /// <summary>
            /// 2:App
            /// </summary>
            [Description("APP")]
            APP = 2,
            /// <summary>
            /// 3:微信
            /// </summary>
            [Description("微信")]
            WeChat = 3,
            /// <summary>
            /// 4:IOS
            /// </summary>
            [Description("IOS")]
            IOS = 4

        }

        /// <summary>
        /// 登录状态（0失败，1成功）
        /// </summary>
        public enum LoginStatus
        {
            /// <summary>
            /// 0:失败
            /// </summary>
            [Description("失败")]
            Fail = 0,
            /// <summary>
            /// 1:成功
            /// </summary>
            [Description("成功")]
            Success = 1
        }

        /// <summary>
        /// 机构属性。
        /// </summary>
        public enum OrgAttribute
        {
            /// <summary>
            /// 机构，指由两个或两个以上构件通过活动联接形成的构件系统。
            /// </summary>
            [Description("机构")]
            Org = 1,
            /// <summary>
            /// 部门，是一个组织的机构。通常一个公司单位会分成很多部门。
            /// </summary>
            [Description("部门")]
            Dept = 2,
            /// <summary>
            /// 分子公司
            /// </summary>
            [Description("分子公司")]
            Factory = 3,
            /// <summary>
            /// 分公司
            /// </summary>
            [Description("分公司")]
            SubCompany = 4,
            /// <summary>
            /// 生产区
            /// </summary>
            [Description("生产区")]
            Produre = 5,
            /// <summary>
            /// 大队
            /// </summary>
            [Description("大队")]
            BigTeam = 6,
            /// <summary>
            /// 科研机构
            /// </summary>
            [Description("科研机构")]
            Scientific = 7
        }
        public enum LogType
        {
            /// <summary>
            /// 系统日志
            /// </summary>
            系统日志 = 1,
            /// <summary>
            /// 异常日志
            /// </summary>
            异常日志 = 2
        }
        public enum PermissionType
        {
            /// <summary>
            /// 菜单
            /// </summary>
            Menu = 1,
            /// <summary>
            /// 按钮
            /// </summary>
            Button = 2,
        }
        public enum ResponseCode
        {
            /// <summary>
            /// 成功
            /// </summary>
            [Description("成功")]
            Success = 0,
            /// <summary>
            /// 失败
            /// </summary>
            [Description("失败")]
            Fail = 1,
        }
    }
}

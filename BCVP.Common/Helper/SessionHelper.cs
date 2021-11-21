using Fireasy.Common.Serialization;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace BCVP.Common.Helper
{
    /// <summary>
    /// Session会话辅助类。
    /// </summary>
    public static class SessionHelper
    {
        private const string SESSION_KEY = "zero_user";

        /// <summary>
        /// 获取用户会话 <see cref="SessionContext"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SessionContext GetSession(this HttpContext context)
        {
            byte[] bytes;
            if (context.Session.TryGetValue(SESSION_KEY, out bytes))
            {
                return new JsonSerializer().Deserialize<SessionContext>(Encoding.UTF8.GetString(bytes));
            }

            return null;
        }

        /// <summary>
        /// 设置当前的用户会话 <see cref="SessionContext"/> 对象。
        /// </summary>
        /// <param name="context"></param>
        /// <param name="session"></param>
        public static void SetSession(this HttpContext context, SessionContext session)
        {
            var json = new JsonSerializer().Serialize(session);
            context.Session.Set(SESSION_KEY, Encoding.UTF8.GetBytes(json));
        }
    }
    /// <summary>
    /// 用户会话信息。
    /// </summary>
    public class SessionContext
    {
        public int uLoginUserId { get; set; }
        public string uLoginUserAccount { get; set; }
        public string IP { get; set; }
        public string uLoginName { get; set; }

        public string uLoginPwd { get; set; }

        public string VCode { get; set; }

        public bool IsMember { get; set; }
    }
}

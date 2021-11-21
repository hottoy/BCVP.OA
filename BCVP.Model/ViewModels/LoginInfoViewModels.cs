namespace BCVP.Model.ViewModels
{
    public class LoginInfoViewModels
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


namespace KindaFilter.Models
{
    public class AddAsChildRequest
    {
        public string UserMail { get; set; }
        public string ChildEmail { get; set; }
        public bool isChild { get; set; }
        public bool RequestWaiting { get; set; }

    }
}

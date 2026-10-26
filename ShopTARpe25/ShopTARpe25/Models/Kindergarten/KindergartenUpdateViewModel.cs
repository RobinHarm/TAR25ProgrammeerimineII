namespace ShopTARpe25.Models.Kindergarten
{
    public class KindergartenUpdateViewModel
    {
        public Guid? Id { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int? ChildrenCount { get; set; }
        public string KinderGartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<KindergartenImageViewModel> Images { get; set; }
           = new List<KindergartenImageViewModel>();


    }
}

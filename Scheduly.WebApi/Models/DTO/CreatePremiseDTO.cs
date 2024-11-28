namespace Scheduly.WebApi.Models.DTO
{
    public class CreatePremiseDTO
    {
        public int PremiseCategoryId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public bool MustBeApproved { get; set; }
    }
}

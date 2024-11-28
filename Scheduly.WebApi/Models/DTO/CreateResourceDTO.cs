namespace Scheduly.WebApi.Models.DTO
{
    public class CreateResourceDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public bool MustBeApproved { get; set; }
    }
}

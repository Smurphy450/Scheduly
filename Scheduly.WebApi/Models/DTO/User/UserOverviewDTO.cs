using Scheduly.WebApi.Models.DTO.Common;

namespace Scheduly.WebApi.Models.DTO.User
{
    public class UserOverviewDTO
    {
        public List<OverviewResourcesDTO> OverviewResources { get; set; }
        public List<OverviewPremisesDTO> OverviewPremises { get; set; }
    }
}

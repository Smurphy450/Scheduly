using System.Collections.Generic;

namespace Scheduly.WebApi.Models.DTO
{
    public class UserOverviewDTO
    {
        public List<OverviewResourcesDTO> OverviewResources { get; set; }
        public List<OverviewPremisesDTO> OverviewPremises { get; set; }
    }
}

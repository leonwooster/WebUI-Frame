using AutoMapper;
using AspCoreFrame.Entities;
using AspCoreFrame.WebUI.Models;

namespace AspCoreFrame.WebUI.Helpers
{
    /// <summary>
    /// A class that setup the automapper mapping configuration.
    /// More detail can be obtained from https://code-maze.com/automapper-net-core/
    /// </summary>
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CreateMap<ExternalQuestionnaireBO, QFormModel>();            
        }
    }
}

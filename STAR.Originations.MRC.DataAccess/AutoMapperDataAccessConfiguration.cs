
using AutoMapper;

using entities = STAR.Originations.MRC.DataAccess;
using contracts = STAR.Originations.MegaRunbook.Contracts.Data;

namespace STAR.Originations.MRC.DataAccess
{
    internal static class AutoMapperDataAccessConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<entities::ApplicationGroup, contracts::ApplicationGroup>();
            Mapper.CreateMap<entities::RunbookTemplate, contracts::RunbookTemplate>();
            Mapper.CreateMap<entities::RunbookStep, contracts::RunbookStep>();
            Mapper.CreateMap<entities::RunbookStepPbi, contracts::RunbookStepPbi>();
            Mapper.CreateMap<entities::RunbookStepType, contracts::RunbookStepType>();
            Mapper.CreateMap<entities::ApplicationGroup, contracts::ApplicationGroup>();
            Mapper.CreateMap<entities::Team, contracts::Team>();
            Mapper.CreateMap<entities::Contact, contracts::Contact>();
        }
    }
}

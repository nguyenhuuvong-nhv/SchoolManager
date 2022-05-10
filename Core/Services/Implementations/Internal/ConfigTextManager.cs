using Common.Constants;
using Data.Entity;
using Entities.UnitOfWork;
using MicroOrm.Dapper.Repositories;
using Services.Helpers;
using Services.Interfaces.Internal;
using System.Linq;

namespace Services.Implementations.Internal
{
    public class ConfigTextManager : IConfigTextManager
    {
        private readonly IUnitOfWork _unitOfWork;

        private IDapperRepository<ConfigText> ConfigValueRepository => _unitOfWork.GetRepository<ConfigText>();

        public ConfigTextManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ConfigText[] GetAllConfig()
        {
            var result = MemoryCacheHelper.GetOrCreate(
                CacheType.CONFIG_TEXT,
                entry =>
                {
                    return ConfigValueRepository.FindAll()?.ToArray();
                });
            return result;
        }

        public ConfigText[] GetConfigValueFromCache() => MemoryCacheHelper.Get<ConfigText[]>(CacheType.CONFIG_TEXT) ?? GetAllConfig().ToArray();

        public ConfigText GetConfigValueByGroupAndValue(string groupName, string value)
        {
            var configList = GetConfigValueFromCache();
            var configGroup = configList.Where(o => o.ConfigGroup == groupName && o.ConfigValue == value);
            return configGroup.FirstOrDefault();
        }
    }
}

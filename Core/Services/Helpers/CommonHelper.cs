using Common.Dependency;
using Services.Dtos.Shared;
using Services.Interfaces.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class CommonHelper
    {
        public static DictionaryItemDto ToDictionaryItemDto<T>(this string value)
        {
            var configText = SingletonDependency<IConfigTextManager>.Instance.GetConfigValueByGroupAndValue(typeof(T).Name, value);
            return value == null
                ? null
                : new DictionaryItemDto
                {
                    Group = configText?.ConfigGroup,
                    Key = configText?.ConfigKey ?? value,
                    Value = value,
                    DisplayText = configText?.DisplayText ?? value,
                    Order = configText?.ConfigOrder
                };
        }

        public static DictionaryItemDto ToDictionaryItemDto(this string value, string groupName)
        {
            var configText = SingletonDependency<IConfigTextManager>.Instance.GetConfigValueByGroupAndValue(groupName, value);
            return value == null
                ? null
                : new DictionaryItemDto
                {
                    Group = configText?.ConfigGroup,
                    Key = configText?.ConfigKey ?? value,
                    Value = value,
                    DisplayText = configText?.DisplayText ?? value,
                    Order = configText?.ConfigOrder
                };
        }

        
    }
}

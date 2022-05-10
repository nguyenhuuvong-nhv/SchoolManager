using Data.Entity;
using Services.Dtos.Shared;
using System.Threading.Tasks;

namespace Services.Interfaces.Internal
{
    public interface IConfigTextManager
    {
        ConfigText GetConfigValueByGroupAndValue(string groupName, string keyValue);
    }
}

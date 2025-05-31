using Buildings;
using Data;
using Interface;

namespace JSON
{
    public class BuildingSaveHandler : JsonFileHandler<SavedBuildingsWrapper>, IService
    {
        protected override string FileName => "buildings.json";
    }
}
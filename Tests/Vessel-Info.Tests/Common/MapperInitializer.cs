namespace Vessel_Info.Tests.Common
{
    using System.Reflection;
    using Vessel_Info.Data.Models;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;

    public static class MapperInitializer
    {
        private static bool initialized = false;

        public static void Initialize()
        {
            if (!initialized)
            {
                AutoMapperConfig.RegisterMappings(
                    typeof(ClassSocietyAllServiceModel).GetTypeInfo().Assembly,
                    typeof(ClassificationSociety).GetTypeInfo().Assembly);

                initialized = true;
            }
        }
    }
}

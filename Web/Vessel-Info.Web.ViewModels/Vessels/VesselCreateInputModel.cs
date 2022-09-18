namespace Vessel_Info.Web.ViewModels.Vessels
{
    using AutoMapper;
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselCreateInputModel : IMapFrom<VesselCreateServiceModel>, IMapTo<VesselCreateServiceModel>, IHaveCustomMappings
    {
        [Display(Name = "Flag")]
        public string RegistrationFlag { get; set; }

        [Display(Name = "Port of Registry")]
        public string RegistrationRegistryPort { get; set; }

        [Display(Name = "Type of Vessel")]
        public string TypeName { get; set; }

        [Display(Name = "Class Society")]
        public string ClassificationSocietyFullName { get; set; }

        [Display(Name = "Owner")]
        public string OwnerName { get; set; }

        public VesselAllViewModel Vessel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<VesselCreateInputModel, VesselCreateServiceModel>()
                .ForMember(destination => destination.Registration,
                    opts => opts.MapFrom(origin => new VesselRegistrationServiceModel
                    {
                        Flag = origin.RegistrationFlag,
                        RegistryPort = origin.RegistrationRegistryPort
                    }))
                .ForMember(destination => destination.Type,
                    opts => opts.MapFrom(origin => new VesselTypeServiceModel
                    {
                        Name = origin.TypeName
                    }))
                .ForMember(destination => destination.ClassificationSociety,
                    opts => opts.MapFrom(origin => new VesselClassificationSocietyServiceModel
                    {
                        FullName = origin.ClassificationSocietyFullName
                    }))
                .ForMember(destination => destination.Owner,
                    opts => opts.MapFrom(origin => new VesselOwnerServiceModel
                    {
                        Name = origin.OwnerName
                    }));
        }
    }
}

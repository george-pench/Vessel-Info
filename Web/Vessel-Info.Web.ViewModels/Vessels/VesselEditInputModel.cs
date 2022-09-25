namespace Vessel_Info.Web.ViewModels.Vessels
{
    using AutoMapper;
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselEditInputModel : IMapFrom<VesselEditServiceModel>, IMapFrom<VesselAllServiceModel>, IHaveCustomMappings
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
                .CreateMap<VesselEditInputModel, VesselEditServiceModel>()
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

            configuration
                .CreateMap<VesselAllServiceModel, VesselEditInputModel>()
                .ForMember(destination => destination.Vessel,
                    opts => opts.MapFrom(origin => new VesselAllServiceModel
                    {
                        Name = origin.Name,
                        Beam = origin.Beam,
                        Built = origin.Built,
                        CallSign = origin.CallSign,
                        Cubic = origin.Cubic,
                        Draft = origin.Draft,
                        HullType = origin.HullType,
                        Imo = origin.Imo,
                        Loa = origin.Loa,
                        SummerDwt = origin.SummerDwt
                    }));
        }
    }
}

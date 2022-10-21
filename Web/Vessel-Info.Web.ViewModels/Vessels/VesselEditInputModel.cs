namespace Vessel_Info.Web.ViewModels.Vessels
{
    using AutoMapper;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Vessel_Info.Services.Mapping;
    using Vessel_Info.Services.Models.ClassSocieties;
    using Vessel_Info.Services.Models.Owners;
    using Vessel_Info.Services.Models.Registrations;
    using Vessel_Info.Services.Models.Types;
    using Vessel_Info.Services.Models.Vessels;

    public class VesselEditInputModel : IMapTo<VesselFormServiceModel>, IMapFrom<VesselAllServiceModel>, IHaveCustomMappings
    {
        [Display(Name = "Flag")]
        public int RegistrationId { get; set; }

        public IEnumerable<RegistrationBaseServiceModel> Registrations { get; set; }

        [Display(Name = "Class Society")]
        public int ClassificationSocietyId { get; set; }

        public IEnumerable<ClassSocietyBaseServiceModel> ClassificationSocieties { get; set; }

        [Display(Name = "Owner")]
        public int OwnerId { get; set; }

        public IEnumerable<OwnerBaseServiceModel> Owners { get; set; }

        [Display(Name = "Type")]
        public int TypeId { get; set; }

        public IEnumerable<TypeBaseServiceModel> Types { get; set; }

        public VesselAllViewModel Vessel { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<VesselAllServiceModel, VesselEditInputModel>()
                .ForMember(destination => destination.Vessel,
                    opts => opts.MapFrom(origin => new VesselAllServiceModel
                    {
                        Id = origin.Id,
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

using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VegaCars.Controllers.Resources;
using VegaCars.Models;

namespace VegaCars.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API Resource
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();
            CreateMap<Vehicle, VehicleResource>()
              .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
              .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));

            // API Resource to Domain
            CreateMap<VehicleResource, Vehicle>()
              .ForMember(v => v.Id, opt => opt.Ignore())
              .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
              .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
              .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
              .ForMember(v => v.Features, opt => opt.Ignore())
              .AfterMap((vr, v) => {
                  // Remove unselected features
                  var removedFeatures = v.Features.Where(f => !vr.Features.Contains(f.FeatureId));
                  foreach (var f in removedFeatures)
                      v.Features.Remove(f);

                  // Add new features
                  var addedFeatures = vr.Features.Where(id => !v.Features.Any(f => f.FeatureId == id)).Select(id => new VehicleFeature { FeatureId = id });
                  foreach (var f in addedFeatures)
                      v.Features.Add(f);
              });


            #region Depricated - Rewritten with Lambda
            //CreateMap<VehicleResource, Vehicle>()
            // .ForMember(v => v.Id, opt => opt.Ignore())
            // .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            // .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
            // .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            // .ForMember(v => v.Features, opt => opt.Ignore())
            // .AfterMap((vr, v) => {
            //     // Remove unselected features
            //     var removedFeature = new List<VehicleFeature>();
            //     foreach (var f in v.Features)
            //         if (!vr.Features.Contains(f.FeatureId))
            //             removedFeature.Add(f);

            //     foreach (var f in removedFeature)
            //         v.Features.Remove(f);

            //     // Add new features
            //     foreach (var id in vr.Features)
            //         if (!v.Features.Any(f => f.FeatureId == id))
            //             v.Features.Add(new VehicleFeature { FeatureId = id });
            // });
            #endregion

            #region Depricated - Remove Mapping Table Values that Already Exist
            //// Domain to API Resource
            //CreateMap<Make, MakeResource>();
            //CreateMap<Model, ModelResource>();
            //CreateMap<Feature, FeatureResource>();
            //CreateMap<Vehicle, VehicleResource>()
            //    .ForMember(vr => vr.Contact, opt => opt.MapFrom(v => new ContactResource { Name = v.ContactName, Email = v.ContactEmail, Phone = v.ContactPhone }))
            //    .ForMember(vr => vr.Features, opt => opt.MapFrom(v => v.Features.Select(vf => vf.FeatureId)));


            //// API Resource to Domain
            //// Since the shapes are difference, for each member in the db, must map where it is coming from in the resource
            //CreateMap<VehicleResource, Vehicle>()
            //    // Ignore Id so not setting primary key on update (when value supplied)
            //    .ForMember(v => v.Id, opt => opt.Ignore())
            //    .ForMember(v => v.ContactName, opt => opt.MapFrom(vr => vr.Contact.Name))
            //    .ForMember(v => v.ContactEmail, opt => opt.MapFrom(vr => vr.Contact.Email))
            //    .ForMember(v => v.ContactPhone, opt => opt.MapFrom(vr => vr.Contact.Phone))
            //    // Causes error on update, bc mapping already occurs so tries to create duplicate primary key in VehicleFeatures table
            //    .ForMember(v => v.Features, opt => opt.MapFrom(vr => vr.Features.Select(id => new VehicleFeature { FeatureId = id })));
            #endregion

        }
    }
}

using Api.Model.Facilities;
using Api.Model.Facilities.Dto;

namespace Api.Mappers
{
    public static class FacilityTypeMapper
    {
         public static FacilityType MapToFacilityType(this CreateFacilityTypeRequest request)
        {
            return new FacilityType(request.Name);
        }
    }
}
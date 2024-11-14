namespace Api.Model.Facilities.Dto
{
    public struct CreateFacilityDto(string name, string location, Guid typeId)
    {
        public string Location { get; set; } = location;
        public string Name { get; set; } = name;
        public Guid FacilityTypeId { get; set; } = typeId;
    }
}
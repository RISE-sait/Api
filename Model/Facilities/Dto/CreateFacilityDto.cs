namespace Api.Model.Facilities.Dto
{
    public struct CreateFacilityDto(string name, string location)
    {
        public string Location { get; set; } = location;
        public string Name { get; set; } = name;
    }
}
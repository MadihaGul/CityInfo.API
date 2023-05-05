using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key] // use this to make primary key if Id is named different than Id ex. cityId
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // explicitly stating that it should be identity. No need for int or Guid
        public int Id { get; set; }

        [Required] // Better to add at lower level so add these type of data annotations to entity as well
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("CityId")] // Not neccessay 
        public City? City { get; set; } // navigation property
        public int CityId { get; set; } // Not required to explicitly do it but preferable, conventionally it will be regarded automatically as CityId (Name of the Class and Id) 
        public PointOfInterest(string name) { Name = name; }
    }
}

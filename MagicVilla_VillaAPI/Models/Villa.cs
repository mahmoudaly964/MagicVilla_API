namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public string ImageUrl { get; set; }
        public int Occupancy { get; set; }
        public string Amenity { get; set; }

        public int Area { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatededDate { get; set; }

    }
}

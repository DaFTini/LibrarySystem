using System.Collections.Generic;

namespace AutoLibraryUI.Entities;

public class Book
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int PublicationYear { get; set; }
    public string PublicationLocation { get; set; }
    public int TotalCount { get; set; }
    public int AvailableCount { get; set; }
    public string Authors { get; set; }
    
    public ICollection<Booking> Bookings { get; private set; }
}
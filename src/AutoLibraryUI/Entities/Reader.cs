using System;
using System.Collections.Generic;

namespace AutoLibraryUI.Entities;

public class Reader
{
    public int Id { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronimic { get; set; }
    public DateTimeOffset RegistrationDateTime { get; set; }
    public ICollection<Booking> Bookings { get; private set; }
    public bool IsDeleted { get; set; } = false;
}
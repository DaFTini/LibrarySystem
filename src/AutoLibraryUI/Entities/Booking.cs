using System;

namespace AutoLibraryUI.Entities;

public class Booking
{
    public int Id { get; private set; }
    public bool IsReturned { get; set; }
    public DateTimeOffset GetDateTime { get; set; }
    public DateTimeOffset ReturnDateTime { get; set; }
    public string StringGetDateTime { get => GetDateTime.ToString("d"); }
    public string StringReturnDateTime { get => ReturnDateTime.ToString("d"); }


    public Reader Reader { get; set; }
    public Book Book { get; set; }
    
    public int ReaderId { get; set; }
    public int BookId { get; set; }
}
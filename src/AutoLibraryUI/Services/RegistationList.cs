using AutoLibraryUI.Data;
using AutoLibraryUI.Entities;
using AutoLibraryUI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace AutoLibraryUI.Services;

public class RegistationList : IAuthService<Librarian>
{
    private readonly PasswordHasher<Librarian> _passwordHasher;
    private readonly LibraryDbContext _libraryDbContext;
    
    public RegistationList(PasswordHasher<Librarian> passwordHasher,
    LibraryDbContext libraryDbContext)
    {
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _libraryDbContext = libraryDbContext ?? throw new ArgumentNullException(nameof(libraryDbContext));
    }
    
    public bool Register(Librarian librarian, string password)
    {
        try
        {
            var newLibrarian = new Librarian
            {
                FullName = librarian.FullName,
                Username = librarian.Username,
                Role = librarian.Role
            };
        
            newLibrarian.PasswordHash = _passwordHasher.HashPassword(newLibrarian, password);

            _libraryDbContext.Librarians.Add(newLibrarian);
            _libraryDbContext.SaveChanges();
        }
        catch (Exception)
        {
            return false;
        }
        
        return true;
    }

    public Librarian? Login(string username, string password)
    {
        var librarian = _libraryDbContext.Librarians
            .FirstOrDefault(x => x.Username == username);

        if (librarian == null)
            return null;
        
        var result = _passwordHasher.VerifyHashedPassword(librarian, librarian.PasswordHash,
            password);
        
        return result == PasswordVerificationResult.Success
            ? librarian 
            : null;
    }

    public Reader GetReaderById(int id)
    {
        var reader = _libraryDbContext.Readers
            .Include(x => x.Bookings)
                .FirstOrDefault(x => x.Id == id) 
            ?? throw new Exception("Читатель не найден в базе данных");

        return reader;
    }

    public Reader CreateReader(string firstName, string lastName, string patronimic)
    {
        var reader = new Reader
        {
            FirstName = firstName,
            LastName = lastName,
            Patronimic = patronimic,
            RegistrationDateTime = DateTimeOffset.UtcNow,
        };

        _libraryDbContext.Readers.Add(reader);
        _libraryDbContext.SaveChanges();

        return reader;
    }

    public Reader EditReaderPersonalData(int id, string firstName, string lastName, string patronimic)
    {
        var reader = GetReaderById(id);

        reader.FirstName = firstName;
        reader.LastName = lastName;
        reader.Patronimic = patronimic;

        _libraryDbContext.Readers.Update(reader);
        _libraryDbContext.SaveChanges();

        return reader;
    }

    public void DeleteReader(int id)
    {
        var reader = GetReaderById(id);

        reader.IsDeleted = true;

        _libraryDbContext.Readers.Update(reader);
        _libraryDbContext.SaveChanges();;
    }

    public IEnumerable<Reader> GetReaders(bool showDeleted = false)
    {
        return _libraryDbContext.Readers
            .Where(x => !x.IsDeleted)
            .ToList();
    }


    public void TakeBook(int readerId, int bookId, DateTime start, DateTime end)
    {
        Booking booking = new Booking()
        {
            ReaderId = readerId,
            BookId = bookId,
            GetDateTime = new DateTimeOffset(start).ToUniversalTime(),
            ReturnDateTime = new DateTimeOffset(end).ToUniversalTime()
        };
        _libraryDbContext.Bookings.Add(booking);
        _libraryDbContext.SaveChanges();

        var book = _libraryDbContext.Books.First(x => x.Id == bookId);
        book.AvailableCount -= 1;
        booking.GetDateTime = DateTimeOffset.UtcNow;
        _libraryDbContext.Books.Update(book);
        _libraryDbContext.SaveChanges();
    }

    public void ReturnBook(int bookingId)
    {
        var booking = _libraryDbContext.Bookings.First(x => x.Id == bookingId);

        if (booking.IsReturned)
        {
            return;
        }

        booking.IsReturned = true;
        booking.ReturnDateTime = DateTimeOffset.UtcNow;
        booking.Book.AvailableCount += 1;
        _libraryDbContext.SaveChanges();

        //var book = _libraryDbContext.Books.First(x => x.Id == booking.Book.Id);
        //book.AvailableCount += 1;
        //_libraryDbContext.Books.Update(book);
        //_libraryDbContext.SaveChanges();
    }

    public IEnumerable<Booking> GetBookings(int id)
    {
        return _libraryDbContext.Bookings
            .Include(x => x.Reader)
            .Include(x => x.Book)
            .Where(x => x.ReaderId == id)
            .OrderBy(x => x.GetDateTime)
            .ToList();
    }
}
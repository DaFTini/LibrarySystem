using AutoLibraryUI.Data;
using AutoLibraryUI.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AutoLibraryUI.Services;

public class SearchOptions
{
    public string Query { get; set; } = string.Empty;
    public int MinDate { get; set; } = 0;
    public int MaxDate { get; set; } = 9999;
    public string Authors { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Isbn { get; set; } = string.Empty;
    public bool HasAvailable { get; set; } = false;
    public string PublicationLocation { get; set; } = string.Empty;

    public static SearchOptions Default { get => new(); }

    public string NormilizedQuery { get => Query.ToUpper().Trim(); }
}

public class CatalogService
{
    private readonly LibraryDbContext _libraryDbContext;

    public CatalogService(LibraryDbContext libraryDbContext)
    {
        _libraryDbContext = libraryDbContext ?? throw new ArgumentNullException(nameof(libraryDbContext));
    }

    public IEnumerable<Book> Search(SearchOptions options)
    {
        return _libraryDbContext.Books
            .AsEnumerable()
            .Where(x => x.AvailableCount > 0 || !options.HasAvailable)
            .Where(x => x.PublicationYear >= options.MinDate && x.PublicationYear <= options.MaxDate)
            .Where(x => string.IsNullOrEmpty(options.Authors) || x.Authors.Trim().ToUpper().Contains(options.Authors.ToUpper().Trim()))
            .Where(x => string.IsNullOrEmpty(options.PublicationLocation) || 
                x.PublicationLocation.Trim().ToUpper().Contains(options.PublicationLocation.ToUpper().Trim()))
            .Where(x => string.IsNullOrEmpty(options.NormilizedQuery) ||
                options.NormilizedQuery.Contains(x.Code.ToString()) ||
                options.NormilizedQuery.Contains(x.PublicationYear.ToString()) ||
                options.NormilizedQuery.Contains(x.Name.Trim().ToUpper().ToString()) ||
                x.Name.Trim().ToUpper().ToString().Contains(options.NormilizedQuery) ||
                x.Authors.Trim().ToUpper().Contains(options.NormilizedQuery) ||
                x.PublicationLocation.Trim().Contains(options.NormilizedQuery)
                )
            .OrderBy(x => x.Code)
            .ToList();
    }
}
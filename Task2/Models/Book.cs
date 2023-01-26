using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class BookDetailsDTO
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Cover { get; set; }
    [Required]
    public string? Content { get; set; }
    public string? Author { get; set; }
    [Required]
    public string? Genre { get; set; }

    public ICollection<ReviewDetailsDTO>? Reviews { get; set; }
    public ICollection<RatingDetailsDTO>? Ratings { get; set; }
}

public class BookDTO
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Author { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public decimal AverageRatingsScore { get; set; } = 0;
}

public class BookContentDTO
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Cover { get; set; }
    [Required]
    public string? Content { get; set; }
    public decimal AverageRatingsScore { get; set; }
    public ReviewDTO[]? Reviews { get; set; }
}

public class BookPutDTO
{
    public int? Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public string? Author { get; set; }
    [Required]
    public string? Genre { get; set; }
    public string? Cover { get; set; }
    [Required]
    public string? Content { get; set; }
}
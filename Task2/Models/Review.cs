using System.ComponentModel.DataAnnotations;

namespace Task2.Models;

public class ReviewDetailsDTO
{
    public int Id { get; set; }
    [Required]
    public string? Message { get; set; }
    public int BookId { get; set; }
    [Required]
    public string? Reviewer { get; set; }
    [Required]
    public BookDetailsDTO? Book { get; set; }
}

public class ReviewDTO
{
    public int Id { get; set; }
    [Required]
    public string? Message { get; set; }
    [Required]
    public string? Reviewer { get; set; }
}
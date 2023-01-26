namespace Task2.Models;

public class RatingDetailsDTO
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public double Score { get; set; }
    public BookDetailsDTO? Book { get; set; }
}

public class RatingDTO
{
    public double Score { get; set; }
}
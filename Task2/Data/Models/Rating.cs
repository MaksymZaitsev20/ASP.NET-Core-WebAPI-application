namespace Task2.Data.DataModels;

public class RatingDetailsDTO
{
    public int Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Id")]
    public int BookId { get; set; }
    public double Score { get; set; }
    public BookDetailDTO Book { get; set; }
}

public class RatingDTO
{
    public double Score { get; set; }
}
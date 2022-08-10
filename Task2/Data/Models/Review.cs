namespace Task2.Data.DataModels;

public class ReviewDetailsDTO
{
    public int Id { get; set; }
    public string Message { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.ForeignKey("Id")]
    public int BookId { get; set; }
    public string Reviewer { get; set; }
    public BookDetailDTO Book { get; set; }
}

public class ReviewDTO
{
    public int Id { get; set; }
    public string Message { get; set; }
    public string Reviewer { get; set; }
}
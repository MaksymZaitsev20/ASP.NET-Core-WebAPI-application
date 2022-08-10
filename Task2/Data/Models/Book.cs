namespace Task2.Data.DataModels;

public class BookDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    // Cover it`s image
    public string? Cover { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }

    public List<ReviewDetailsDTO> Reviews { get; set; } = new();
    public List<RatingDetailsDTO> Ratings { get; set; } = new();
}

public class BookDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public int ReviewsCount { get; set; } = 0;
    public decimal AverageRatingsScore { get; set; } = 0;
}

public class BookContentDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    // Cover it`s image
    public string Cover { get; set; }
    public string Content { get; set; }
    public decimal AverageRatingsScore { get; set; }
    public ReviewDTO[] Reviews { get; set; }
}

public class BookPutDTO
{
    public int? Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
    public string Cover { get; set; }
    public string Content { get; set; }
}
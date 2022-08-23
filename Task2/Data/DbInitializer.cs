using Task2.Models;

namespace Task2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(BookContext context)
        {
            if (context.Books.Any())
                return;

            var books = new BookDetailsDTO[]
            {
                new BookDetailsDTO{Title="Harry Potter", Author="J. K. Rowling", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Fantasy"},
                new BookDetailsDTO{Title="39 clues", Author="Robert Boing", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Adventure novel"},
                new BookDetailsDTO{Title="The Witcher", Author="Andrzej Sapkowski", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Fantasy"},
                new BookDetailsDTO{Title="Adventures of Sherlock Holmes", Author="Arthur Conan Doyle", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Advetures"},
                new BookDetailsDTO{Title="Dreamcatcher", Author="Stephen King", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Horror"},
                new BookDetailsDTO{Title="Don Quixote", Author="Miguel de Cervantes", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Novel"},
                new BookDetailsDTO{Title="The Great Gatsby", Author="F. Scott Fitzgerald", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Fiction"},
                new BookDetailsDTO{Title="Katerina", Author="Taras Shevchenko", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="Novel"},
                new BookDetailsDTO{Title="The Gates of Europe", Author="Serhii Plokhy", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="History"},
                new BookDetailsDTO{Title="Death and the Penguin", Author="Andrey Kurkov", Content="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Cover="cover", Genre="History"}
            };

            context.Books.AddRange(books);
            context.SaveChanges();


            var reviews = new ReviewDetailsDTO[]
            {
                new ReviewDetailsDTO{Message="Good book!", BookId=1, Reviewer="John Perry"},
                new ReviewDetailsDTO{Message="Terrible...", BookId=4, Reviewer="Tom Soyer"},
                new ReviewDetailsDTO{Message="So so...", BookId=2, Reviewer="Mike Tomson"},
                new ReviewDetailsDTO{Message="Not bad)", BookId=3, Reviewer="Alisa Wonder"},
                new ReviewDetailsDTO{Message="Very interesting book!", BookId=4, Reviewer="Diana Fia"},
                new ReviewDetailsDTO{Message="Emmmm...", BookId=2, Reviewer="Kind Man"},
                new ReviewDetailsDTO{Message="In my opinion, very nice", BookId=2, Reviewer="Striker Gamer"},
                new ReviewDetailsDTO{Message="What are you talking about, author?", BookId=5, Reviewer="Single Man"},
                new ReviewDetailsDTO{Message="Cool", BookId=6, Reviewer="Kitana"},
                new ReviewDetailsDTO{Message="Boring", BookId=7, Reviewer="Valentin Budeyko"},
                new ReviewDetailsDTO{Message="Clear", BookId=8, Reviewer="Cameron Diaz"},
                new ReviewDetailsDTO{Message="What you mean...", BookId=9, Reviewer="Aleksandra Daddario"},
                new ReviewDetailsDTO{Message="Can be better", BookId=1, Reviewer="Johnny Depp"},
                new ReviewDetailsDTO{Message="I want to read it again", BookId=7, Reviewer="Angelina Jolie"},
                new ReviewDetailsDTO{Message="Strange emotions", BookId=1, Reviewer="Brad Pitt"},
                new ReviewDetailsDTO{Message="I didn`t understand the essence of the content", BookId=8, Reviewer="Leonardo DiCaprio"}
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();


            var ratings = new RatingDetailsDTO[]
            {
                new RatingDetailsDTO{Score=7.8, BookId=1},
                new RatingDetailsDTO{Score=5.8, BookId=2},
                new RatingDetailsDTO{Score=8.3, BookId=1},
                new RatingDetailsDTO{Score=9.9, BookId=3},
                new RatingDetailsDTO{Score=4.3, BookId=4},
                new RatingDetailsDTO{Score=9.0, BookId=5},
                new RatingDetailsDTO{Score=9.4, BookId=1},
                new RatingDetailsDTO{Score=5.5, BookId=2},
                new RatingDetailsDTO{Score=5.2, BookId=9},
                new RatingDetailsDTO{Score=9.0, BookId=8},
                new RatingDetailsDTO{Score=1.1, BookId=6},
                new RatingDetailsDTO{Score=9.9, BookId=7},
                new RatingDetailsDTO{Score=10.0, BookId=8},
                new RatingDetailsDTO{Score=8.8, BookId=7},
                new RatingDetailsDTO{Score=9.7, BookId=5}
            };

            context.AddRange(ratings);
            context.SaveChanges();
        }
    }
}
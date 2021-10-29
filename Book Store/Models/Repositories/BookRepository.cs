using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
         {
             new Book
             {
                    Id=1,
                    Title ="C# Programming",
                    Description ="No description",
                    Author=new Author
                    {
                        Id=2
                    },
                    ImageUrl="a.jpg"
                    
             },
             new Book
                {
                    Id=2,
                    Title ="Java Programming",
                    Description ="Nothing",
                    Author=new Author(),
                     ImageUrl="b.jpg"
             },
             new Book
             {
                    Id=3,
                    Title ="Python Programming",
                    Description ="No data",
                    Author=new Author(),
                     ImageUrl="b.jpg"
             },
             
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id==id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            var result= books.Where(a => a.Title.Contains(term)
            ||a.Description.Contains(term)
            ||a.Author.FullName.Contains(term)).ToList();
            return result;
        }

        public void Update(int id, Book entity)
        {
            var book = Find(id);

            book.Title = entity.Title;
            book.Description = entity.Description;
            book.Author = entity.Author;
            book.ImageUrl = entity.ImageUrl;
        }
    }
}

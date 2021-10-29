using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models.Repositories
{
    public class BookDBRepository : IBookStoreRepository<Book>
    {
        BookStoreDbcontext db;
        public BookDBRepository(BookStoreDbcontext _db)
        {
            db = _db;
        }
        public void Add(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
        }

        public Book Find(int id)
        {
            var book = db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return db.Books.Include(a=>a.Author).ToList();
        }

        public List<Book> Search(string term)
        {
            var result = db.Books.Where(a => a.Title.Contains(term)
            || a.Description.Contains(term)
            || a.Author.FullName.Contains(term)).ToList();
            return result;
        }

        public void Update(int id, Book entity)
        {
            db.Update(entity);
            db.SaveChanges();
        }
    }
}

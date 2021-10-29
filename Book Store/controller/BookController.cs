using Book_Store.Models.ViewModel;
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.controller
{
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookRepository, IBookStoreRepository<Author> authorRepository, IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            //string FileName = string.Empty;
            //if (model.File != null)
            //{
                //string Uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                //FileName = file.FileName;
                //string FullPath = Path.Combine(Uploads, FileName);
                //file.CopyTo(new FileStream(FullPath, FileMode.Create));
            //}
            string FileName=UploadFile(model.File) ??string.Empty;
           
            if(ModelState.IsValid)
            {
                try
                {
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please Select an Author From List";
                        
                        return View(GetAllAuthors());
                    }
                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl=FileName
                       
                    };
                    bookRepository.Add(book);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

            ModelState.AddModelError("", "you have to fill all required methods");
            return View(GetAllAuthors());
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;
            var model = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Authors = authorRepository.List().ToList(),
                ImageUrl=book.ImageUrl
            };
            return View(model);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel model)
        {
            try
            {
                string FileName = UploadFile(model.File, model.ImageUrl); 
                //string FileName = string.Empty;
                //if (model.File != null)
                //{
                //    string Uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                //    FileName = model.File.FileName;
                //    string FullPath = Path.Combine(Uploads, FileName);
                //    //Delete The Oldfile
                //    // string OldFile = bookRepository.Find(model.BookId).ImageUrl;
                //    string OldFile = model.ImageUrl;
                //    string OldPath = Path.Combine(Uploads, OldFile);
                //    if (FullPath != OldFile)
                //    {
                //        System.IO.File.Delete(OldPath);
                //        //Save the Newfile
                //        model.File.CopyTo(new FileStream(FullPath, FileMode.Create));
                //    }
                //}
                var author = authorRepository.Find(model.AuthorId);
                Book book = new Book
                {
                    Id = model.BookId,
                    Title = model.Title,
                    Description = model.Description,
                    Author = author,
                    ImageUrl=FileName
                };
                bookRepository.Update(model.BookId, book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Search(string term)
        {
           var book= bookRepository.Search(term);
            return View("Index", book);
        }
       List<Author>FillSelectList()
        { 
           var Authors = authorRepository.List().ToList();
            Authors.Insert(0, new Author { Id = -1, FullName = "Select Author From List" });
            return Authors;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            var viewmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return viewmodel;

        }
        string UploadFile(IFormFile file)
        {
            string FileName = string.Empty;
            if (file != null)
            {
                string Uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                FileName = file.FileName;
                string FullPath = Path.Combine(Uploads, FileName);
                file.CopyTo(new FileStream(FullPath, FileMode.Create));
                return FileName;
            }
            return null;
        }
        string UploadFile(IFormFile file,string ImageUrl)
        {
            string FileName = string.Empty;
            if (file != null)
            {
                string Uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                FileName = file.FileName;
                string FullPath = Path.Combine(Uploads, FileName);
                //Delete The Oldfile
                // string OldFile = bookRepository.Find(model.BookId).ImageUrl;
                string OldFile = ImageUrl;
                string OldPath = Path.Combine(Uploads, OldFile);
                if (FullPath != OldFile)
                {
                    System.IO.File.Delete(OldPath);
                    //Save the Newfile
                    file.CopyTo(new FileStream(FullPath, FileMode.Create));
                }
                return FileName;
            }
            return ImageUrl;
        }
    }
}

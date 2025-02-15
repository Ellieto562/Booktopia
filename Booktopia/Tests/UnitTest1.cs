using Booktopia.Controllers;
using Booktopia.Data;
using Booktopia.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    public class UnitTest1
    {
        public class BookControllerTests
        {
            private Mock<ApplicationDbContext> GetDbContext()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "BooktopiaTestDb")
                    .Options;

                var mockContext = new Mock<ApplicationDbContext>(options);
                return mockContext;
            }

            [Fact]
            public void Add_ValidBook_ShouldRedirectToIndex()
            {
                var mockContext = GetDbContext();
                var controller = new BookController(mockContext.Object);
                var book = new Book { Title = "Test Book", Author = "Author", Genre = "Fiction" };

                var result = controller.Add(book) as RedirectToActionResult;

                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            }

            [Fact]
            public void Delete_NonExistingBook_ShouldReturnNotFound()
            {
                var mockContext = GetDbContext();
                var controller = new BookController(mockContext.Object);

                var result = controller.Delete(999);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class ReadingListControllerTests
        {
            private Mock<ApplicationDbContext> GetDbContext()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "BooktopiaTestDb")
                    .Options;

                var mockContext = new Mock<ApplicationDbContext>(options);
                return mockContext;
            }

            [Fact]
            public void Create_ValidReadingList_ShouldRedirectToIndex()
            {
                var mockContext = GetDbContext();
                var controller = new ReadingListController(mockContext.Object);

                var result = controller.Create("My List") as RedirectToActionResult;

                Assert.NotNull(result);
                Assert.Equal("Index", result.ActionName);
            }

            [Fact]
            public void Delete_NonExistingReadingList_ShouldReturnNotFound()
            {
                var mockContext = GetDbContext();
                var controller = new ReadingListController(mockContext.Object);

                var result = controller.Delete(999);

                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
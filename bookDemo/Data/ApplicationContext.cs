using bookDemo.Models;

namespace bookDemo.Data
{
    public static class ApplicationContext
    {
        public static List<Book> Books { get; set; }
        static ApplicationContext()
        {
            Books = new List<Book>()
               {
                   new Book(){Id=1, Title="Game Of Thrones", Price=5},
                   new Book(){Id=2,Title="Hunters", Price=8},
                   new Book(){Id=3,Title="Princess", Price = 10}
               };
        }
    }
}

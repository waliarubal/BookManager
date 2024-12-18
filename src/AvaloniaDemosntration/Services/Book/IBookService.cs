using AvaloniaDemosntration.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvaloniaDemosntration.Services;
public interface IBookService
{
    Task<bool> Save(BookModel book);
    Task<bool> Remove(BookModel book);
    Task<IList<BookModel>> GetAll();
}

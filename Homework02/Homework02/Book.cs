using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework02
{
    public class Book
    {
        public string Title { get; set; }

        public string Genre { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }
    }
}

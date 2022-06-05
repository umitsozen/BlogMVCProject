using Blog.Entities.Concrete;
using Blog.Shared.Entities.Abstract;
using Blog.Shared.Utilities.Results.ComplexType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities.DTOs
{
    public class ArticleDto:DtoGetBase
    {
        public Article Article{ get; set; }      
    }
}

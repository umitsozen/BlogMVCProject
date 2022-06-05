using AutoMapper;
using Blog.Entities.Concrete;
using Blog.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.AutoMapper.Profiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>()
                .ForMember(dest => dest.CreatedDate, opt =>opt.MapFrom(d=> DateTime.Now)); 
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(d => DateTime.Now));
        }
    }
}

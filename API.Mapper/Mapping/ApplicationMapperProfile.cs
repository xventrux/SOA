using API.Contracts.Book;
using API.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Mapper.Mapping
{
    public class ApplicationMapperProfile : Profile
    {
        public ApplicationMapperProfile()
        {
            CreateMap<Book, BookCreateDto>();
            CreateMap<BookCreateDto, Book>().ForMember(i => i.CreationDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(i => i.UpdateDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(i => i.isDelete, opt => opt.MapFrom(src => false))
                .ForMember(i => i.Images, opt => opt.MapFrom(src => new List<MyFile>()));
        }
    }
}

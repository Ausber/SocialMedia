using AutoMapper;
using SocialMedia.Core.DTOs;
using SocialMedia.Core.Entities;

namespace SocialMedia.Infraestructure.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDto>();
            CreateMap<PostDto, Post>();
        }
    }
}

using SocialMedia.Core.QueryFilters;
using System;

namespace SocialMedia.Infraestructure.Interfaces
{
    public interface IUriService
    {
        Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl);
    }
}
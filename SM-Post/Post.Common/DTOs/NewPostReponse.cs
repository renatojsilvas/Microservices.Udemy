using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Post.Common.DTOs
{
    public class NewPostResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
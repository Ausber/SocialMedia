using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.Execptions
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }
    }
}

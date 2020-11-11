using CoinApp.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoinApp.Infrastructure
{
    public class WorkContext : IWorkContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;
        private string _basePath;

        public WorkContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId
        {
            get
            {
                if (string.IsNullOrEmpty(_userId))
                {
                    _userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value ?? "B40D7680-D8C8-4473-A3B5-ADF0FC045B90";
                }

                return _userId;
            }
        }

        public string BasePath
        {
            get
            {
                if (string.IsNullOrEmpty(_basePath))
                {
                    _basePath = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "basepath")?.Value ?? @"C:/CoinApp";

                }

                return _basePath;
            }
        }

    }
}


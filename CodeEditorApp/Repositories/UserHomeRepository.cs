using CodeEditorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeEditorApp.Repositories
{
    
    public class UserHomeRepository
    {
        private ApplicationDbContext _db;

        public UserHomeRepository()
        {
            _db = new ApplicationDbContext();
        }
    }
}
using CodeEditorApp.Models;
using CodeEditorApp.Models.Entities;
using CodeEditorApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CodeEditorApp.Repositories
{
    public class TreeRepository
    {
        private ApplicationDbContext _db;

        public TreeRepository()
        {
            _db = new ApplicationDbContext();
        }
        public List<FolderViewModel> GetAllSubFolders(int folderID)
        {
            List<Folder> tmpFolders = _db.Folders.Where(x => x.HeadFolderID == folderID).ToList();
            List<FolderViewModel> returnList = new List<FolderViewModel>();

            if (tmpFolders != null)
            {
                foreach (Folder folderItem in tmpFolders)
                {
                    returnList.Add(new FolderViewModel
                    {
                        ID = folderItem.ID,
                        Name = folderItem.Name,
                        ProjectID = folderItem.ProjectID,
                        HeadFolderID = folderItem.HeadFolderID,
                        IsSolutionFolder = folderItem.IsSolutionFolder,
                        Files = GetAllSubFiles(folderItem.ID),
                        SubFolders = GetAllSubFolders(folderItem.ID)
                    });
                }
                return returnList;
            }

            return null;
        }


        public List<FileViewModel> GetAllSubFiles(int FolderID)
        {
            List<File> fileList = _db.Files.Where(x => x.HeadFolderID == FolderID).ToList();
            List<FileViewModel> returnList = new List<FileViewModel>();

            if (fileList != null)
            {
                foreach (File fileItem in fileList)
                {
                    returnList.Add(new FileViewModel()
                    {
                        ID = fileItem.ID,
                        Name = fileItem.name,
                        HeadFolderID = fileItem.HeadFolderID,
                        ProjectID = fileItem.ProjectID,
                        FileType = fileItem.FileType
                    });
                }
                return returnList;
            }
            return null;
        }

        public List<SelectListItem> GetFileTypes()
        {
            List<SelectListItem> ReturnList = new List<SelectListItem>();

            ReturnList.Add(new SelectListItem() { Value = "", Text = "- Select Project Type -" });

            foreach (FileType item in _db.FileTypes.ToList())
            {
                ReturnList.Add(new SelectListItem()
                {
                    Value = item.ID.ToString(),
                    Text = item.Name,
                });
            }
            return ReturnList;
        }
    }
}
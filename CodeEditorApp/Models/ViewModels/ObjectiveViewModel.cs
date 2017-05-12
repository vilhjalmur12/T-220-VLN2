namespace CodeEditorApp.Models.ViewModels
{
    public class ObjectiveViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Finished { get; set; }
        public int GoalID { get; set; }
        public int ProjectID { get; set; }
        public string AspNetUserID { get; set; }
    }
}
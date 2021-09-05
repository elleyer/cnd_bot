using System.ComponentModel.DataAnnotations;
using CndBot.Core.Actions;

namespace CndBot.Core
{
    public class FormDataModel
    {
        [Key]
        public virtual int Id { get; set; }

        public string Name { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        
        public FormType FormType { get; set; }
        
        public long UserId { get; set; }
        
        public int Age { get; set; }
        
        public bool Approved { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class ActionItem
    {
        [Key]
        public int ActionItemId { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionTime { get; set; }

        // Foreign keys
        public int ItemId { get; set; }

        // Navigation properties
        public virtual Item Item { get; set; }
    }
}

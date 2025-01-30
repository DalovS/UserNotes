using Microsoft.AspNetCore.Identity;

namespace UserNotes.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }// връзка с потребителя от Identity
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IdentityUser User { get; set; }
        public ICollection<NoteTag> NoteTags { get; set; }
    }
}

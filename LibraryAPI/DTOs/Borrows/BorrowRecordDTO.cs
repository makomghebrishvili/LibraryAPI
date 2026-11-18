namespace LibraryAPI.DTOs.Borrows
{
    public class BorrowRecordDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTime BorrowedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsOverdue => ReturnedAt == null && DueDate < DateTime.UtcNow;
    }
}

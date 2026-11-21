namespace Chat.Shared;

 public class MessageDto
    {
        public string? Nickname { get; set; }
        public string? Text { get; set; }
    }
    public class Message
    {
        public string? Nickname { get; set; }
        public string? Text { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
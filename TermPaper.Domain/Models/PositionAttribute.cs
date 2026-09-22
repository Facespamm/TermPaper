    namespace TermPaper.Domain.Models;

    public class PositionAttribute
    {
        public int Id { get; set; }
        
        public int PositionId { get; set; }
        
        public int AttributeId { get; set; }

        public Position Position { get; set; } = null!;

        public Attributes Attributes { get; set; } = null!;
        
        public int Order { get; set; }
    }
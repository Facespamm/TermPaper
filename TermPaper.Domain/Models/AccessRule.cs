using TermPaper.Enum;

namespace TermPaper.Domain.Models;

public class AccessRule
{
    public int Id { get; set; }
    
    public int AttributeId { get; set; }
    
    public int PositionId  { get; set; }
    
    public Operator Operator { get; set; }
    
    public string Value { get; set; }
}
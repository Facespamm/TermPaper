namespace TermPaper.Application.Dto.UsersAtributesDto;

public class GetUserAttributeDto
{
    public int Id {get; set;}
    
    public int AtributeId {get; set;}
    
    public string UserId  {get; set;}
    
    public string Value {get; set;}
    
    public DateTime LastUsedAt {get; set;}
    
}
namespace TermPaper.Application.Dto.UsersAtributesDto;

public class UpdateUserAttributeDto
{
    public int Id {get; set;}
    
    public int? AtributeId {get; set;}
    
    public string? Value {get; set;}
}
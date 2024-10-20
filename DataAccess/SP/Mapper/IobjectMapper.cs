using DTOs;

namespace DataAccess.SP.Mapper
{
    public interface IObjectMapper
    {
        BaseDto BuildObject(Dictionary<string, object> objectRow);
        List<BaseDto> BuildObjects(List<Dictionary<string, object>> objectRows);
    }
}

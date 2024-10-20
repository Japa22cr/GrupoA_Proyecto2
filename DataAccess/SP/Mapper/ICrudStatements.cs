using DataAccess.SP.DAO;
using DTOs;

namespace DataAccess.SP.Mapper
{
    public interface ICrudStatements
    {
        SqlOperation GetCreateStatement(BaseDto entityDTO);
        SqlOperation GetUpdateStatement(BaseDto entityDTO);
        SqlOperation GetDeleteStatement(BaseDto entityDTO);
        SqlOperation GetRetrieveAllStatement();
        SqlOperation GetRetrieveByIdStatement(int Id);
    }
}

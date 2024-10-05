using DataAccess.Dao;
using DTOs;

namespace DataAccess.Mapper
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

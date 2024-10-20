using DataAccess.SP.DAO;
using DTOs;

namespace DataAccess.SP.CRUD
{
    public abstract class CrudFactory
    {
        protected SqlDao dao;
        public abstract void Create(BaseDto entityDTO);
        public abstract List<T> RetrieveAll<T>();
        public abstract void Update(BaseDto entityDTO);
        public abstract void Delete(BaseDto entityDTO);
        public abstract BaseDto RetrieveById(int id);
    }
}
using DataAccess.Dao;

using DTOs;


namespace DataAccess.Mapper
{
    public class ExerciseMapper : ICrudStatements, IObjectMapper
    {
        public BaseDto BuildObject(Dictionary<string, object> objectRow)
        {
            var exercise = new ExerciseDto
            {
                Description = (string)objectRow["Descripcion"],
                Difficulty = (string)objectRow["Dificultad"],
                Duration = (int)objectRow["Duracion"],
                Name = (string)objectRow["Nombre"],
                Id = (int)objectRow["Id"],
                Repetitions = (int)objectRow["Repeticiones"],
                Type = (string)objectRow["Tipo"],
            };

            return exercise;
        }

        public List<BaseDto> BuildObjects(List<Dictionary<string, object>> objectRows)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetCreateStatement(BaseDto entityDTO)
        {
            SqlOperation operation = new SqlOperation();
            operation.ProcedureName = "dbo.sp_agregarejercicio";

            ExerciseDto exercise = (ExerciseDto)entityDTO;

            operation.AddVarcharParam("Nombre", exercise.Name);
            operation.AddVarcharParam("Descripcion", exercise.Description);
            operation.AddVarcharParam("Tipo", exercise.Type);
            operation.AddIntegerParam("Duracion", exercise.Duration);
            operation.AddIntegerParam("Repeticiones", exercise.Repetitions);
            operation.AddVarcharParam("Dificultad", exercise.Difficulty);

            return operation;
        }

        public SqlOperation GetDeleteStatement(BaseDto entityDTO)
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetrieveAllStatement()
        {
            throw new NotImplementedException();
        }

        public SqlOperation GetRetrieveByIdStatement(int Id)
        {
            SqlOperation operation = new SqlOperation();
            operation.ProcedureName = "dbo.sp_obtenerEjercicioPorId";

            operation.AddIntegerParam("Id", Id);

            return operation;
        }

        public SqlOperation GetUpdateStatement(BaseDto entityDTO)
        {
            throw new NotImplementedException();
        }
    }
}

//using DataAccess.SP.DAO;
//using DataAccess.SP.Mapper;
//using DTOs;

//namespace DataAccess.SP.CRUD
//{
//    public class ExerciseCrudFactory : CrudFactory
//    {
//        private ExerciseMapper mapper;

//        public ExerciseCrudFactory() : base()
//        {
//            mapper = new ExerciseMapper();
//            dao = SqlDao.GetInstance();
//        }

//        public override void Create(BaseDto entityDTO)
//        {
//            SqlOperation operation = mapper.GetCreateStatement(entityDTO);
//            dao.ExecuteStoredProcedure(operation);
//        }
//        public override void Update(BaseDto entityDTO)
//        {
//            throw new NotImplementedException();
//        }
//        public override void Delete(BaseDto entityDTO)
//        {
//            throw new NotImplementedException();
//        }
//        public override List<T> RetrieveAll<T>()
//        {
//            SqlOperation operation = mapper.GetRetrieveAllStatement();

//            List<Dictionary<string, object>> result = dao.ExecuteStoredProcedureWithQuery(operation);

//            List<BaseDto> mappedExercises = mapper.BuildObjects(result);

//            List<T> exerciseList = new List<T>();

//            foreach (var exercise in mappedExercises)
//            {
//                var convertedExercise = (T)Convert.ChangeType(exercise, typeof(T));
//                exerciseList.Add(convertedExercise);
//            }

//            return exerciseList;
//        }
//        public List<T> RetrieveByType<T>(string muscle)
//        {
//            throw new NotImplementedException();
//        }


//        public override BaseDto RetrieveById(int id)
//        {
//            SqlOperation operation = mapper.GetRetrieveByIdStatement(id);

//            Dictionary<string, object> result = dao.ExecuteStoredProcedureWithUniqueResult(operation);
//            var Exercise = mapper.BuildObject(result);

//            return Exercise;

//        }
//    }
//}

using DataAccess;
using DataAccess.CRUD;
using DTOs;

namespace BL
{
    public class ExerciseManager
    {
        public void AddExercise(ExerciseDto exercise)
        {
            ExerciseCrudFactory exerciseCrud = new ExerciseCrudFactory();
            exerciseCrud.Create(exercise);
        }

        public ExerciseDto GetExerciseById(int id)
        {
            ExerciseCrudFactory exerciseCrud = new ExerciseCrudFactory();
            return (ExerciseDto)exerciseCrud.RetrieveById(id);
        }

        public List<ExerciseDto> GetAllExercise()
        {
            ExerciseCrudFactory exerciseCrud = new ExerciseCrudFactory();
            return exerciseCrud.RetrieveAll<ExerciseDto>();
        }

    }
}

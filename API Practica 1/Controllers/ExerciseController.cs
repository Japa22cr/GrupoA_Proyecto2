using BL;
using DTOs;
using Microsoft.AspNetCore.Mvc;


namespace API_Practica_1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExerciseController : Controller
    {
        [HttpPost]
        public IActionResult AddExercise(ExerciseDto exercise)
        {
            try
            {

                ExerciseManager em = new ExerciseManager();
                em.AddExercise(exercise);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }

        }

        [HttpGet]
        public ActionResult<ExerciseDto> GetExerciseById(int id)
        {
            try
            {

                ExerciseManager em = new ExerciseManager();
                var exercise = em.GetExerciseById(id);
                if (exercise == null)
                {
                    return NotFound();
                }
                return Ok(exercise);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]

        public ActionResult<ExerciseDto> GetAllExercise()
        {
            try
            {

                ExerciseManager em = new ExerciseManager();
                var exercise = em.GetAllExercise();
                if (exercise == null)
                {
                    return NotFound();
                }
                return Ok(exercise);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

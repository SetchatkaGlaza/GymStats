using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymStats.Models
{
    // --- СПРАВОЧНИКИ ---
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty; // Грудь, Спина, Ноги...
        public string Equipment { get; set; } = string.Empty;   // Штанга, Гантели...
        public bool IsCustom { get; set; } = false; // true - если создал сам
    }

    // --- ШАБЛОНЫ ---
    public class WorkoutTemplate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class TemplateExercise
    {
        [Key]
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int ExerciseId { get; set; }
        public int Order { get; set; } // Порядок выполнения
    }

    // --- ФАКТ ТРЕНИРОВОК ---
    public class Workout
    {
        [Key]
        public int Id { get; set; }
        public int? TemplateId { get; set; } // Nullable, если тренировались без шаблона
        public DateTime Date { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
    }

    public class WorkoutExercise
    {
        [Key]
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
        public int Order { get; set; }

        // ДОБАВЬ ЭТУ СТРОКУ:
        public Exercise Exercise { get; set; }
    }

    public class Set // Подход
    {
        [Key]
        public int Id { get; set; }
        public int WorkoutExerciseId { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int RPE { get; set; } // 1-10
        public string Tempo { get; set; } = string.Empty; // Например, "3-1-1-0"
        public int Order { get; set; }
    }

    // --- АНТРОПОМЕТРИЯ И МЕДИА ---
    public class BodyMeasurement
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public double Weight { get; set; }
        public double Chest { get; set; }
        public double Waist { get; set; }
        public double Hips { get; set; }
        public double Biceps { get; set; }
        public double Thigh { get; set; }
        public double Calf { get; set; }
    }

    public class MediaEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty; // "Photo" или "Video"
    }
}
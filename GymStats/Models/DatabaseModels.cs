using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace GymStats.Models
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string MuscleGroup { get; set; } = string.Empty;
        public string Equipment { get; set; } = string.Empty;
        public bool IsCustom { get; set; } = false;
    }

    public class WorkoutTemplate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<TemplateExercise> TemplateExercises { get; set; } = new List<TemplateExercise>();
    }

    public class TemplateExercise
    {
        [Key]
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int ExerciseId { get; set; }
        public int Order { get; set; }
        public int TargetSets { get; set; } = 3;
        public int TargetReps { get; set; } = 10;
        public double TargetWeight { get; set; } = 0;
        public int TargetRPE { get; set; } = 7;
        public string TargetTempo { get; set; } = string.Empty;
        public WorkoutTemplate Template { get; set; } = null!;
        public Exercise Exercise { get; set; } = null!;
    }

    public class Workout
    {
        [Key]
        public int Id { get; set; }
        public int? TemplateId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Name { get; set; } = "Тренировка";
        public string Notes { get; set; } = string.Empty;
        
        // Навигационное свойство
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }

    public class WorkoutExercise
    {
        [Key]
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public int ExerciseId { get; set; }
        public int Order { get; set; }
        
        // Навигационные свойства (ИСПРАВЛЕНО)
        public Workout Workout { get; set; } = null!; 
        public Exercise Exercise { get; set; } = null!;
        public ICollection<Set> Sets { get; set; } = new List<Set>();
    }

    public class Set
    {
        [Key]
        public int Id { get; set; }
        public int WorkoutExerciseId { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int RPE { get; set; }
        public string Tempo { get; set; } = string.Empty;
        public int Order { get; set; }

        public WorkoutExercise WorkoutExercise { get; set; } = null!;
    }

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
        public double Neck { get; set; }
    }

    public class MediaEntry
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string FilePath { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MediaType { get; set; } = string.Empty;
    }
}
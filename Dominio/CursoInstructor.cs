using System;
using System.Security.Cryptography.X509Certificates;
namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoId {get; set;}
        public int InstructorId {get; set;}
        
        public Curso Curso {get; set;}
        public Instructor Instructor {get; set;}
    }
}
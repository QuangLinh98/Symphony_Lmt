using LModels.ExModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using LModels;

namespace Course_Overview.ViewModel
{
    public class EX_SubjectVM: AbstractModel<EX_Subject>
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
       
    }

    public class ModelSaveRequestSubmit
    {
        public List<Lession> Lession { get; set; }
        public string SubjectName { get; set; }
    }
    public class Lession
    {
        public string No { get; set; }
        public string LessonName { get; set; }
    }

    public class LessionEdit
    {
        public int Id { get; set; }
        public string LessonNumber { get; set; }
        public string LessonName { get; set; }
    }


    public class ModelSaveRequestEdit
    {
        public int SubjectID { get; set; }
        public List<LessionEdit> Lession { get; set; }
        public string SubjectName { get; set; }
    }


    public class EX_QuestionVM : AbstractModel<EX_Question>
    {
        public string QuestionText { get; set; }
        public int SubjectID { get; set; }
        public int LessionID { get; set; }

    }

    public class ModelQuestionSave
    {
        public List<AnswerSave> AnswerSave { get; set; }
        public int LessionIdAdd { get; set; }
        public string QuestionText { get; set; }
        public int SubjectIdAdd { get; set; }
    }
    public class AnswerSave
    {
        public string Answer { get; set; }
        
    }

    public class EX_ExamsVM : AbstractModel<EX_Exam>
    {
        public string ExamName { get; set; }
        public int SubjectID { get; set; }

    }




}

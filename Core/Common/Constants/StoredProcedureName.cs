namespace Common.Constants
{
    public class StoredProcedureName
    {
        #region Schools

        public static readonly string FILTER_SCHOOL = "API_Schools_Filter";

        #endregion

        #region Grades 
        public static readonly string GET_GRADE_BY_ID = "API_Grade_GetById";
        public static readonly string FILTER_GRADE = "API_Grades_Filter";

        #endregion

        #region Students

        public static readonly string GET_STUDENT_BY_ID = "API_Student_GetById";

        public static readonly string FILTER_STUDENT = "API_Students_Filter";

        #endregion
    }
}

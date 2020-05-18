using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ObjectComparer.Tests
{
    [TestClass]
    public class ComparerFixture
    {

        [TestMethod]
        public void Null_values_are_similar_test()
        {
            string first = null, second = null;
            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void Premitive_values_are_similar_test()
        {
            float first = 5.2f,
                second = 5.2f;
            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void PremitiveArray_values_are_similar_test()
        {
            double[] first = null,
                     second = null;
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new double[] { };
            second = new double[] { };
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new double[] { 7, 8, 5, 6, };
            second = new double[] { 5, 7, 6, 8 };
            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void PremitiveList_values_are_similar_test()
        {
            List<double> first = null,
                         second = null;
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new List<double>();
            second = new List<double>();
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new List<double> { 5, 6, 7, 8 };
            second = new List<double> { 5, 6, 7, 8 };
            Assert.IsTrue(Comparer.AreSimilar(first, second));


        }

        [TestMethod]
        public void Struct_values_are_similar_test()
        {
            Student firstStudent = new Student { Id = 100, Name = "John", MarksArray = new[] { 80, 90, 100 } };
            Student secondStudent = new Student { Id = 100, Name = "John", MarksArray = new[] { 100, 80, 90 } };
            KeyValuePair<int, Student> first = new KeyValuePair<int, Student>(100, firstStudent),
                second = new KeyValuePair<int, Student>(100, secondStudent);


            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void Struct_values_are_not_similar_test()
        {
            Student firstStudent = new Student { Id = 100, Name = "John", MarksArray = new[] { 80, 90, 100 } };
            Student secondStudent = new Student { Id = 101, Name = "John", MarksArray = new[] { 80, 90, 100 } };
            KeyValuePair<int, Student> first = new KeyValuePair<int, Student>(100, firstStudent),
                second = new KeyValuePair<int, Student>(100, secondStudent);


            Assert.IsFalse(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void ClassObjArray_values_are_similar_test()
        {
            SubjectGradesstr[] first = null,
                                second = null;

            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new SubjectGradesstr[] { };
            second = new SubjectGradesstr[] { };

            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new SubjectGradesstr[] { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" },null };
            second = new SubjectGradesstr[] { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" },null };

            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void ClassObjList_values_are_similar_test()
        {
            List<double> first = null,
                            second = null;
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new List<double>();
            second = new List<double>();
            Assert.IsTrue(Comparer.AreSimilar(first, second));

            first = new List<double> { 5, 6, 7, 8 };
            second = new List<double> { 5, 6, 7, 8 };
            Assert.IsTrue(Comparer.AreSimilar(first, second));
        }

        [TestMethod]
        public void ClassObject_are_similar_test()
        {
            Student firstStudent = null;
            Student secondStudent = null;
            Assert.IsTrue(Comparer.AreSimilar(firstStudent, secondStudent));

            firstStudent = new Student();
            secondStudent = new Student();
            Assert.IsTrue(Comparer.AreSimilar(firstStudent, secondStudent));

            firstStudent = new Student
            {
                Id = 100,
                Name = "Kanad Mehta",
                MarksArray = new int[] { 75, 80, 56, 90 },
                MarksList = new List<int> { 75, 80, 56, 90 },
                GradesArray = new SubjectGradesstr[] { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" }, },
                GradesStrEnumDic = new Dictionary<string, GradeEnum>(),
                GradesStrClassObjDic = new Dictionary<string, SubjectGrades>(),
                GradesClassObj = new List<SubjectGradesstr> { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" }, },
            };
            firstStudent.GradesStrEnumDic.Add("P", GradeEnum.Ap);
            firstStudent.GradesStrEnumDic.Add("C", GradeEnum.B);
            firstStudent.GradesStrEnumDic.Add("M", GradeEnum.D);

            firstStudent.GradesStrClassObjDic.Add("P", new SubjectGrades { Grade = GradeEnum.Ap });
            firstStudent.GradesStrClassObjDic.Add("C", new SubjectGrades { Grade = GradeEnum.B });
            firstStudent.GradesStrClassObjDic.Add("M", new SubjectGrades { Grade = GradeEnum.D });

            secondStudent = new Student
            {
                Id = 100,
                Name = "Kanad Mehta",
                MarksArray = new int[] { 75, 80, 56, 90 },
                MarksList = new List<int> { 75, 80, 56, 90 },
                GradesArray = new SubjectGradesstr[] { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" }, },
                GradesStrEnumDic = new Dictionary<string, GradeEnum>(),
                GradesStrClassObjDic = new Dictionary<string, SubjectGrades>(),
                GradesClassObj = new List<SubjectGradesstr> { new SubjectGradesstr { Grade = "A" }, new SubjectGradesstr { Grade = "B" }, },
            };
            secondStudent.GradesStrEnumDic.Add("P", GradeEnum.Ap);
            secondStudent.GradesStrEnumDic.Add("C", GradeEnum.B);
            secondStudent.GradesStrEnumDic.Add("M", GradeEnum.D);

            secondStudent.GradesStrClassObjDic.Add("P", new SubjectGrades { Grade = GradeEnum.Ap });
            secondStudent.GradesStrClassObjDic.Add("C", new SubjectGrades { Grade = GradeEnum.B });
            secondStudent.GradesStrClassObjDic.Add("M", new SubjectGrades { Grade = GradeEnum.D });
            Assert.IsTrue(Comparer.AreSimilar(firstStudent, secondStudent));
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] MarksArray { get; set; }
        public List<int> MarksList { get; set; }
        public SubjectGradesstr[] GradesArray { get; set; }
        public Dictionary<string, GradeEnum> GradesStrEnumDic { get; set; }
        public Dictionary<string, SubjectGrades> GradesStrClassObjDic { get; set; }
        public List<SubjectGradesstr> GradesClassObj { get; set; }
    }

    public class SubjectGradesstr
    {
        public string Grade { get; set; }
    }
    public class SubjectGrades
    {
        public GradeEnum Grade { get; set; }
    }

    public enum GradeEnum
    {
        Ap, A, Bp, B, C, D
    }

}

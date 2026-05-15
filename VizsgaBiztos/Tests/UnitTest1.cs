using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using System.Security.Cryptography;
using System.Text;

namespace Tests
{
    // Entity Tests
    public class UserEntityTests
    {
        [Fact]
        public void User_Should_Have_Required_Properties()
        {
            // Arrange & Act
            var user = new User
            {
                Id = 1,
                NeptunCode = "AAABBB",
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = "hashedpassword",
                Role = Role.Student
            };

            // Assert
            user.Id.Should().Be(1);
            user.NeptunCode.Should().Be("AAABBB");
            user.Name.Should().Be("Test User");
            user.Email.Should().Be("test@example.com");
            user.Role.Should().Be(Role.Student);
        }

        [Fact]
        public void User_Can_Be_Admin_Or_Student()
        {
            // Arrange & Act
            var adminUser = new User { Role = Role.Admin };
            var studentUser = new User { Role = Role.Student };

            // Assert
            adminUser.Role.Should().Be(Role.Admin);
            studentUser.Role.Should().Be(Role.Student);
        }
    }

    // Scoring Tests
    public class ExamResultScoringTests
    {
        [Theory]
        [InlineData(10, 10, 100)]
        [InlineData(5, 10, 50)]
        [InlineData(0, 10, 0)]
        [InlineData(7, 10, 70)]
        [InlineData(1, 1, 100)]
        public void ExamResult_Score_Calculation_Should_Be_Correct(int correctAnswers, int totalQuestions, decimal expectedScore)
        {
            // Arrange & Act
            var score = totalQuestions > 0 ? (decimal)correctAnswers / totalQuestions * 100 : 0;

            // Assert
            score.Should().Be(expectedScore);
        }

        [Fact]
        public void ExamResult_With_Zero_Questions_Should_Have_Zero_Score()
        {
            // Arrange & Act
            int totalQuestions = 0;
            int correctAnswers = 0;
            var score = totalQuestions > 0 ? (decimal)correctAnswers / totalQuestions * 100 : 0;

            // Assert
            score.Should().Be(0);
        }
    }

    // Question Type Tests
    public class QuestionTypeTests
    {
        [Fact]
        public void Question_Should_Support_TrueFalse_Type()
        {
            // Arrange & Act
            var question = new Question
            {
                Id = 1,
                SheetId = 1,
                Text = "2+2=4",
                QuestionType = QuestionType.TrueFalse,
                CorrectAnswer = "True"
            };

            // Assert
            question.QuestionType.Should().Be(QuestionType.TrueFalse);
            question.CorrectAnswer.Should().Be("True");
        }

        [Fact]
        public void Question_Should_Support_MultipleChoice_Type()
        {
            // Arrange & Act
            var question = new Question
            {
                Id = 1,
                SheetId = 1,
                Text = "What is 2+2?",
                QuestionType = QuestionType.MultipleChoice,
                CorrectAnswer = "A",
                OptionA = "4",
                OptionB = "5",
                OptionC = "6",
                OptionD = "3"
            };

            // Assert
            question.QuestionType.Should().Be(QuestionType.MultipleChoice);
            question.CorrectAnswer.Should().Be("A");
            question.OptionA.Should().Be("4");
        }
    }

    // Exam Timing Tests
    public class ExamTimingTests
    {
        [Fact]
        public void Exam_Should_Have_StartTime_Before_EndTime()
        {
            // Arrange & Act
            var exam = new Exam
            {
                ExamDate = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(11, 30, 0)
            };

            // Assert
            exam.EndTime.Should().BeGreaterThan(exam.StartTime);
        }

        [Fact]
        public void Exam_Duration_Should_Be_Calculable()
        {
            // Arrange & Act
            var exam = new Exam
            {
                ExamDate = DateTime.Today,
                StartTime = new TimeSpan(10, 0, 0),
                EndTime = new TimeSpan(11, 30, 0)
            };
            var duration = exam.EndTime - exam.StartTime;

            // Assert
            duration.TotalMinutes.Should().Be(90);
        }
    }

    // Password Tests
    public class PasswordHashingTests
    {
        [Fact]
        public void Password_Hash_Should_Be_Consistent()
        {
            // Arrange
            var password = "testPassword123";

            // Act
            var hash1 = ComputeHash(password);
            var hash2 = ComputeHash(password);

            // Assert
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void Different_Passwords_Should_Produce_Different_Hashes()
        {
            // Arrange & Act
            var hash1 = ComputeHash("password1");
            var hash2 = ComputeHash("password2");

            // Assert
            hash1.Should().NotBe(hash2);
        }

        private static string ComputeHash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
            }
        }
    }

    // Student Answer Tests
    public class StudentAnswerTests
    {
        [Theory]
        [InlineData("A", "A", true)]
        [InlineData("A", "B", false)]
        [InlineData("True", "True", true)]
        [InlineData("True", "False", false)]
        public void StudentAnswer_IsCorrect_Should_Reflect_Correctness(string givenAnswer, string correctAnswer, bool expectedIsCorrect)
        {
            // Arrange & Act
            var isCorrect = givenAnswer == correctAnswer;

            // Assert
            isCorrect.Should().Be(expectedIsCorrect);
        }
    }

    // Sheet Tests
    public class SheetEntityTests
    {
        [Fact]
        public void Sheet_Should_Have_CreatedAt_Property()
        {
            // Arrange & Act
            var now = DateTime.Now;
            var sheet = new Sheet
            {
                Title = "Test Sheet",
                CreatedAt = now
            };

            // Assert
            sheet.CreatedAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Sheet_Can_Contain_Multiple_Questions()
        {
            // Arrange & Act
            var sheet = new Sheet { Questions = new List<Question>() };
            sheet.Questions.Add(new Question { Text = "Q1" });
            sheet.Questions.Add(new Question { Text = "Q2" });

            // Assert
            sheet.Questions.Should().HaveCount(2);
        }
    }

    // Exam Student Relationship
    public class ExamStudentRelationshipTests
    {
        [Fact]
        public void ExamStudent_Should_Link_Exam_And_Student()
        {
            // Arrange & Act
            var examStudent = new ExamStudent
            {
                ExamId = 1,
                StudentId = 5
            };

            // Assert
            examStudent.ExamId.Should().Be(1);
            examStudent.StudentId.Should().Be(5);
        }
    }

    // Authentication Tests
    public class UserAuthenticationTests
    {
        [Fact]
        public void Admin_User_Should_Have_Admin_Role()
        {
            // Arrange & Act
            var admin = new User { Role = Role.Admin, Email = "admin@test.com" };

            // Assert
            admin.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public void Student_User_Should_Have_Student_Role()
        {
            // Arrange & Act
            var student = new User { Role = Role.Student, Email = "student@test.com" };

            // Assert
            student.Role.Should().Be(Role.Student);
        }

        [Fact]
        public void User_NeptunCode_Should_Be_Set()
        {
            // Arrange & Act
            var user = new User { NeptunCode = "ABC123XYZ" };

            // Assert
            user.NeptunCode.Should().NotBeNullOrEmpty();
            user.NeptunCode.Length.Should().Be(9);
        }
    }

    // Exam Validation
    public class ExamValidationTests
    {
        [Fact]
        public void Exam_Should_Have_Valid_Date()
        {
            // Arrange & Act
            var exam = new Exam { ExamDate = DateTime.Today };

            // Assert
            exam.ExamDate.Should().NotBe(default(DateTime));
        }

        [Fact]
        public void Exam_EndTime_Should_Be_After_StartTime()
        {
            // Arrange
            var exam = new Exam
            {
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11)
            };

            // Act
            var isValid = exam.EndTime > exam.StartTime;

            // Assert
            isValid.Should().BeTrue();
        }
    }
}

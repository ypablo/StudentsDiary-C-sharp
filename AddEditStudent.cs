using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private string _filePath = System.IO.Path.Combine(Environment.CurrentDirectory, "students3.txt");

        private int _studentID;
        private Student _student;

        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);
        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentID = id;
            GetStudentData();
            tbName.Select();
        }

        private void GetStudentData()
        {
            if (_studentID != 0)
            {
                Text = "Edit student's info";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentID);

                if (_student == null)
                    throw new Exception("Brak uzytkownika o podanym Id.");

                FillTextBoxes();

            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbName.Text = _student.FirstName;
            tbSurename.Text = _student.SureName;
            tbMath.Text = _student.Math;
            tbPhysics.Text = _student.Physics;
            tbTechnology.Text = _student.Technology;
            tbEnglishLang.Text = _student.EnglishLang;
            tbFrenchLang.Text = _student.FrenchLang;
            rtbComments.Text = _student.Comments;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if(_studentID != 0)
            {
                students.RemoveAll(x => x.Id == _studentID);
            }
            else
            {
                AssignIdToNewStudent(students);
            }

            var student = new Student
            {
                Id = _studentID,
                FirstName = tbName.Text,
                SureName = tbSurename.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                EnglishLang = tbEnglishLang.Text,
                Physics = tbPhysics.Text,
                FrenchLang = tbFrenchLang.Text
            };

            students.Add(student);
            _fileHelper.SerializeToFile(students);
            Close();
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            if (studentWithHighestId == null)
            {
                _studentID = 1;
            }
            else
            {
                _studentID = studentWithHighestId.Id + 1;
            }
        }
    }
}

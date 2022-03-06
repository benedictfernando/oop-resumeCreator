using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;
using Gehtsoft.PDFFlow;
using Gehtsoft.PDFFlow.Builder;
using Gehtsoft.PDFFlow.Models.Enumerations;
using Gehtsoft.PDFFlow.Utils;
using HorizontalAlignment = Gehtsoft.PDFFlow.Models.Enumerations.HorizontalAlignment;

namespace oop_resumeCreator
{
    public partial class resume : Form
    {
        public resume()
        {
            InitializeComponent();
        }

        // declare an empty person
        Person person;

        private void load_Click(object sender, EventArgs e)
        {
            // note: you can change 'json' value accdg. to pathfile of your own .json file
            string json = @"C:\Users\Benedict Fernando\Downloads\FERNANDO_BENEDICT.json";
            
            // load .json file to global 'person' variable @line 17
            LoadJson(json);

            // enable and disable buttons
            this.load.Enabled = false;
            this.generate.Enabled = true;
        }

        public void LoadJson(string file)
        {
            // deserialize JSON directly from .JSON file
            using (StreamReader json = File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                person = (Person) serializer.Deserialize(json, typeof(Person));
            }
        }
        private void generate_Click(object sender, EventArgs e)
        {
            // setup a savefiledialog
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save Resume";
            saveFileDialog1.DefaultExt = "pdf";
            saveFileDialog1.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.generate.Enabled = false;
                string filename = saveFileDialog1.FileName;
                createResume(filename);

                // finalize success flow
                MessageBox.Show("Resume successfully saved!");
                Application.Exit();
            }            
        }

        private void createResume(string filename)
        {
            // create a document builder
            DocumentBuilder resume = DocumentBuilder.New();
            var headerSize = 18;

            // customize paper settings
            var resumeBuilder = resume.AddSection()
                .SetMargins(50).SetSize(PaperSize.Letter)
                .SetOrientation(PageOrientation.Portrait)
                .SetStyleFont(Fonts.Courier(11));

            // For topmost details
            resumeBuilder.AddParagraph($"{person.firstname} {person.lastname}")
                .SetBold().SetAlignment(HorizontalAlignment.Center).SetFontSize(20);
            resumeBuilder.AddParagraph("Full Stack Web Developer")
                .SetAlignment(HorizontalAlignment.Center).SetMarginBottom(10);
            
            // For skills
            resumeBuilder.AddParagraph("Skills:").SetBold()
                .SetMarginTop(18).SetMarginBottom(5).SetFontSize(headerSize);
            foreach (var skill in person.skills)
                resumeBuilder.AddParagraph(skill)
                    .SetListBulleted(ListBullet.Bullet, 0);

            // For techstacks
            resumeBuilder.AddParagraph("Technology Stacks:").SetBold()
                .SetMarginTop(18).SetMarginBottom(5).SetFontSize(headerSize);
            foreach (var tech in person.techstacks)
                resumeBuilder.AddParagraph(tech)
                    .SetListBulleted(ListBullet.Bullet, 0);

            // For experiences
            resumeBuilder.AddParagraph("Experiences:").SetBold()
                .SetMarginTop(18).SetMarginBottom(5).SetFontSize(headerSize);
            foreach (var experience in person.experiences)
            {
                Experience exp = JsonConvert
                    .DeserializeObject<Experience>(experience.ToString());
                resumeBuilder.AddParagraph(exp.role)
                    .SetMarginBottom(2).SetListBulleted(ListBullet.Dash, 0);
                resumeBuilder.AddParagraph($"{exp.company} | {exp.date}");
            }

            // For personal
            resumeBuilder.AddParagraph("Details:").SetBold()
                .SetMarginTop(18).SetMarginBottom(5).SetFontSize(headerSize);
            resumeBuilder.AddParagraph($"Location: {person.location}");
            resumeBuilder.AddParagraph($"Email: {person.email}");
            resumeBuilder.AddParagraph($"Number: {person.number}");

            // build file
            resume.Build(filename);
        }

        public class Person
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string[] skills { get; set; }
            public string[] techstacks { get; set; }
            public object[] experiences { get; set; }
            public string location { get; set; }
            public string email { get; set; }
            public string number { get; set; }
        }

        public class Experience
        {
            public string role { get; set; }
            public string company { get; set; }
            public string date { get; set; }
        }
    }
}

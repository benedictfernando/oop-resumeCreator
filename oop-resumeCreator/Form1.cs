using Newtonsoft.Json;
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
            
            LoadJson(json);
        }

        public void LoadJson(string file)
        {
            // deserialize JSON directly from a file
            using (StreamReader json = File.OpenText(file))
            {
                JsonSerializer serializer = new JsonSerializer();
                person = (Person) serializer.Deserialize(json, typeof(Person));
            }
        }

        public class Person
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string[] skills { get; set; }
            public object[] experiences { get; set; }
            public object personal { get; set; }
        }

        public class Experience
        {
            public string role { get; set; }
            public string company { get; set; }
            public string date { get; set; }
        }

        public class Personal
        {
            public string location { get; set; }
            public string email { get; set; }
            public string number { get; set; }
        }
    }
}

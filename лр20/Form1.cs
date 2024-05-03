using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Xml.Serialization; 
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace лр20
{
    public partial class Form1 : Form
    {
        // Список для хранения информации о самолетах
        private List<Plane> planes = new List<Plane>();
        // Список для хранения информации о членах экипажа
        private List<CrewMember> crewMembers = new List<CrewMember>();

        public Form1()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия на кнопку "Добавить самолет"
        private void button1_Click(object sender, EventArgs e)
        {
            // Создание нового объекта самолета и заполнение его данными из формы
            Plane plane = new Plane
            {
                Type = comboBox1.Text,
                Model = comboBox2.Text,
                PassengerCapacity = (int)numericUpDown4.Value,
                YearOfManufacture = dateTimePicker2.Value.Year,
                Payload = Convert.ToDouble(textBox3.Text),
                LastMaintenanceDate = dateTimePicker1.Value.Date // Используем только дату без времени
            };

            // Добавление самолета в список
            planes.Add(plane);

            // Очистка полей формы для самолета
            ClearPlaneFields();
        }

        // Обработчик события нажатия на кнопку "Добавить члена экипажа"
        private void button2_Click(object sender, EventArgs e)
        {
            // Создание нового объекта члена экипажа и заполнение его данными из формы
            CrewMember crewMember = new CrewMember
            {
                LastName = textBox4.Text,
                FirstName = textBox5.Text,
                MiddleName = textBox6.Text,
                Position = comboBox3.Text,
                BirthDate = monthCalendar1.SelectionStart,
                Experience = (int)numericUpDown1.Value,
                Gender = radioButton1.Checked ? "Мужской" : "Женский",
                CrewId = (int)numericUpDown2.Value
            };

            // Добавление члена экипажа в список
            crewMembers.Add(crewMember);

            // Очистка полей формы для члена экипажа
            ClearCrewMemberFields();
        }

        // Обработчик события нажатия на кнопку "Сохранить в XML"
        private void button3_Click(object sender, EventArgs e)
        {
            // Сериализация списка самолетов в XML и сохранение в файл
            SerializeToXml(planes, "planes.xml");

            // Вывод сообщения об успешном сохранении
            MessageBox.Show("Данные сохранены в файле planes.xml", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события нажатия на кнопку "Сохранить в JSON"
        private void button4_Click(object sender, EventArgs e)
        {
            // Сериализация списка самолетов в JSON и сохранение в файл
            SerializeToJson(planes, "planes.json");

            // Вывод сообщения об успешном сохранении
            MessageBox.Show("Данные сохранены в файле planes.json", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события нажатия на кнопку "Чтение из XML"
        private void button5_Click(object sender, EventArgs e)
        {
            // Десериализация списка самолетов из XML файла
            List<Plane> loadedPlanes = DeserializeFromXml<List<Plane>>("planes.xml");

            // Очистка элементов ListBox
            listBox1.Items.Clear();

            // Добавление информации о каждом самолете в ListBox
            foreach (var plane in loadedPlanes)
            {
                listBox1.Items.Add($"Тип: {plane.Type}");
                listBox1.Items.Add($"Модель: {plane.Model}");
                listBox1.Items.Add($"Вместимость пассажиров: {plane.PassengerCapacity}");
                listBox1.Items.Add($"Год выпуска: {plane.YearOfManufacture}");
                listBox1.Items.Add($"Грузоподъемность: {plane.Payload}");
                listBox1.Items.Add($"Дата последнего тех. обслуживания: {plane.LastMaintenanceDate}");
                listBox1.Items.Add("------------------------------"); // Разделитель между самолетами
            }

            // Вывод сообщения о успешном чтении
            MessageBox.Show("Данные успешно загружены из файла planes.xml", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события нажатия на кнопку "Чтение из JSON"
        private void button6_Click(object sender, EventArgs e)
        {
            // Десериализация списка самолетов из JSON файла
            List<Plane> loadedPlanes = DeserializeFromJson<List<Plane>>("planes.json");

            // Очистка элементов ListBox
            listBox1.Items.Clear();

            // Добавление информации о каждом самолете в ListBox
            foreach (var plane in loadedPlanes)
            {
                listBox1.Items.Add($"Тип: {plane.Type}");
                listBox1.Items.Add($"Модель: {plane.Model}");
                listBox1.Items.Add($"Вместимость пассажиров: {plane.PassengerCapacity}");
                listBox1.Items.Add($"Год выпуска: {plane.YearOfManufacture}");
                listBox1.Items.Add($"Грузоподъемность: {plane.Payload}");
                listBox1.Items.Add($"Дата последнего тех. обслуживания: {plane.LastMaintenanceDate}");
                listBox1.Items.Add("------------------------------"); // Разделитель между самолетами
            }

            // Вывод сообщения о успешном чтении
            MessageBox.Show("Данные успешно загружены из файла planes.json", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Обработчик события нажатия на кнопку "Очистить"
        private void button7_Click(object sender, EventArgs e)
        {
            // Очистка всех полей формы
            ClearAllFields();
        }

        // Метод для сериализации объекта в XML файл
        private void SerializeToXml<T>(T data, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, data);
            }
        }

        // Метод для десериализации объекта из XML файла
        private T DeserializeFromXml<T>(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(fileName))
            {
                T result = (T)serializer.Deserialize(reader);
                return result;
            }
        }

        // Метод для сериализации объекта в JSON файл
        private void SerializeToJson<T>(T data, string fileName)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(fileName, json);
        }

        // Метод для десериализации объекта из JSON файла
        private T DeserializeFromJson<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }

        // Метод для очистки всех полей формы для самолета
        private void ClearPlaneFields()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            numericUpDown4.Value = 0;
            textBox3.Clear();
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker1.Value = DateTime.Now;
        }

        // Метод для очистки всех полей формы для члена экипажа
        private void ClearCrewMemberFields()
        {
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            comboBox3.SelectedIndex = -1;
            monthCalendar1.SelectionStart = DateTime.Now;
            numericUpDown1.Value = 0;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            numericUpDown2.Value = 0;
        }

        // Метод для очистки всех полей формы
        private void ClearAllFields()
        {
            ClearPlaneFields();
            ClearCrewMemberFields();
        }
    }

    // Класс для хранения информации о самолете
    public class Plane
    {
        public string Type { get; set; } // Тип самолета
        public string Model { get; set; } // Модель самолета
        public int PassengerCapacity { get; set; } // Вместимость пассажиров
        public int YearOfManufacture { get; set; } // Год выпуска
        public double Payload { get; set; } // Грузоподъемность
        public DateTime LastMaintenanceDate { get; set; } // Дата последнего тех. обслуживания
    }

    // Класс для хранения информации о члене экипажа
    public class CrewMember
    {
        public string LastName { get; set; } // Фамилия
        public string FirstName { get; set; } // Имя
        public string MiddleName { get; set; } // Отчество
        public string Position { get; set; } // Должность
        public DateTime BirthDate { get; set; } // Дата рождения
        public int Experience { get; set; } // Стаж
        public string Gender { get; set; } // Пол
        public int CrewId { get; set; } // ID экипажа
    }
}

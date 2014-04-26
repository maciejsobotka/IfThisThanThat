using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace ITTT_Final
{
    class Serialization
    {
        private Form1 myForm;
        public Serialization(Form1 myForm)
        {
            this.myForm = myForm;
        }
        public void Serialize(List<Task> list)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<Task>));
            FileStream file;
            file = File.Create("Data.dat");
            ser.Serialize(file, list);
            file.Close();
            myForm.UpdateInfoBox("Serializacja powiodłą się");
            Logs.Info("Serializacja powiodłą się");
        }
        public List<Task> DeSerialize()
        {
            List<Task> list = new List<Task>();
            Task tmp = new Task();
            XmlSerializer ser = new XmlSerializer(typeof(List<Task>));
            try 
            {
                FileStream file = File.OpenRead("Data.dat");
                list = (List<Task>)ser.Deserialize(file);
                file.Close();
                myForm.UpdateInfoBox("DeSerializacja powiodłą się");
                Logs.Info("DeSerializacja powiodłą się");

            }catch(FileNotFoundException)
            {
                myForm.UpdateInfoBox("Nie znaleziono pliku z serializowanymi danymi");
                Logs.Error("Nie znaleziono pliku z serializowanymi danymi");
            }
            return list;
        }
    }
}

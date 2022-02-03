using Balizas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Balizas
{
    public partial class Form1 : Form
    {
        List<Baliza> balizas;
        public Form1()
        {
            InitializeComponent();
           
        }
        private void showStations() {
            Communication communication = new Communication();
           // List<Baliza> balizas = communication.GetBalizas();
            foreach (Baliza baliza in balizas)
            {
                listBox1.Items.Add(baliza.name);
            }
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Mapa map = new Mapa(balizas);
            map.Show();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Carga los datos de las balizas si estos no estan en la base de datos
            Database database = new Database();
            Communication communication = new Communication();
            balizas = database.GetBalizas();
            if (balizas.Count() == 0)
            {
                balizas = communication.GetBalizas();
                database.InsertAll(balizas);
            }
            prepareDatabase();
            database.deleteAllReadings();
            communication.getAllReadings(balizas);
            showStations();
            
        }

        private void prepareDatabase()
        {
            Communication communication = new Communication();
            DateTime date = DateTime.Now;
            Baliza baliza = new Baliza();
            baliza.id = "C016";
            Debug.WriteLine("Baliza" + baliza.id);
            communication.GetReadings(date, baliza.id);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Cambiado");
            Database database = new Database();
            Communication communication = new Communication();
            String balizaName = listBox1.GetItemText(listBox1.SelectedItem);
            DateTime date = DateTime.Now;
            foreach (Baliza baliza in balizas)
            {

                Debug.WriteLine("nombre de la baliza: " + baliza.name + " "+ balizaName);
                if (baliza.name.Equals(balizaName))
                {
                    
                    //communication.GetReadings(date,baliza.id);
                    
                    List<Reading> readings= database.GetReadings();
                    foreach(Reading read in readings)
                    {
                        if(read.BalizaID == baliza.id)
                        {
                           // Debug.WriteLine("Size " + readings.Count());
                          //  Reading reading = readings[0];
                            Debug.WriteLine("ftenperature: " + read.temperature);
                            Debug.WriteLine("fhumidity: " + read.humidity);
                            Debug.WriteLine("firradiance: " + read.irradiance);
                            Debug.WriteLine("fprecipitation: " + read.precipitation);
                            tempLabel.Text = read.temperature + "ºC";
                            humLabel.Text = read.humidity + "%";
                            precLabel.Text = read.precipitation + "";
                            irLabel.Text = read.irradiance + "";
                        }
                    }    
                  
                }
            }
            
        }

        private void humLabel_Click(object sender, EventArgs e)
        {

        }
    }
}

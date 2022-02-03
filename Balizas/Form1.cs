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
        Communication communication;
        Database database;
        List<Baliza> balizas;
        
        public Form1()
        {
            InitializeComponent();
           
        }
        private void showStations() {

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
            database = new Database();
            communication = new Communication();
            balizas = database.GetBalizas();
            if (balizas.Count() == 0)
            {
                balizas = communication.GetBalizas();
                database.InsertAll(balizas);
            }
           
            database.deleteAllReadings();
            communication.getAllReadings(balizas);
            showStations();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("Cambiado");
            
            
            String balizaName = listBox1.GetItemText(listBox1.SelectedItem);
            DateTime date = DateTime.Now;
            foreach (Baliza baliza in balizas)
            {

                Debug.WriteLine("nombre de la baliza: " + baliza.name + " "+ balizaName);
                if (baliza.name.Equals(balizaName))
                {
                    
                    List<Reading> readings= database.GetReadings();
                    foreach(Reading read in readings)
                    {
                        if(read.BalizaID == baliza.id)
                        {
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

        private void timer1_Tick(object sender, EventArgs e)
        {

            communication.getAllReadings(balizas);
        }
    }
}

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
        
        public Form1()
        {
            InitializeComponent();
            showStations();
        }
        private void showStations() {
            Communication communication = new Communication();
            List<Baliza> balizas = communication.GetBalizas();
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
            Communication communication = new Communication();
            List<Baliza> balizas = communication.GetBalizas();
            Mapa map = new Mapa(balizas);
            map.Show();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Communication communication = new Communication();
            List<Baliza> balizas = communication.GetBalizas();
            Database database = new Database();
            database.InsertAll(balizas);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Database database = new Database();
            //database.Connect();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Communication communication = new Communication();
            DateTime date = DateTime.Now;
            Baliza baliza = new Baliza();
            baliza.id = "C054";
            Debug.WriteLine("Baliza" + baliza.id);
            communication.GetReadings(date,baliza);
        }
    }
}

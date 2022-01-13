using Balizas.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        
    }
}

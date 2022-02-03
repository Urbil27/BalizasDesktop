using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Balizas.Models;
using System.Collections.Generic;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace Balizas
{
    public partial class Mapa : Form
    {
        public List<Baliza> balizas;

        public Mapa(List<Baliza> balizas)
        {
            InitializeComponent();
            this.balizas = balizas;
        }
       

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(0, 0);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Position = new GMap.NET.PointLatLng(43.033104, -2.520273);
            gMapControl1.Zoom = 9;
            gMapControl1.Zoom = gMapControl1.Zoom-1;

            gMapControl1.AutoScroll = true;
            foreach(Baliza b in balizas)
            {
                GMapOverlay markersOverlay = new GMapOverlay("markers");
                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(b.y, b.x),
                  GMarkerGoogleType.green);
                markersOverlay.Markers.Add(marker);
                gMapControl1.Overlays.Add(markersOverlay);
            }
        }

        private void Mapa_Load(object sender, EventArgs e)
        {

        }
    }
}

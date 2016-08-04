﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZkManagement.Entidades;
using ZkManagement.Logica;

namespace ZkManagement.Interfaz
{
    public partial class SincronizarDispositivo : Form
    {
        public SincronizarDispositivo()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SincronizarDispositivo_Load(object sender, EventArgs e)
        {
            LlenarComboBox();
            LlenarDgvLocal();
        }

        private void LlenarComboBox()
        {
            ControladorReloj cr = new ControladorReloj();
            List<Reloj> relojes = new List<Reloj>();

            try
            {
                relojes = cr.TodosRelojes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            foreach(Reloj r in relojes)
            {
                comboRelojes.Items.Add(r.Numero + " " + r.Nombre);
            }
            
        }

        private void comboRelojes_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LlenarDgvLocal()
        {
            dgvLocal.AutoGenerateColumns = false;
            ControladorEmpleados ce = new ControladorEmpleados();
            DataTable empleados = new DataTable();
            try
            {
                empleados = ce.GetEmpleados();
                dgvLocal.DataSource = empleados;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dgvLocal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Como el datagridview tiene la propieda ReadOnly activada, no me permite seleccionar el checkbox.
            // Necesito si o si manejarlo por código en este evento.
            if (e.RowIndex == -1)
                return;

            if (dgvLocal.Columns[e.ColumnIndex].Name == "Seleccionar")
            {
                DataGridViewRow row = dgvLocal.Rows[e.RowIndex];

                DataGridViewCheckBoxCell cellSeleccion = row.Cells["Seleccionar"] as DataGridViewCheckBoxCell;

                //Verifico si está tildado
                if (Convert.ToBoolean(cellSeleccion.Value))
                {
                    cellSeleccion.Value = false;
                }
                else
                {
                    cellSeleccion.Value = true;
                }
            }
        }
    }
}
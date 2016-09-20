﻿using System;
using System.Collections.Generic;
using ZkManagement.Datos;
using ZkManagement.Entidades;

namespace ZkManagement.Logica
{
    class ControladorReloj
    {
        public List<Reloj> TodosRelojes()
        {
            List<Reloj> relojes = new List<Reloj>();
            try
            {
                relojes = CatalogoRelojes.GetInstancia().GetRelojes();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return relojes;
        }

        public void ModifReloj(Reloj r)
        {
            try
            {
                if (r.Id > 0) { CatalogoRelojes.GetInstancia().ActualizarReloj(r); }
                else { CatalogoRelojes.GetInstancia().AgregarReloj(r); }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        public void EliminarReloj(Reloj r)
        {
            try
            {
                CatalogoRelojes.GetInstancia().EliminarReloj(r);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActualizarBorrado(int idReloj, int cantidad)
        {
            DateTime fecha = DateTime.Now;
            ControladorLogin cl = new ControladorLogin();
            try
            {
                CatalogoRelojes.GetInstancia().SetBorrado(cl.GetUsrId(), idReloj, cantidad, fecha);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}

﻿using System;
using System.Data.SqlClient;
using ZkManagement.Util;

namespace ZkManagement.Datos
{
    class CatalogoConfiguraciones
    {
        // Patrón Singleton //
        private static CatalogoConfiguraciones _instancia;
        public static CatalogoConfiguraciones GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new CatalogoConfiguraciones();
            }
            return _instancia;
        }

        // Hasta acá //

        private string query = string.Empty;

        public string GetConfig(int id)
        {         
            string valor;
            SqlCommand cmd;
            SqlDataReader dr;
            try
            {

                query = "SELECT Valor FROM Configuracion WHERE ConfigId=" + id.ToString();
                cmd = new SqlCommand(query,Conexion.GetInstancia().GetConn());
                dr = cmd.ExecuteReader();
                dr.Read();
                valor = dr["Valor"].ToString();
                dr.Close();
            }
            catch(SqlException sqlEx)
            {
                Logger.GetLogger().Error(sqlEx);
                throw new Exception("Error al consulta el valor de configuracion: " + id.ToString());
            }
            catch(Exception ex)
            {
                Logger.GetLogger().Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al consultar el valor de configuracion: " + id.ToString());
            }
            finally
            {
                try
                {
                    Conexion.GetInstancia().ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return valor;
        }

        public void SetConfig(int id, string valor)
        {
            try
            {
                query = "UPDATE Configuracion SET Valor='" + valor + "' WHERE ConfigId=" + id.ToString();
                SqlCommand cmd = new SqlCommand(query, Conexion.GetInstancia().GetConn());
                cmd.ExecuteNonQuery();
            }
            catch(SqlException sqlEx)
            {
                Logger.GetLogger().Error(sqlEx.StackTrace);
                throw new Exception("Error al actualizar la tabla configuracion");
            }
            catch(Exception ex)
            {
                Logger.GetLogger().Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al actualizar la tabla configuracion");
            }
            finally
            {
                try
                {
                    Conexion.GetInstancia().ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}

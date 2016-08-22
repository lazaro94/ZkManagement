﻿using System;
using System.Data.SqlClient;
using ZkManagement.Entidades;
using log4net;
using System.Collections.Generic;

namespace ZkManagement.Datos
{
    class CatalogoHuellas
    {
        private Conexion con = new Conexion();
        private SqlConnection conn = new SqlConnection();
        private ILog logger = LogManager.GetLogger("");

        public void InsertarHuella(Huella h, int id)
        {
            try
            {
                conn = con.Conectar();
                SqlCommand cmd = new SqlCommand("INSERT INTO Huellas VALUES(" + id.ToString() + ", '" + h.Template + "', '" + h.FingerIndex.ToString() + "', '" + h.Lengh.ToString() + "' )", conn);
                cmd.ExecuteNonQuery();
            }
            catch(SqlException sqlEx)
            {
                logger.Error(sqlEx.StackTrace);
                throw (sqlEx);
              //  throw new Exception("Error al insertar registros en la tabla huellas");
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al intentar actualizar en la tabla huellas");
            }
            conn.Close();
        }

        public bool Existe(Huella h, int id)
        {
            try
            {
                conn = con.Conectar();
                SqlCommand cmd = new SqlCommand("SELECT HuellaId FROM HUELLAS WHERE IdEmpleado='"+id.ToString() + "' AND FingerIndex='" + h.FingerIndex.ToString() + "'" , conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    conn.Close();
                    return true;
                }
                else
                {
                    conn.Close();
                    return false;                  
                }
            }
            catch (SqlException sqlex)
            {
                logger.Error(sqlex.StackTrace);
                throw new Exception("Error al consultar la tabla de huellas");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al consultar la tabla huellas");
            }            
        }

        public void ActualizarHuella(Huella h, int id)
        {
            try
            {
                conn = con.Conectar();
                SqlCommand cmd = new SqlCommand("UPDATE Huellas SET IdEmpleado='" + id.ToString() + "', Template='" + h.Template + "', FingerIndex='" + h.FingerIndex.ToString() + "', Lengh='" + h.Lengh + "'", conn);
                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                logger.Error(sqlex.StackTrace);
                throw new Exception("Error al actualizar la tabla de huellas");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al actualizar la tabla huellas");
            }
        }
        public List<string> GetHuellas(Empleado emp)
        {
            List<string> huellas = new List<string>();
            try
            {
                conn = con.Conectar();
                SqlCommand cmd = new SqlCommand("SELECT Template FROM Huellas WHERE IdEmpleado=" + emp.Id.ToString());
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    huellas.Add(dr["Template"].ToString());
                }
            }
            catch(SqlException sqlex)
            {
                logger.Error(sqlex.StackTrace);
                throw new Exception("Error al consultar la tabla huellas");
            }
            catch(Exception ex)
            {
                logger.Fatal(ex.StackTrace);
                throw new Exception("Error desconocido al intentar consultar la tabla huellas");
            }
            return huellas;
        }
    }
}

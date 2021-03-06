﻿using System;
using System.Data;
using Entidades;
using Util;
using System.Collections.Generic;
using System.Data.Common;

namespace Database
{
    public class DataEmpleado
    {
        private string query = string.Empty;

        public List<Empleado> Empleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            IDataReader dr = null;

            try
            {                            
                query = "SELECT e.Legajo, e.IdEmpleado, e.Nombre, e.Tarjeta, e.DNI, e.Pin, e.Privilegio, e.Baja, e.Apellido, COUNT(h.IdEmpleado) as CantHuellas FROM Empleados e LEFT JOIN Huellas h ON e.IdEmpleado=h.IdEmpleado GROUP BY e.IdEmpleado, e.Nombre, e.Apellido, e.Pin, e.Tarjeta, e.Legajo, e.DNI, e.Privilegio, e.Baja ORDER BY e.Nombre ASC";           
                dr = FactoryConnection.Instancia.GetReader(query, FactoryConnection.Instancia.GetConnection());
                while (dr.Read())
                {
                    DateTime parseValue;
                    Empleado e = new Empleado();
                    e.Legajo = dr["Legajo"].ToString();
                    e.Id = Convert.ToInt32(dr["IdEmpleado"]);
                    e.Nombre = dr["Nombre"].ToString();
                    e.Tarjeta = dr["Tarjeta"].ToString();
                    e.DNI = dr["DNI"].ToString();
                    e.Pin = dr["Pin"].ToString();
                    e.Privilegio = Convert.ToInt32(dr["Privilegio"]);
                    e.CantHuellas = Convert.ToInt32(dr["CantHuellas"]);
                    e.Apellido = dr["Apellido"].ToString();
                    if (DateTime.TryParse(dr["Baja"].ToString(), out parseValue))
                    {
                        if (parseValue != null && parseValue != DateTime.MinValue)
                        {
                            e.Baja = parseValue;
                        }
                    }
                    else
                    {
                        e.Baja = null;
                    }
                    empleados.Add(e);
                }
            }
            catch(AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar consultar los datos de los empleados", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar consultar los datos de los empleados", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return empleados;
        }

        public void Eliminar(List<Empleado> empleados)
        {
            IDbCommand cmd = null;
            query = "DELETE FROM Empleados WHERE IdEmpleado=@Id";
            try
            {
                cmd = FactoryConnection.Instancia.GetCommand(query, FactoryConnection.Instancia.GetConnection());
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Id";
                foreach (Empleado e in empleados)
                {
                    cmd.Parameters.Clear(); //--> Borro el parametro que inserté en la pasada anterior.
                    parameter.Value = e.Id;
                    cmd.Parameters.Add(parameter);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar eliminar empleado de la base de datos", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar eliminar el empleado", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Actualizar(Empleado emp)
        {
            IDbCommand cmd = null;
            IDbDataParameter param;
            try
            {
                query = "UPDATE Empleados SET DNI='" + emp.DNI + "', Legajo='" + emp.Legajo + "', Nombre='" + emp.Nombre + "', Pin=@pin, Tarjeta='" + emp.Tarjeta +
                    "', Privilegio='" + emp.Privilegio.ToString() + "', Baja=@baja, Apellido='" + emp.Apellido + "' WHERE IdEmpleado=" + emp.Id.ToString();
                cmd = FactoryConnection.Instancia.GetCommand(query, FactoryConnection.Instancia.GetConnection());
                param = cmd.CreateParameter();                             
                param.ParameterName = "@baja";
                param.Value = emp.Baja == null ? (object)DBNull.Value : emp.Baja;
                cmd.Parameters.Add(param);
                param = cmd.CreateParameter();
                param.ParameterName = "@pin";
                param.Value = emp.Pin == null ? (object)DBNull.Value : emp.Pin;
                cmd.Parameters.Add(param);
                cmd.ExecuteNonQuery();
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar actualizar los datos en la tabla empleados", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar actualizar los datos del empleado", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void Agregar(Empleado emp)
        {
            IDbCommand cmd = null;
            try
            {
                query = "INSERT INTO Empleados (Nombre, Apellido, Pin, Tarjeta, Legajo, DNI, Privilegio, Baja, Alta) Values('" + emp.Nombre + "', '" + emp.Apellido + "', " + emp.Pin.ToString() + ", '" + emp.Tarjeta +
                    "', '" + emp.Legajo + "', '" + emp.DNI + "', '" + emp.Privilegio.ToString() + "', @baja, GETDATE() )";

                cmd = FactoryConnection.Instancia.GetCommand(query, FactoryConnection.Instancia.GetConnection());

                var par = cmd.CreateParameter();
                par.ParameterName="@baja";
                if(emp.Baja == null)
                {
                    par.Value = DBNull.Value;
                }else
                {
                    par.Value = emp.Baja;
                }
                cmd.Parameters.Add(par);

                cmd.ExecuteNonQuery();
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al actualizar la tabla empleados", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar agregar el empleado", "Fatal", ex);
            }
            finally
            {
                try
                {
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public Empleado GetIdByLegajo(Empleado emp)
        {
            IDataReader dr = null;
            try
            {
                query = "SELECT e.IdEmpleado, e.DNI, e.Baja FROM Empleados e WHERE e.Legajo='" + emp.Legajo +"'";
                dr = FactoryConnection.Instancia.GetReader(query, FactoryConnection.Instancia.GetConnection());
                if (dr.Read())
                {
                    emp.Id = Convert.ToInt32(dr["IdEmpleado"]);
                    emp.DNI = dr["DNI"].ToString();
                    emp.Baja = dr.IsDBNull(2) ? null : (DateTime?)dr.GetDateTime(2);
                }       
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar consultar la tabla empleados", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar consultar la tabla empleados", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return emp;
        }

        public void InsertarRegis(Fichada f)
        {
            IDbCommand cmd = null;
            try
            {
                query = "INSERT INTO Registros (IdEmpleado, Tipo, Reloj, Fecha) VALUES('" + f.Empleado.Id.ToString() + "', '" + f.Movimiento + "', " + f.Reloj.Numero.ToString() + ", '" + f.Registro.ToString("dd-MM-yyyy HH:mm:ss") + "')";
                cmd = FactoryConnection.Instancia.GetCommand(query, FactoryConnection.Instancia.GetConnection());
                cmd.ExecuteNonQuery();
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar insertar en la tabla registros", "Error", dbEx);
            }
            catch(Exception ex)
            {
                throw new AppException("Error desconocido al intentar actualizar la tabla registros", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public Empleado GetDataByLegajo(Empleado emp)
        {
            IDataReader dr = null;
            try
            {
                query = "SELECT e.IdEmpleado, e.Nombre, e.Apellido, e.Tarjeta, e.DNI, e.Pin, e.Privilegio, e.Baja FROM Empleados e WHERE e.Legajo='" + emp.Legajo + "'";
                dr = FactoryConnection.Instancia.GetReader(query, FactoryConnection.Instancia.GetConnection());
                if (dr.Read())
                {
                    emp.Id = Convert.ToInt32(dr["IdEmpleado"]);
                    emp.Nombre = dr["Nombre"].ToString();
                    emp.Tarjeta = dr["Tarjeta"].ToString();
                    emp.DNI = dr["DNI"].ToString();
                    emp.Pin = dr["Pin"].ToString();
                    emp.Privilegio = Convert.ToInt32(dr["Privilegio"]);
                    emp.Baja = Convert.ToDateTime(dr["Baja"]);
                    emp.Apellido = dr["Apellido"].ToString();
                }
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al intentar consultar la tabla empleados", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar consultar la tabla empleados", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return emp;

        }

        //Este método lo utilizo para setearle las huellas al list de empleados
        public List<Empleado> SetHuellas(List<Empleado> empleados)
        {
            query = "SELECT FingerIndex, Template, Lengh, Flag FROM Huellas WHERE IdEmpleado = @Id";
            IDataReader dr = null;
            IDbCommand cmd = null;

            try
            {
                cmd = FactoryConnection.Instancia.GetCommand(query, FactoryConnection.Instancia.GetConnection());
                var parameter = cmd.CreateParameter();
                parameter.ParameterName = "@Id";
                foreach (Empleado e in empleados)
                {
                    cmd.Parameters.Clear(); //--> Borro el parametro que inserté en la pasada anterior.
                    parameter.Value = e.Id;
                    cmd.Parameters.Add(parameter);

                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Huella h = new Huella();
                        h.FingerIndex = Convert.ToInt32(dr["FingerIndex"]);
                        h.Lengh = Convert.ToInt32(dr["Lengh"]);
                        h.Template = dr["Template"].ToString();
                        h.Flag = Convert.ToInt32(dr["Flag"]);
                        e.Huellas.Add(h);
                    }
                    dr.Close(); //--> Lo cierro para poder iniciarlizarlo cuando vuelvo a pasar por el bucle.
                }
            }
            catch (AppException appex)
            {
                throw appex;
            }
            catch (DbException dbEx)
            {
                throw new AppException("Error al consultar la tabla huellas", "Error", dbEx);
            }
            catch (Exception ex)
            {
                throw new AppException("Error desconocido al intentar consultar la tabla huellas", "Fatal", ex);
            }
            finally
            {
                try
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                    FactoryConnection.Instancia.ReleaseConn();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return empleados;
        }
    }
}

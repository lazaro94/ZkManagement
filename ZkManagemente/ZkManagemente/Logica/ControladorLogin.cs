﻿using System;
using ZkManagement.Datos;
using ZkManagement.Entidades;
using ZkManagemente.Util;

namespace ZkManagement.Logica
{
    class ControladorLogin
    {
        public void ValidarUsuario(Usuario usuario)
        {
            Usuario usr = new Usuario();
            CatalogoUsuarios cu = new CatalogoUsuarios();
            try
            {
                usr = cu.GetUsuario(usuario);
                if (usr.usr == null) { throw new AppException("Usuario invalido"); }
                if (usr.pass!=usuario.pass) { throw new AppException("Password invalida"); }
            }
            catch  (InvalidCastException ioex)
            {
                throw ioex;
            }
        }
    }
}

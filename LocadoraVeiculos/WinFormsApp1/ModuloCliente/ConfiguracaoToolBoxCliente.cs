﻿using LocadoraVeiculosForm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocadoraVeiculosForm.ModuloCliente
{
    public class ConfiguracaoToolBoxCliente : ConfiguracaoToolboxBase
    {
        public override string TipoCadastro => "Cadastro de clientes";

        public override string TooltipInserir => throw new NotImplementedException();

        public override string TooltipEditar => throw new NotImplementedException();

        public override string TooltipExcluir => throw new NotImplementedException();
    }
}

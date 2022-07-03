﻿using LocadoraVeiculos.Aplicacao.ModuloCliente.ClienteEmpresa;
using LocadoraVeiculos.BancoDados.ModuloCliente;
using LocadoraVeiculos.BancoDados.ModuloCliente.ClienteEmpresa;
using LocadoraVeiculos.Dominio.ModuloCliente.ClienteEmpresa;
using LocadoraVeiculosForm.Compartilhado;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LocadoraVeiculosForm.ModuloCliente.ClienteEmpresa
{
    public class ControladorEmpresa : ControladorBase
    {

        private RepositorioEmpresaBancoDados repositorio;
        private ServicoEmpresa servicoEmpresa;
        private ListagemEmpresasControl listagem;

        public ControladorEmpresa(RepositorioEmpresaBancoDados repositorio, ServicoEmpresa servicoEmpresa)
        {

            this.repositorio = repositorio;
            this.servicoEmpresa = servicoEmpresa;
            listagem = new ListagemEmpresasControl();

        }

        private void CarregarEmpresas()
        {
            List<Empresa> empresas = repositorio.SelecionarTodos();

            listagem.AtualizarRegistrosPessoaJuridica(empresas);
        }


        private Empresa ObtemEmpresaSelecionada()
        {
            int id = listagem.SelecionarNumeroEmpresa();

            Empresa empresaSelecionada = repositorio.SelecionarPorId(id);

            return empresaSelecionada;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Inserir()
        {

            var tela = new TelaCadastroEmpresaForm();
            tela.Empresa = new Empresa();
            tela.GravarRegistro = servicoEmpresa.Inserir;
            DialogResult resultado = tela.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                CarregarEmpresas();
            }
        }

        public override void Editar()
        {
            Empresa empresaSelecionada = ObtemEmpresaSelecionada();

            if (empresaSelecionada == null)
            {
                MessageBox.Show("Selecione um cliente primeiro",
                "Edição de Clientes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            TelaCadastroEmpresaForm tela = new TelaCadastroEmpresaForm();

            tela.Empresa = empresaSelecionada;

            tela.GravarRegistro = servicoEmpresa.Editar;

            DialogResult resultado = tela.ShowDialog();

            if (resultado == DialogResult.OK)
                CarregarEmpresas();
        }

        public override void Excluir()
        {
            Empresa empresaSelecionada = ObtemEmpresaSelecionada();

            if (empresaSelecionada == null)
            {
                MessageBox.Show("Selecione uma empresa primeiro",
                "Exclusão de empresa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DialogResult resultado = MessageBox.Show("Deseja realmente excluir a empresa?",
                "Exclusão de empresa", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (resultado == DialogResult.OK)
            {
                try
                {
                    repositorio.Excluir(empresaSelecionada);
                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message,
                        "Exclusão de empresa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                
                CarregarEmpresas();
            }
        }

        public override UserControl ObtemListagem()
        {
            if (listagem == null)
                listagem = new ListagemEmpresasControl();

            CarregarEmpresas();

            return listagem;
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolBoxEmpresa();
        }
    }
}

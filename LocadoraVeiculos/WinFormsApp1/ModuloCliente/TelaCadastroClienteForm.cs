﻿using LocadoraVeiculos.Dominio.ModuloCliente;
using System;
using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using ValidationResult = FluentValidation.Results.ValidationResult;
using LocadoraVeiculos.BancoDados.ModuloCliente;
using LocadoraVeiculos.BancoDados.ModuloCliente.ClienteEmpresa;
using LocadoraVeiculos.Dominio.ModuloCliente.ClienteEmpresa;
using System.Collections.Generic;

namespace LocadoraVeiculosForm.ModuloCliente
{
    public partial class TelaCadastroClienteForm : Form
    {

        Cliente cliente;
        List<Empresa> empresas;
        RepositorioClienteEmBancoDados repositorio;
        RepositorioEmpresaBancoDados repositorioEmpresa;
        public TelaCadastroClienteForm(RepositorioEmpresaBancoDados repositorioEmpresa)
        {
            InitializeComponent();
            comboBoxEmpresas.Enabled = false;
            empresas = repositorioEmpresa.SelecionarTodos();
            repositorio = new RepositorioClienteEmBancoDados();
            this.repositorioEmpresa = repositorioEmpresa;
        }

        public Func<Cliente, ValidationResult> GravarRegistro { get; set; }


        public Cliente Cliente
        {
            get
            {
                return cliente;
            }
            set
            {
                cliente = value;
                txtNome.Text = cliente.Nome;
                txtEmail.Text = cliente.Email;
                txtTelefone.Text = cliente.Telefone;
                txtCpf.Text = cliente.CPF;
                txtEndereco.Text = cliente.Endereco;
                txtNumeroCnh.Text = cliente.CnhNumero.ToString();
                txtNomeCnh.Text = cliente.CnhNome;
                if (cliente.CnhVencimento == DateTime.MinValue)
                    monthCalendarVencimento.Value = DateTime.Now.Date;
                else
                    monthCalendarVencimento.Value = cliente.CnhVencimento;
                comboBoxEmpresas.SelectedItem = cliente.Empresa;

            }
        }

        private void buttonCadastrar_Click(object sender, EventArgs e)
        {

            cliente.Nome = txtNome.Text;
            cliente.Email = txtEmail.Text;
            cliente.Telefone = txtTelefone.Text;
            cliente.Endereco = txtEndereco.Text;
            cliente.CPF = txtCpf.Text;
            cliente.CnhNumero = Convert.ToInt32(txtNumeroCnh.Text);
            cliente.CnhNome = txtNomeCnh.Text;
            cliente.CnhVencimento = monthCalendarVencimento.Value;
            cliente.Empresa = null;
            foreach (var item in empresas)
            {

                if (item.Nome.Equals(comboBoxEmpresas.SelectedItem))
                    cliente.Empresa = item;

            }

            var resultadoValidacao = GravarRegistro(cliente);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaMenuPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }
        }

        private void TelaCadastroClientesForm_Load(object sender, EventArgs e)
        {
            TelaMenuPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void TelaCadastroClientesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TelaMenuPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxPessoasFisicas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if(comboBoxEmpresas.Items.Count == 0)
            {

                foreach (var empresa in empresas)
                {

                    comboBoxEmpresas.Items.Add(empresa.Nome);

                }

            }
            
            if (checkBoxCondutor.Checked)
                comboBoxEmpresas.Enabled = true;
            else
                comboBoxEmpresas.Enabled = false;
        }
    }
}

﻿using FluentValidation.Results;
using LocadoraVeiculos.Dominio.ModuloTaxa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocadoraVeiculosForm.ModuloTaxa
{
    public partial class TelaCadastroTaxaForm : Form
    {

        Taxa taxa;
        public TelaCadastroTaxaForm()
        {
            InitializeComponent();
        }

        public Func<Taxa, ValidationResult> GravarRegistro { get; set; }

        public Taxa Taxa
        {

            get
            {

                return taxa;

            }

            set
            {

                taxa = value;
                txtDescricao.Text = taxa.Descricao;
                txtValor.Text = taxa.Valor.ToString();
                comboBoxTipoCalculo.SelectedIndex = (int)taxa.TipoCalculo;

            }

        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {

            taxa.Descricao = txtDescricao.Text;
            taxa.Valor = Convert.ToDecimal(txtValor.Text);
            taxa.TipoCalculo = (TipoCalculoEnum)comboBoxTipoCalculo.SelectedIndex;

            var resultadoValidacao = GravarRegistro(taxa);

            if (resultadoValidacao.IsValid == false)
            {
                string erro = resultadoValidacao.Errors[0].ErrorMessage;

                TelaMenuPrincipalForm.Instancia.AtualizarRodape(erro);

                DialogResult = DialogResult.None;
            }

        }

        private void TelaCadastroTaxasForm_Load(object sender, EventArgs e)
        {
            TelaMenuPrincipalForm.Instancia.AtualizarRodape("");
        }

        private void TelaCadastroTaasForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TelaMenuPrincipalForm.Instancia.AtualizarRodape("");
        }

    }
}

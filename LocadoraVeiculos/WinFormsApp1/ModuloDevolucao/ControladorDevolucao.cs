﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using LocadoraVeiculos.Aplicacao.ModuloDevolucao;
using LocadoraVeiculos.Aplicacao.ModuloLocacao;
using LocadoraVeiculos.Aplicacao.ModuloPlanoCobranca;
using LocadoraVeiculos.Aplicacao.ModuloTaxa;
using LocadoraVeiculos.Dominio.ModuloDevolucao;
using LocadoraVeiculosForm.Compartilhado;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace LocadoraVeiculosForm.ModuloDevolucao
{
    public class ControladorDevolucao : ControladorBase
    {

        private ServicoDevolucao servico;
        private ListagemDevolucoesControl listagem;
        private ServicoLocacao servicoLocacao;
        private ServicoPlanoCobranca servicoPlano;
        private ServicoTaxa servicoTaxa;

        public ControladorDevolucao(ServicoDevolucao servico, ServicoLocacao servicoLocacao, ServicoPlanoCobranca servicoPlano, ServicoTaxa servicoTaxa)
        {
            listagem = new ListagemDevolucoesControl();
            this.servico = servico;
            this.servicoLocacao = servicoLocacao;
            this.servicoPlano = servicoPlano;
            this.servicoTaxa = servicoTaxa;
        }

        public override void Editar()
        {
            var id = listagem.SelecionarIdDevolucao();

            if (id == Guid.Empty)
            {
                MessageBox.Show("Selecione uma devolução primeiro",
                    "Edição de Devoluções", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var resultado = servico.SelecionarPorId(id);

            if (resultado.IsFailed)
            {
                MessageBox.Show(resultado.Errors[0].Message,
                    "Edição de devoluções", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var devoluçãoSelecionada = resultado.Value;

            var tela = new TelaCadastroDevolucaoForm(servicoLocacao, servicoPlano, servicoTaxa);

            tela.Devolucao = devoluçãoSelecionada;

            tela.GravarRegistro = servico.Editar;

            if (tela.ShowDialog() == DialogResult.OK)
                CarregarDevolucoes();
        }

        public override void Excluir()
        {
            var id = listagem.SelecionarIdDevolucao();

            if (id == Guid.Empty)
            {
                MessageBox.Show("Selecione uma devolução primeiro",
                    "Exclusão de Devolução", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var resultadoSelecao = servico.SelecionarPorId(id);

            if (resultadoSelecao.IsFailed)
            {
                MessageBox.Show(resultadoSelecao.Errors[0].Message,
                    "Exclusão de devolução", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var devolucaoSelecionada = resultadoSelecao.Value;

            if (MessageBox.Show("Deseja realmente excluir a devolução?", "Exclusão de Devolução",
                 MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var resultadoExclusao = servico.Excluir(devolucaoSelecionada);

                if (resultadoExclusao.IsSuccess)
                    CarregarDevolucoes();
                else
                    MessageBox.Show(resultadoExclusao.Errors[0].Message,
                        "Exclusão de Deoluções", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override void Inserir()
        {
            var tela = new TelaCadastroDevolucaoForm(servicoLocacao, servicoPlano, servicoTaxa);
            tela.Devolucao = new Devolucao();
            tela.GravarRegistro = servico.Inserir;
            DialogResult resultado = tela.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                CarregarDevolucoes();
            }
        }

        public override ConfiguracaoToolboxBase ObtemConfiguracaoToolbox()
        {
            return new ConfiguracaoToolBoxDevolucao();
        }

        private void CarregarDevolucoes()
        {
            var resultado = servico.SelecionarTodos();

            if (resultado.IsSuccess)
            {
                List<Devolucao> devolucoes = resultado.Value;

                listagem.AtualizarRegistros(devolucoes);

                TelaMenuPrincipalForm.Instancia.AtualizarRodape($"Visualizando {devolucoes.Count} devoluções");
            }
            else
            {
                MessageBox.Show(resultado.Errors[0].Message, "Carregar listagem",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override UserControl ObtemListagem()
        {
            if (listagem == null)
                listagem = new ListagemDevolucoesControl();

            CarregarDevolucoes();

            return listagem;
        }

        public override void GerarPdf()
        {
            var id = listagem.SelecionarIdDevolucao();

            if (id == Guid.Empty)
            {
                MessageBox.Show("Selecione uma devolução primeiro",
                    "Pdf de Devoluções", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var resultado = servico.SelecionarPorId(id);

            if (resultado.IsFailed)
            {
                MessageBox.Show(resultado.Errors[0].Message,
                    "Pdf de devoluções", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var devoluçãoSelecionada = resultado.Value;

            string nomeArquivo = @"C:\Windows\Temp" + @"\teste.pdf";
            FileStream arquivoPDF = new FileStream(nomeArquivo, FileMode.Create);
            Document doc = new Document(PageSize.A4);
            PdfWriter escritorPDF = PdfWriter.GetInstance(doc, arquivoPDF);

            string dados = "";

            Paragraph paragrafo = new Paragraph(dados, new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 14, (int)System.Drawing.FontStyle.Regular));
            paragrafo.Alignment = Element.ALIGN_CENTER;
            paragrafo.Add("Devolução" + "\n\n");
            paragrafo.Add("Id da devolução: " + devoluçãoSelecionada.Id + "\n\n");
            paragrafo.Add("Nome do cliente: " + devoluçãoSelecionada.Locacao.Cliente.Nome + "\n\n");
            paragrafo.Add("Grupo de veículos da  locação: " + devoluçãoSelecionada.Locacao.GrupoVeiculos.Nome + "\n\n");

            doc.Open();
            doc.Add(paragrafo);
            doc.Close();

        }
    }
}

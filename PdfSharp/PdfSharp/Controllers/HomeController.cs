using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PdfSharp.Models;
using PdfSharpCore.Drawing;

namespace PdfSharp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public FileResult GerarRelatorio()
        {
            using (var doc = new PdfSharpCore.Pdf.PdfDocument())
            {
                var page = doc.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
                page.Orientation = PdfSharpCore.PageOrientation.Portrait;
                var graphics = PdfSharpCore.Drawing.XGraphics.FromPdfPage(page);
                var corFonte = PdfSharpCore.Drawing.XBrushes.Black;
                var textFormatter = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                var fonteOrganizacao = new PdfSharpCore.Drawing.XFont("Arial", 10);
                var fonteDescricao = new PdfSharpCore.Drawing.XFont("Arial", 8, PdfSharpCore.Drawing.XFontStyle.BoldItalic);
                var titulodetalhes = new PdfSharpCore.Drawing.XFont("Arial", 14, PdfSharpCore.Drawing.XFontStyle.Bold);
                var fonteDetalhesDescricao = new PdfSharpCore.Drawing.XFont("Arial", 7);

                var logo = @"C:\TestesDev\PdfSharp\PdfSharp\PdfSharp\wwwroot\imagens\transferir.jpg";

                var qtdPaginas = doc.PageCount;

                textFormatter.DrawString(qtdPaginas.ToString(), new PdfSharpCore.Drawing.XFont("Arial", 10), corFonte, new PdfSharpCore.Drawing.XRect(578, 825, page.Width, page.Height));


                //Impressão do logo
                XImage imagem = XImage.FromFile(logo);
                graphics.DrawImage(imagem, 20, 5, 300, 50);

                textFormatter.DrawString("Nome :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 75, page.Width, page.Height));
                textFormatter.DrawString("Thiago Lanza", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 75, page.Width, page.Height));

                textFormatter.DrawString("Profissão :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 95, page.Width, page.Height));
                textFormatter.DrawString("Programador", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 95, page.Width, page.Height));

                textFormatter.DrawString("Tempo :", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, 115, page.Width, page.Height));
                textFormatter.DrawString("2 anos", fonteOrganizacao, corFonte, new PdfSharpCore.Drawing.XRect(80, 115, page.Width, page.Height));

                // Título maior
                var tituloDetalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);
                tituloDetalhes.Alignment = PdfSharpCore.Drawing.Layout.XParagraphAlignment.Center;
                tituloDetalhes.DrawString("Detalhes", titulodetalhes, corFonte, new PdfSharpCore.Drawing.XRect(0, 120, page.Width, page.Height));


                //Título das colunas
                var alturaTituloDetalhesY = 140;
                var detalhes = new PdfSharpCore.Drawing.Layout.XTextFormatter(graphics);

                detalhes.DrawString("Descrição", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(20, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Atendimento", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(144, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Operação", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(220, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Quantidade", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(290, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Status", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(360, alturaTituloDetalhesY, page.Width, page.Height));

                detalhes.DrawString("Data", fonteDescricao, corFonte, new PdfSharpCore.Drawing.XRect(420, alturaTituloDetalhesY, page.Width, page.Height));

                //dados do relatório
                var alturaDetalhesItens = 160;

                void gerarTexto(string texto, int largura)
                {
                    textFormatter.DrawString(texto, fonteDetalhesDescricao, corFonte, new PdfSharpCore.Drawing.XRect(largura, alturaDetalhesItens, page.Width, page.Height));
                }

                for (int i = 1; i < 30; i++)
                {
                    string IToString = i.ToString();
                    string TextoDescricao = "Descrição :" + IToString;
                    string TextoAtendimento = "Atendimento : " + IToString;
                    string TextoOperacao = "Operação : " + IToString;
                    string TextoQuantidade = "Quantidade : " + IToString;
                    string TextoStatus = "Status : " + IToString;

                    gerarTexto(TextoDescricao, 15);
                    gerarTexto(TextoAtendimento, 145);
                    gerarTexto(TextoOperacao, 215);
                    gerarTexto(TextoQuantidade, 290);
                    gerarTexto(TextoStatus, 360);
                    gerarTexto(DateTime.Now.ToString(), 420);
                    alturaDetalhesItens += 20;
                }


                using (MemoryStream stream = new MemoryStream())
                {
                    var contentType = "application/pdf";

                    doc.Save(stream, false);

                    var nomeArquivo = "relatorioThiago.pdf";

                    return File(stream.ToArray(), contentType, nomeArquivo);
                }

            }
        }
    }
}

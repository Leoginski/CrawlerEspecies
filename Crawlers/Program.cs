//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="StarGiários S.A.">
//     Copyright (c) StarGiários S.A.. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Crawlers
{
    using HtmlAgilityPack;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Class Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Url inicial utilizada para capturar a quantidade de páginas
        /// </summary>
        private static string index = "https://www.worldwildlife.org/species/directory?direction=desc&sort=extinction_status";

        /// <summary>
        /// Url que será formatada para navegar através das páginas
        /// </summary>
        private static string url = "https://www.worldwildlife.org/species/directory?direction=desc&page={0}&sort=extinction_status";

        /// <summary>
        /// Main Method
        /// </summary>
        /// <param name="args">Args to execute</param>
        public static void Main(string[] args)
        {
            int i;
            HtmlDocument doc = NavegaPagina(index);
            List<Especie> especies = new List<Especie>();

            var nodes = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'list-pagination')]/li");

            foreach (var node in nodes)
            {
                if (HasNext(node, out i))
                {
                    doc = NavegaPagina(string.Format(url, i));
                    var linhas = node.SelectNodes("//tbody/tr");

                    especies.AddRange(from linha in linhas select CapturaEspecie(linha));
                }
            }

            var json = JsonConvert.SerializeObject(especies);

            System.IO.File.WriteAllText(@"C:\Users\u6068836\Desktop\json.json", json);
        }

        /// <summary>
        /// Navega para a página solicitada pelo parâmetro url
        /// </summary>
        /// <param name="url">Url para a qual o método deverá navegar</param>
        /// <returns>Retorna um nó que contem a página</returns>
        private static HtmlDocument NavegaPagina(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }

        /// <summary>
        /// Verifica se deve navegar para a próxima página.
        /// </summary>
        /// <param name="node">Node 'li' contida na lista de páginas</param>
        /// <param name="i">Número da página atual</param>
        /// <returns>Booleano</returns>
        private static bool HasNext(HtmlNode node, out int i)
        {
            var li = node.InnerText.Replace("\n", string.Empty);
            return int.TryParse(li, out i);
        }

        /// <summary>
        /// Cria uma instância da classe Especie
        /// </summary>
        /// <param name="node">Node que contém 'tr' da lista de espécies</param>
        /// <returns>Objeto Especie</returns>
        private static Especie CapturaEspecie(HtmlNode node)
        {
            Especie especie = new Especie();

            var coluna = node.SelectSingleNode("./td[1]");
            especie.CommonName = coluna.InnerText;
            coluna = node.SelectSingleNode("./td[2]");
            especie.ScientificName = coluna.InnerText;
            coluna = node.SelectSingleNode("./td[3]");
            especie.ConservationStatus = coluna.InnerText;

            return especie;
        }
    }
}
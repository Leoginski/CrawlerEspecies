//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="StarGi�rios S.A.">
//     Copyright (c) StarGi�rios S.A.. All rights reserved.
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
        /// Url inicial utilizada para capturar a quantidade de p�ginas
        /// </summary>
        private static string index = "https://www.worldwildlife.org/species/directory?direction=desc&sort=extinction_status";

        /// <summary>
        /// Url que ser� formatada para navegar atrav�s das p�ginas
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
        /// Navega para a p�gina solicitada pelo par�metro url
        /// </summary>
        /// <param name="url">Url para a qual o m�todo dever� navegar</param>
        /// <returns>Retorna um n� que contem a p�gina</returns>
        private static HtmlDocument NavegaPagina(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }

        /// <summary>
        /// Verifica se deve navegar para a pr�xima p�gina.
        /// </summary>
        /// <param name="node">Node 'li' contida na lista de p�ginas</param>
        /// <param name="i">N�mero da p�gina atual</param>
        /// <returns>Booleano</returns>
        private static bool HasNext(HtmlNode node, out int i)
        {
            var li = node.InnerText.Replace("\n", string.Empty);
            return int.TryParse(li, out i);
        }

        /// <summary>
        /// Cria uma inst�ncia da classe Especie
        /// </summary>
        /// <param name="node">Node que cont�m 'tr' da lista de esp�cies</param>
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
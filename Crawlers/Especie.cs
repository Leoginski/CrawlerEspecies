//-----------------------------------------------------------------------
// <copyright file="Especie.cs" company="StarGiários S.A.">
//     Copyright (c) StarGiários S.A.. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Crawlers
{
    /// <summary>
    /// Class for species
    /// </summary>
    public class Especie
    {
        /// <summary>
        /// Gets or sets Common Name
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Gets or sets Scientific Name
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Gets or sets Conservation Status
        /// </summary>
        public string ConservationStatus { get; set; }
    }
}

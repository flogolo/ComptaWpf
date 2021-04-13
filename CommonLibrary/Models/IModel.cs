
using System;
namespace CommonLibrary.Models
{
    /// <summary>
    /// Interface pour les modèles
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Identifiant de l'objet
        /// </summary>
        long Id { get; set;}

        DateTime? CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}

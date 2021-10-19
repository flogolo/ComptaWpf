using System.Collections.Generic;

namespace MaCompta.ViewModels
{
    public class VersionModifications
    {
        public string Version { get; set; }

        public List<string> Modifications { get; private set; }

        public VersionModifications(string version)
        {
            Version = version;
            Modifications = new List<string>();
        }
        public string VersionToString
        {
            get
            {
                return string.Format("Version v{0}", Version);
            }
        }
        public string ModificationsToString
        {
            get
            {
                return "- " + string.Join(".\r\n- ", Modifications) + ".";
            }
        }

        /// <summary>
        /// remplissage de la liste des versions
        /// </summary>
        public static void FillVersions(List<VersionModifications> versions)
        {
            var v7 = new VersionModifications("1.0.0.7");
            v7.Modifications.Add("Gestion et affichage des versions et modifications");
            versions.Add(v7);
            var v6 = new VersionModifications("1.0.0.6");
            v6.Modifications.Add("Test de la connexion effective à la base de données avant le chargement");
            versions.Add(v6);
            var v5 = new VersionModifications("1.0.0.5");
            v5.Modifications.Add("Possibilité de supprimer une opération prédéfinie");
            v5.Modifications.Add("Tri des opérations prédéfinies");
            versions.Add(v5);
            var v4b = new VersionModifications("1.0.0.4b");
            v4b.Modifications.Add("Le changement de compte d'une opération met à jour les soldes");
            versions.Add(v4b);
            var v4 = new VersionModifications("1.0.0.4");
            v4.Modifications.Add("Modification d'un virement avec la possibilité de mettre un compte vide");
            versions.Add(v4);
            var v3 = new VersionModifications("1.0.0.3");
            v3.Modifications.Add("Duplication de virement avec le type");
            versions.Add(v3);
            var version2 = new VersionModifications("1.0.0.2");
            version2.Modifications.Add("Modification fonctionnement du filtre");
            versions.Add(version2);
            var version1 = new VersionModifications("1.0.0.1");
            version1.Modifications.Add("Couleurs des boutons et dérouleurs");
            versions.Add(version1);
            var version = new VersionModifications("1.0.0.0");
            version.Modifications.Add("Après chaque filtrage, les opérations sont retriées dans l'ordre des dates");
            version.Modifications.Add("Les filtres sur les colonnes sont utilisables après filtrage avancé");
            version.Modifications.Add("Le changement de compte d'un opération affiche l'opération sur le compte cible s'il est ouvert");
            version.Modifications.Add("Couleurs des onglets");
            versions.Add(version);
        }
    }
}

using System;

namespace CommonLibrary.IOC
{
    /// <summary>
    /// Interface pour l'injection de contrôle
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Enregistrer un conteneur
        /// </summary>
        /// <typeparam name="TTypeToResolve"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        void Register<TTypeToResolve, TConcrete>();
        /// <summary>
        /// Enregistre un conteneur avec un cycle particulier
        /// </summary>
        /// <typeparam name="TTypeToResolve"></typeparam>
        /// <typeparam name="TConcrete"></typeparam>
        /// <param name="lifeCycle"></param>
        void Register<TTypeToResolve, TConcrete>(LifeCycle lifeCycle);
        /// <summary>
        /// Retrouve un conteneur
        /// </summary>
        /// <typeparam name="TTypeToResolve"></typeparam>
        /// <returns></returns>
        TTypeToResolve Resolve<TTypeToResolve>();
        /// <summary>
        /// Retrouve un conteneur
        /// </summary>
        /// <param name="typeToResolve"></param>
        /// <returns></returns>
        object Resolve(Type typeToResolve);
    }
}
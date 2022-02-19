using System;

namespace Garant.Platform.Core.Exceptions
{
    /// <summary>
    /// Исключение возникнет, если не были переданы обязательные параметры для списка подкатегорий франшиз.
    /// </summary>
    public class EmptyParamsFranchiseSubCategoryException : Exception
    {
        public EmptyParamsFranchiseSubCategoryException() : base(@"Не переданы обязательные параметры для получения списка подкатегорий франшиз") 
        {
        }
    }
}
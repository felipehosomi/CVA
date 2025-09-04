using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class GenreBLL
    {
        #region Atributos
        private GenreDAO GenreDAO { get; set; }
        #endregion

        #region Construtor
        public GenreBLL()
        {
            this.GenreDAO = new GenreDAO();
        }
        #endregion

        public List<GenreModel> Get()
        {
            try
            {
                return GenreDAO.Get();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

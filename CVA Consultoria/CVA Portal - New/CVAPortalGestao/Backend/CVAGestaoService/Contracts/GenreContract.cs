using BLL.Classes;
using MODEL.Classes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CVAGestaoService.Contracts
{
    [DataContract]
    public class GenreContract
    {
        private GenreBLL GenreBLL { get; set; }

        public GenreContract()
        {
            this.GenreBLL = new GenreBLL();
        }

        public List<GenreModel> Get()
        {
            return GenreBLL.Get();
        }
    }
}
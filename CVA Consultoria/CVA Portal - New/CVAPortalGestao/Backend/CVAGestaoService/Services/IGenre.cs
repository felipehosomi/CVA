using MODEL.Classes;
using System.Collections.Generic;
using System.ServiceModel;

namespace CVAGestaoService.Services
{
    [ServiceContract]
    public interface IGenre
    {
        [OperationContract]
        List<GenreModel> GetGenre();
    }
}
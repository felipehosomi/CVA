using System.Linq;

namespace CVA_Rep_DAL
{
    /// <summary>
    ///     Repositório EF para o objeto CVA_BAS
    /// </summary>
    public class CVA_BAS_Repository : GenericDataRepository<CVA_BAS>, ICVA_BAS_Repository
    {
        public CVA_BAS GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_FUNC
    /// </summary>
    public class CVA_FUNC_Repository : GenericDataRepository<CVA_FUNC>, ICVA_FUNC_Repository
    {
        public CVA_FUNC GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_OBJ
    /// </summary>
    public class CVA_OBJ_Repository : GenericDataRepository<CVA_OBJ>, ICVA_OBJ_Repository
    {
        public CVA_OBJ GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_REG
    /// </summary>
    public class CVA_REG_Repository : GenericDataRepository<CVA_REG>, ICVA_REG_Repository
    {
        public CVA_REG GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_REG_LOG
    /// </summary>
    public class CVA_REG_LOG_Repository : GenericDataRepository<CVA_REG_LOG>, ICVA_REG_LOG_Repository
    {
        public CVA_REG_LOG GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_STU
    /// </summary>
    public class CVA_STU_Repository : GenericDataRepository<CVA_STU>, ICVA_STU_Repository
    {
        public CVA_STU GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_TIM
    /// </summary>
    public class CVA_TIM_Repository : GenericDataRepository<CVA_TIM>, ICVA_TIM_Repository
    {
        public CVA_TIM GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_TIP_REG
    /// </summary>
    public class CVA_TIP_REG_Repository : GenericDataRepository<CVA_TIP_REG>, ICVA_TIP_REG_Repository
    {
        public CVA_TIP_REG GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }

    /// <summary>
    ///     Repositório EF para o objeto CVA_EML
    /// </summary>
    public class CVA_EML_Repository : GenericDataRepository<CVA_EML>, ICVA_EML_Repository
    {
        public CVA_EML GetSingle(int ID)
        {
            return GetAll().FirstOrDefault(x => x.ID == ID);
        }
    }
}
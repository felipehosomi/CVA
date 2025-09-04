namespace CVA_Rep_DAL
{
    /// <summary>
    ///     Inteface para o repositório CVA_BAS
    /// </summary>
    public interface ICVA_BAS_Repository : IGenericDataRepository<CVA_BAS>
    {
        CVA_BAS GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_FUNC
    /// </summary>
    public interface ICVA_FUNC_Repository : IGenericDataRepository<CVA_FUNC>
    {
        CVA_FUNC GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_OBJ
    /// </summary>
    public interface ICVA_OBJ_Repository : IGenericDataRepository<CVA_OBJ>
    {
        CVA_OBJ GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_REG
    /// </summary>
    public interface ICVA_REG_Repository : IGenericDataRepository<CVA_REG>
    {
        CVA_REG GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_REG_LOG
    /// </summary>
    public interface ICVA_REG_LOG_Repository : IGenericDataRepository<CVA_REG_LOG>
    {
        CVA_REG_LOG GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_STU
    /// </summary>
    public interface ICVA_STU_Repository : IGenericDataRepository<CVA_STU>
    {
        CVA_STU GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_TIM
    /// </summary>
    public interface ICVA_TIM_Repository : IGenericDataRepository<CVA_TIM>
    {
        CVA_TIM GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_TIP_REG
    /// </summary>
    public interface ICVA_TIP_REG_Repository : IGenericDataRepository<CVA_TIP_REG>
    {
        CVA_TIP_REG GetSingle(int ID);
    }

    /// <summary>
    ///     Inteface para o repositório CVA_EML
    /// </summary>
    public interface ICVA_EML_Repository : IGenericDataRepository<CVA_EML>
    {
        CVA_EML GetSingle(int ID);
    }
}
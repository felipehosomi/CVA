using CVA.Core.DIRF.AUXILIAR;
using CVA.Core.DIRF.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.Core.DIRF.DAO
{
    public class DirfDAO
    {
        #region Atributos
        string db { get; set; }
        SqlConnection conn { get; set; }
        SAPbobsCOM.Company _company { get; set; }
        #endregion

        #region Construtor
        public DirfDAO(string db)
        {
            this.db = db;
            this.conn = new SqlConnection();
        }

        #endregion

        public DataTable Get_RESPO(FiltroModel filtro)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT
                                            T0.CPF                            AS 'B'
			                                ,T0.FIRSTNAME + ' ' + T0.LASTNAME AS 'C'
			                                ,SUBSTRING(T0.OFFICETEL,0,3)      AS 'D'
			                                ,SUBSTRING(T0.OFFICETEL,3,9)      AS 'E'
			                                ,T0.OFFICEEXT                     AS 'F'
			                                ,T0.FAX	                          AS 'G'
			                                ,T0.EMAIL                         AS 'H'
			 
                                        FROM OHEM T0
			                            WHERE
			                                    T0.EMPID = {filtro.Representante}";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_DECPJ_A(FiltroModel filtro)
        {
            var tb = new DataTable();
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT DISTINCT
                                               T0.TAXIDNUM AS 'B',
                                        	   T0.BPLNAME  AS 'C',
                                        	   T1.CODE     AS 'D'
                                        
                                        FROM OBPL T0
                                        
                                        INNER JOIN OBNI T1 ON T0.DECLTYPE = T1.CODE";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tb.Load(dr);
                        conn.Close();
                    }
                    return tb;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_DECPJ_B(FiltroModel filtro)
        {
            var tb = new DataTable();
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;

                    cmd.CommandText = $@"SELECT
                                            T0.CPF   AS 'E'

                                        FROM OHEM T0
                               WHERE
                                   T0.EMPID = {filtro.Representante}";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        tb.Load(dr);
                        conn.Close();
                    }

                    return tb;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_IDREC(FiltroModel filtro)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT DISTINCT 
                                            T0.OFFCLCODE AS 'B'
                                         FROM OWHT T0
                                         RIGHT JOIN PCH5 T1 ON T1.WTCODE = T0.WTCODE
                                         INNER JOIN OPCH T2 ON T2.DOCENTRY = T1.ABSENTRY

                                         WHERE YEAR(T2.DOCDATE) = {filtro.AnoReferencia}
                                           AND T2.CANCELED ='N'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_BPJDEC(FiltroModel filtro, string codigo)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT DISTINCT
                                            T3.OFFCLCODE
                                            ,T4.TAXID0   AS 'B'
                                            ,T0.CARDNAME AS 'C'
                                        FROM OCRD T0

                                        INNER JOIN OPCH T1 ON T1.CARDCODE = T0.CARDCODE
                                        INNER JOIN PCH5 T2 ON T2.ABSENTRY = T1.DOCENTRY
                                        INNER JOIN OWHT T3 ON T3.WTCODE = T2.WTCODE
                                        INNER JOIN PCH12 T4 ON T4.DOCENTRY = T1.DOCENTRY

                                        WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                                        AND T3.OFFCLCODE = {codigo}
                                        AND T1.CANCELED = 'N'
                                        ORDER BY T3.OFFCLCODE";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_RTRT(FiltroModel filtro, object codigo, object Pn)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT
	(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 1
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'B'
	
	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 2
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'C'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 3
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'D'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 4
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'E'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 5
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'F'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 6
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'G'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 7
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'H'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 8
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'I'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 9
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'J'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 10
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'K'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 11
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'L'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.TaxbleAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 12
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'M'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_RTIRF(FiltroModel filtro, object codigo, object Pn)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT
	(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 1
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'B'
	
	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 2
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'C'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 3
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'D'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 4
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'E'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 5
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'F'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 6
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'G'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 7
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'H'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 8
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'I'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 9
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'J'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 10
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'K'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 11
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'L'

	,(SELECT DISTINCT CONVERT(DECIMAL(19,2),SUM(T0.WTAmnt))
                     FROM PCH5 T0

                     INNER JOIN OPCH T1 ON T1.DOCENTRY = T0.ABSENTRY
                     INNER JOIN OWHT T2 ON T2.WTCODE = T0.WTCODE
                        
                     WHERE YEAR(T1.DOCDATE) =  {filtro.AnoReferencia}
                       AND T2.OFFCLCODE = {codigo.ToString()} 
		               AND MONTH(T1.DOCDATE) = 12
					   AND T1.CANCELED = 'N'
					   AND T1.CARDNAME = '{Pn.ToString()}') AS 'M'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_Representantes()
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT EMPID                      AS 'ID',
                                                FIRSTNAME + ' ' + LASTNAME AS 'NOME',
                                                CPF                        AS 'CPF'

                                         FROM OHEM WHERE REPLEGAL = 'Y'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public DataTable Get_RepresentanteInfo(int id)
        {
            try
            {
                using (conn)
                {
                    OpenConnection();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $@"SELECT EMPID AS 'ID',
                                                FIRSTNAME + ' ' + LASTNAME AS 'NOME'

                                         FROM OHEM WHERE REPLEGAL = 'Y'";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        var tb = new DataTable();
                        tb.Load(dr);
                        conn.Close();
                        return tb;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private void OpenConnection()
        {
            conn.Close();

            conn.ConnectionString = $@"Data Source = SAPQAS; Database = {this.db}; User Id = sa; Password = sa@#Atlantic;";
            conn.Open();
        }
    }
}

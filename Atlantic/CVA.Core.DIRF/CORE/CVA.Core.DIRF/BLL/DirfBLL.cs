using System;
using System.Collections.Generic;
using CVA.Core.DIRF.AUXILIAR;
using CVA.Core.DIRF.DAO;
using CVA.Core.DIRF.MODEL;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;

namespace CVA.Core.DIRF.BLL
{
    public class DirfBLL
    {
        #region Atributos
        public DirfDAO _dao { get; set; }
        public Writer _writer { get; set; }
        #endregion

        #region Construtor
        public DirfBLL()
        {
            this._writer = new Writer();
        }
        #endregion


        public bool Gerar_Arquivo(FiltroModel filtro, string db)
        {
            this._dao = new DirfDAO(db);
            try
            {
                var DirfModel = new DirfModel();

                DirfModel.DIRF = Gera_LinhaDIRF(filtro);
                DirfModel.RESPO = Gera_LinhaRESPO(_dao.Get_RESPO(filtro));
                DirfModel.DECPJ = Gera_LinhaDECPJ(filtro, _dao.Get_DECPJ_A(filtro), _dao.Get_DECPJ_B(filtro));
                DirfModel.IDREC = new List<IdrecModel>();

                var idrecResult = _dao.Get_IDREC(filtro);

                for (int i = 0; i < idrecResult.Rows.Count; i++)
                {
                    var IdrecModel = new IdrecModel();

                    IdrecModel.IDREC = Gera_LinhaIDREC(idrecResult.Rows[i]);
                    IdrecModel.Info = new List<InfoModel>();

                    var infoResult = _dao.Get_BPJDEC(filtro, idrecResult.Rows[i]["B"].ToString());

                    for (int j = 0; j < infoResult.Rows.Count; j++)
                    {
                        var InfoModel = new InfoModel()
                        {
                            BPJDEC = Gera_LinhaBPJDEC(infoResult.Rows[j]),
                            RTRT = Gera_LinhaRTRT(_dao.Get_RTRT(filtro, idrecResult.Rows[i]["B"], infoResult.Rows[j]["C"])),
                            RTIRF = Gera_LinhaRTIRF(_dao.Get_RTIRF(filtro, idrecResult.Rows[i]["B"], infoResult.Rows[j]["C"]))
                        };
                        IdrecModel.Info.Add(InfoModel);
                    }
                    DirfModel.IDREC.Add(IdrecModel);
                }

                return _writer.Write(DirfModel);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string Gera_LinhaDIRF(FiltroModel filtro)
        {
            string A, B, C, D, E, F;

            A = ("DIRF|");
            B = ($@"{filtro.AnoReferencia}|");
            C = ($@"{filtro.AnoCalendario}|");
            D = ($@"{filtro.Tipo}|");
            E = ($@"{filtro.Recibo}|");
            F = ("P49VS72|");

            return A + B + C + D + E + F;
        }

        public string Gera_LinhaRESPO(DataTable result)
        {
            string A, B, C, D, E, F, G, H;

            A = ("RESPO|");
            B = TrataString(Regex.Replace(($@"{result.Rows[0]["B"].ToString()}"), "[^0-9a-zA-Z]+", ""), 11) + "|";
            C = ($@"{TrataString(result.Rows[0]["C"].ToString(), 60)}|");
            D = ($@"{TrataString(result.Rows[0]["D"].ToString(), 2)}|");
            E = ($@"{TrataString(result.Rows[0]["E"].ToString(), 9)}|");
            F = ($@"{TrataString(result.Rows[0]["F"].ToString(), 6)}|");
            G = ($@"{TrataString(result.Rows[0]["G"].ToString(), 9)}|");
            H = ($@"{TrataString(result.Rows[0]["H"].ToString(), 60)}|");

            return A + B + C + D + E + F + G + H;
        }

        public string Gera_LinhaDECPJ(FiltroModel filtro, DataTable result_A, DataTable result_B)
        {
            string A, B, C, D, E, F, G, H, I, J, K, L, M;

            A = ("DECPJ|");
            B = TrataString(Regex.Replace(($@"{result_A.Rows[0]["B"].ToString()}"), "[^0-9a-zA-Z]+", ""), 14) + "|";
            C = TrataString(FormataNomeEmpresa($@"{result_A.Rows[0]["C"].ToString()}"), 150) + "|";
            D = TrataString(Regex.Replace(($@"{result_A.Rows[0]["D"].ToString()}"), "[^0-9a-zA-Z]+", ""), 1) + "|";
            E = TrataString(Regex.Replace(($@"{result_B.Rows[0]["E"].ToString()}"), "[^0-9a-zA-Z]+", ""), 11) + "|";
            F = ($@"{filtro.DECPJ_F}|");
            G = ($@"{filtro.DECPJ_G}|");
            H = ($@"{filtro.DECPJ_H}|");
            I = ($@"{filtro.DECPJ_I}|");
            J = ($@"{filtro.DECPJ_J}|");
            K = ($@"{filtro.DECPJ_K}|");
            L = ($@"{filtro.DECPJ_L}|");
            M = ($@"{filtro.DECPJ_M}|");


            return A + B + C + D + E + F + G + H + I + J + K + L + M;
        }

        public string Gera_LinhaIDREC(DataRow result)
        {
            string A, B;

            A = ("IDREC|");
            B = ($@"{TrataString(result["B"].ToString(), 5)}|");

            return A + B;
        }

        public string Gera_LinhaBPJDEC(DataRow result)
        {
            string A, B, C;

            A = ("BPJDEC|");

            B = TrataString(Regex.Replace(($@"{result["B"].ToString()}"), "[^0-9a-zA-Z]+", ""), 14) + "|";
            C = TrataString(FormataNomeEmpresa($@"{result["C"].ToString()}"), 150) + "|";

            return A + B + C;
        }

        public string Gera_LinhaRTRT(DataTable result)
        {
            string A, B, C, D, E, F, G, H, I, J, K, L, M;

            A = ("RTRT|");
            B = Insere_Zeros(($@"{result.Rows[0]["B"].ToString()}|"));
            C = Insere_Zeros(($@"{result.Rows[0]["C"].ToString()}|"));
            D = Insere_Zeros(($@"{result.Rows[0]["D"].ToString()}|"));
            E = Insere_Zeros(($@"{result.Rows[0]["E"].ToString()}|"));
            F = Insere_Zeros(($@"{result.Rows[0]["F"].ToString()}|"));
            G = Insere_Zeros(($@"{result.Rows[0]["G"].ToString()}|"));
            H = Insere_Zeros(($@"{result.Rows[0]["H"].ToString()}|"));
            I = Insere_Zeros(($@"{result.Rows[0]["I"].ToString()}|"));
            J = Insere_Zeros(($@"{result.Rows[0]["J"].ToString()}|"));
            K = Insere_Zeros(($@"{result.Rows[0]["K"].ToString()}|"));
            L = Insere_Zeros(($@"{result.Rows[0]["L"].ToString()}|"));
            M = Insere_Zeros(($@"{result.Rows[0]["M"].ToString()}|"));

            return A + B + C + D + E + F + G + H + I + J + K + L + M;
        }

        public string Gera_LinhaRTIRF(DataTable result)
        {
            string A, B, C, D, E, F, G, H, I, J, K, L, M;

            A = ("RTIRF|");

            B = Insere_Zeros(($@"{result.Rows[0]["B"].ToString()}"));
            C = Insere_Zeros(($@"{result.Rows[0]["C"].ToString()}"));
            D = Insere_Zeros(($@"{result.Rows[0]["D"].ToString()}"));
            E = Insere_Zeros(($@"{result.Rows[0]["E"].ToString()}"));
            F = Insere_Zeros(($@"{result.Rows[0]["F"].ToString()}"));
            G = Insere_Zeros(($@"{result.Rows[0]["G"].ToString()}"));
            H = Insere_Zeros(($@"{result.Rows[0]["H"].ToString()}"));
            I = Insere_Zeros(($@"{result.Rows[0]["I"].ToString()}"));
            J = Insere_Zeros(($@"{result.Rows[0]["J"].ToString()}"));
            K = Insere_Zeros(($@"{result.Rows[0]["K"].ToString()}"));
            L = Insere_Zeros(($@"{result.Rows[0]["L"].ToString()}"));
            M = Insere_Zeros(($@"{result.Rows[0]["M"].ToString()}"));

            return A + B + C + D + E + F + G + H + I + J + K + L + M;
        }

        private string Insere_Zeros(string linha)
        {
            linha = Regex.Replace(linha, "[^0-9a-zA-Z]+", "");

            var comprimento = (13 - linha.Length);          

            for (int i = 0; i < comprimento; i++)
            {
                linha = "0" + linha;
            }
            return linha + "|";
        }

        public List<RepresentanteModel> Get_Representantes(string db)
        {
            this._dao = new DirfDAO(db);
            var result = _dao.Get_Representantes();

            var modelList = new List<RepresentanteModel>();
            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new RepresentanteModel
                {
                    Id = result.Rows[i]["ID"].ToString(),
                    Nome = result.Rows[i]["NOME"].ToString(),
                    CPF = result.Rows[i]["CPF"].ToString()
                };

                modelList.Add(model);
            }

            return modelList;
        }

        public string TrataString(string s, int c)
        {
            if (s.Length >= c)
                return s.Substring(0, c);
            else
                return s;
        }

        public string FormataNomeEmpresa(string s)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = s.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            var nome = sbReturn.ToString();
            for (int i = nome.Length; i < 150; i++)
            {
                nome = nome + " ";
            }

            return nome;            
        }
    }
}

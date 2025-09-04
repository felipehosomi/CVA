using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class NoteBLL
    {
        #region Atributos
        private NoteDAO _noteDAO { get; set; }
        private UserBLL _userBLL { get; set; }
        private ProjectBLL _projectBLL { get; set; }
        private CalendarBLL _calendarBLL { get; set; }
        private SpecialtyBLL _specialtyBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        private AuthorizationBLL _authorizationBLL { get; set; }
        #endregion

        #region Construtor
        public NoteBLL()
        {
            _noteDAO = new NoteDAO();
            _userBLL = new UserBLL();
            _projectBLL = new ProjectBLL();
            _calendarBLL = new CalendarBLL();
            _specialtyBLL = new SpecialtyBLL();
            _collaboratorBLL = new CollaboratorBLL();
            _authorizationBLL = new AuthorizationBLL();
        }
        #endregion

        public List<NoteModel> Get_UserNotes(int id, int mes, int ano)
        {
            var result = _noteDAO.Get_UserNotes(id, mes, ano);

            return Load_Model(result);
        }

        public List<NoteModel> Search(NoteFilterModel model)
        {
            try
            {
                if (model.interno)
                    model.UserId = _collaboratorBLL.GetByUserID(model.UserId).Id;

                var result = _noteDAO.Search(model);

                var list = new List<NoteModel>();

                for (var i = 0; i < result.Rows.Count; i++)
                {
                    var note = new NoteModel();
                    note.Date = Convert.ToDateTime(result.Rows[i]["Date"].ToString());
                    note.User = new UserModel { Name = result.Rows[i]["User.Name"].ToString() };
                    note.Project = new ProjectModel()
                    {
                        Nome = result.Rows[i]["Project.Nome"].ToString(),
                        Codigo = result.Rows[i]["Project.Codigo"].ToString(),
                        Tag = result.Rows[i]["Project.Tag"].ToString(),

                        Cliente = new ClientModel
                        {
                            Name = result.Rows[i]["Client.Name"].ToString()
                        },
                        TipoProjeto = new ProjectTypeModel
                        {
                            Nome = result.Rows[i]["ProjectType.Name"].ToString()
                        }
                    };

                    note.InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString());
                    note.FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString());
                    note.Description = result.Rows[i]["Description"].ToString();
                    note.Requester = result.Rows[i]["Requester"].ToString();
                    note.Specialty = new SpecialtyModel() { Name = result.Rows[i]["Specialty.Name"].ToString() };
                    if (result.Rows[i]["IntervalHour"] == DBNull.Value || result.Rows[i]["IntervalHour"] == null)
                        note.IntervalHour = null;
                    else
                        note.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());

                    if (!String.IsNullOrEmpty(result.Rows[i]["Ticket.NumTck"].ToString()))
                    {
                        note.Ticket = new TicketModel()
                        {
                            Code = Convert.ToInt32(result.Rows[i]["Ticket.NumTck"].ToString())
                        };
                    }
                    else
                        note.Ticket = new TicketModel()
                        {
                            Code = 0
                        };

                    list.Add(note);
                }

                list = Load_NoteHours(list);

                return list;
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public MessageModel Save(NoteModel model)
        {
            if (model.Id == 0)
                return Insert(model);
            else
                return Update(model);
        }

        public MessageModel Insert(NoteModel model)
        {
            try
            {
                //var Col = _userBLL.GetCollaborator(model.User.Id);
                var Col = _userBLL.GetCollaborator(model.USR);
                var isValid = ValidateModel(model, Col);
                if (isValid != null)
                    return isValid;

                model.Value = "0";
                if (model.Ticket == null)
                {
                    model.Ticket = new TicketModel();
                    model.Ticket.Code = 0;
                }

                var HorasFase = Calculate_StepHours(model);
                var HorasEspecialidade = Calculate_SpecialtyHours(model);
                var HorasRecurso = Calculate_ResourceNotes(model);

                if (_noteDAO.Insert(model, HorasFase, HorasEspecialidade, HorasRecurso) > 0)
                    return MessageBLL.Generate("Apontamento registrado com sucesso!", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao salvar o apontamento, informe o administrador do sistema", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }



        public MessageModel Update(NoteModel model)
        {
            try
            {
                //var Col = _userBLL.GetCollaborator(model.User.Id);
                var Col = _userBLL.GetCollaborator(model.USR);
                var isValid = ValidateModel(model, Col);
                if (isValid != null)
                    return isValid;

                model.Value = "0";
                if (model.Ticket == null)
                {
                    model.Ticket = new TicketModel();
                    model.Ticket.Code = 0;
                }

                var HorasFase = Calculate_StepHours2(model);
                var HorasEspecialidade = Calculate_SpecialtyHours2(model);
                //var HorasRecurso = Calculate_ResourceNotes2(model);
                var HorasRecurso = Calculate_ResourceNotes3(model);

                if (_noteDAO.Update(model, HorasFase, HorasEspecialidade, HorasRecurso) > 0)
                    return MessageBLL.Generate("Apontamento atualizado com sucesso", 0);
                else
                    return MessageBLL.Generate("Ocorreu um erro ao salvar o apontamento, informe o administrador do sistema", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Remove(NoteModel model)
        {
            try
            {

                var HorasFase = Calculate_StepHours2(model);
                var HorasEspecialidade = Calculate_SpecialtyHours2(model);
                var horasRecurso = Calculate_ResourceNotes2(model);
                if (_noteDAO.Remove(model, HorasFase, HorasEspecialidade, horasRecurso) > 0)
                    return MessageBLL.Generate("Apontamento removido", 0);
                else
                    return MessageBLL.Generate("Erro ao excluir apontamento", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        #region Auxiliares
        private MessageModel ValidateModel(NoteModel model, CollaboratorModel Col)
        {
            //Valida se tudo foi preenchido-----------------------------------------------------//
            if (model.Project == null || model.Project.Id == 0)                                 //
                return MessageBLL.Generate("Preencha o campo 'Projeto'", 99, true);             //
            if (model.Step == null || model.Step.Id == 0)                                       //
                return MessageBLL.Generate("Preencha o campo 'Fase'", 99, true);                //
            if (model.InitHour == null || !model.InitHour.HasValue)                             //
                return MessageBLL.Generate("Preencha o campo 'Hora Ínicio'", 99, true);         //
            if (model.FinishHour == null || !model.FinishHour.HasValue)                         //
                return MessageBLL.Generate("Preencha o campo 'Hora Término'", 99, true);        //
            if (model.Date == null || model.Date == new DateTime())                             //
                return MessageBLL.Generate("Preencha o campo 'Data'", 99, true);                //
            if (model.Requester == null || !model.FinishHour.HasValue)                          //
                return MessageBLL.Generate("Preencha o campo 'Solicitante'", 99, true);         //
            if (model.Specialty == null || model.Specialty.Id == 0)                             //
                return MessageBLL.Generate("Preencha o campo 'Especialidade'", 99, true);       //
            if (model.Description == null || !model.FinishHour.HasValue)                        //
                return MessageBLL.Generate("Preencha o campo 'Descrição'", 99, true);           //
            //----------------------------------OK----------------------------------------------//


            //Valida horas sobrepostas-----------------------------------------------------------------------//
            if (model.InitHour >= model.FinishHour)                                                          //
                return MessageBLL.Generate("Apontamento inválido. Verifique as horas informadas", 99, true); //
            //-----------------------------------------------------------------------------------------------//

            //Valida horas sobrepostas----------------------------------------------------------------//
            var savedNotes = Load_DayNotes(_noteDAO.Get_DayNotes(model.USR, model.Date.Date));    //
                                                                                                      //
            foreach (var item in savedNotes)                                                          //
            {                                                                                         //
                if (item.Id != model.Id)                                                              //
                {                                                                                     //
                    if (model.InitHour == item.InitHour)  //                                          //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                  //
                                                                                                      //
                    if ((model.InitHour < item.InitHour) && (model.FinishHour > item.InitHour))       //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                  //
                                                                                                      //
                    if ((model.InitHour > item.InitHour) && (model.InitHour < item.FinishHour))       //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                  //
                }                                                                                     //
            }                                                                                         //
            //----------------------------------OK----------------------------------------------------//


            //Valida intervalo obrigatório acima de 8 horas-----------------------------------------------------------------------------------------------//
            TimeSpan horaFinal, horaInicial, intervalo;                                                                                                   //
                                                                                                                                                          //
            horaInicial = TimeSpan.Parse(model.InitHour.Value.ToLongTimeString());                                                                        //
            horaFinal = TimeSpan.Parse(model.FinishHour.Value.ToLongTimeString());                                                                        //
                                                                                                                                                          //
            if (model.IntervalHour != null)                                                                                                               //
                intervalo = TimeSpan.Parse(model.IntervalHour.Value.ToLongTimeString());                                                                  //
            else                                                                                                                                          //
                intervalo = new TimeSpan();                                                                                                               //
                                                                                                                                                          //
            if ((horaFinal - horaInicial).TotalHours >= 8 && (model.IntervalHour == null || intervalo.TotalHours < 1))                                    //
                return MessageBLL.Generate("Em jornadas de trabalho superiores a 8 horas, é necessário apontar pelo menos 1 hora de intervalo", 99, true);//
                                                                                                                                                          //-----------------------------------OK-------------------------------------------------------------------------------------------------------//

            //Valida apontamento com data futura----------------------------------------------------------//
            if (model.Date.Date > DateTime.Today)                                                         //
                return MessageBLL.Generate("Impossível realizar apontamento com data futura.", 99, true); //
                                                                                                          //--------------------------------------OK----------------------------------------------------//


            //Carrega as listas de dias e horas autorizadas para apontamento---------------------------------------------------------//
            var diasAutorizados = _authorizationBLL.Get_DiasAutorizados(Col.Id);                                                     //
            var horasAutorizadas = _authorizationBLL.Get_HorasAutorizadas(Col.Id);                                                   //
                                                                                                                                     //
            var limiteHoras = _authorizationBLL.Get_LimiteHoras(Col.Id);                                                             //
            var feriados = _calendarBLL.Get_All_Holidays(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);//
                                                                                                                                     //------------------------------OK---------------------------------------------------------------------------------------//

            //Valida apontamento em até 2 dias úteis, finais de semana e feriados (caso não estejam autorizado)//
            var diaValido = _calendarBLL.Search_ValidDay();                                                                 //
                                                                                                               //
            if (model.Date < diaValido)                                                                        //
            {                                                                                                  //
                var valid = false;                                                                             //
                foreach (var item in diasAutorizados)                                                          //
                {                                                                                              //
                    if (model.Date.Date >= item.De && model.Date.Date <= item.Ate)                             //
                    {                                                                                          //
                        valid = true;                                                                          //
                        break;                                                                                 //
                    }                                                                                          //
                }                                                                                              //
                if (!valid)                                                                                    //
                    return MessageBLL.Generate("O prazo para apontamento é de até 2 dias úteis", 99, true);    //
            }                                                                                                  //
                                                                                                               //
            if (model.Date.DayOfWeek == DayOfWeek.Saturday || model.Date.DayOfWeek == DayOfWeek.Sunday)        //
            {                                                                                                  //
                var valid = false;                                                                             //
                foreach (var item in diasAutorizados)                                                          //
                {                                                                                              //
                    if (model.Date.Date >= item.De && model.Date.Date <= item.Ate)                             //
                    {                                                                                          //
                        valid = true;                                                                          //
                        break;                                                                                 //
                    }                                                                                          //
                }                                                                                              //
                if (!valid)                                                                                    //
                    return MessageBLL.Generate("Não é possível apontar em um final de semana", 99, true);      //
            }                                                                                                  //
                                                                                                               //
            foreach (var feriado in feriados)                                                                  //
            {                                                                                                  //
                if (model.Date.Date == feriado.Date.Date)                                                      //
                {                                                                                              //
                    var valid = false;                                                                         //
                    foreach (var item in diasAutorizados)                                                      //
                    {                                                                                          //
                                                                                                               //
                        if (model.Date.Date >= item.De && model.Date.Date <= item.Ate)                         //
                        {                                                                                      //
                            valid = true;                                                                      //
                            break;                                                                             //
                        }                                                                                      //
                    }                                                                                          //
                    if (!valid)                                                                                //
                        return MessageBLL.Generate("Não é possível apontar em feriados", 99, true);            //
                }                                                                                              //
            }                                                                                                  //
            //----------------------------------------OK-------------------------------------------------------//

            //Valida apontamento conforme limite de horas diárias-------------------------------------------------//
            var apontamentosSalvos = Load_DayNotes(_noteDAO.Get_DayNotes(model.USR, model.Date.Date.Date));  //
                                                                                                                 //
            var totalTrabalhado = Calculate_NoteHours(model);
            if (totalTrabalhado <= 0)
                return MessageBLL.Generate("Horário de apontamento inválido. Por favor, verifique os campos", 99, true);

            foreach (var item in apontamentosSalvos)                                                              //
            {                                                                                                     //
                if (item.Id != model.Id)                                                                          //
                    totalTrabalhado += Calculate_NoteHours(item);                                             //
            }                                                                                                     //
                                                                                                                  //
            if (totalTrabalhado > TimeSpan.Parse(Col.LimiteHoras.Value.ToLongTimeString()).TotalHours)            //
            {                                                                                                     //
                var valid = false;                                                                                //
                foreach (var item in horasAutorizadas)                                                            //
                {                                                                                                 //
                    if (model.Date.Date == item.Data.Date)                                                        //
                    {                                                                                             //
                        if (totalTrabalhado <= TimeSpan.Parse(item.Horas.ToLongTimeString()).TotalHours)          //
                        {                                                                                         //
                            valid = true;                                                                         //
                            break;                                                                                //
                        }                                                                                         //
                    }                                                                                             //
                }                                                                                                 //
                if (!valid)                                                                                       //
                    return MessageBLL.Generate("Você excedeu o limite de horas diárias para apontamento." +       //
                        " Entre em contato com o responsável para que seja feita a autorização", 99, true);       //
            }

            var HorasFase = Calculate_ResourceNotes(model);

            var LimiteHorasFase = _projectBLL.Get_LimiteHorasFase(model);
            if ((HorasFase) > LimiteHorasFase)
                return MessageBLL.Generate("Você excedeu o limite de horas previstas para a conclusão " +       //
                       "desta fase do projeto. Por favor, entre em contato com o responsável.", 99, true);       //
                                                                                                                 //
                                                                                                                 //-----------------------------------OK---------------------------------------------------------------//

            if (model.Date.Date.Month != DateTime.Today.Month && model.Date.Hour >= 12)
            {
                var valid = false;                                                                             //
                foreach (var item in diasAutorizados)                                                          //
                {                                                                                              //
                    if (model.Date.Date >= item.De && model.Date.Date <= item.Ate)                             //
                    {                                                                                          //
                        valid = true;                                                                          //
                        break;                                                                                 //
                    }                                                                                          //
                }                                                                                              //
                if (!valid)                                                                                    //
                    return MessageBLL.Generate("Período fechado para lançamento de apontamentos.", 99, true);
            }


            //Validações diversas-----------------------------------------------------------------------//
            if (_projectBLL.IsAMS(model.Project.Id) == "1" && model.Ticket == null)                     //
                return MessageBLL.Generate("Projeto do tipo AMS: informe o N° do chamado.", 99, true);  //
                                                                                                        //-------------------------------------OK---------------------------------------------------//

            return null;
        }


        private Double Calculate_StepHours(NoteModel model)
        {
            var result = _noteDAO.Get_StepNotes(model.Project.Id, model.Step.Id);

            var TotalHoras = Calculate_NoteHours(model);
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            return TotalHoras;
        }



        private Double Calculate_StepHours2(NoteModel model)
        {
            var result = _noteDAO.Get_StepNotes(model.Project.Id, model.Step.Id);

            var TotalHoras = 0.0;
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            TotalHoras -= Calculate_NoteHours(model);
            return TotalHoras;
        }

        private Double Calculate_SpecialtyHours(NoteModel model)
        {
            var result = _noteDAO.Get_SpecialtyNotes(model.Project.Id, model.Specialty.Id);

            var TotalHoras = Calculate_NoteHours(model);
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            return TotalHoras;
        }

        private Double Calculate_ResourceNotes(NoteModel model)
        {
            var result = _noteDAO.Get_ResourceNotes(model.Project.Id, model.Step.Id, model.Specialty.Id, model.USR);

            var TotalHoras = Calculate_NoteHours(model);
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            return TotalHoras;
        }

        private Double Calculate_ResourceNotes2(NoteModel model)
        {
            var result = _noteDAO.Get_ResourceNotes(model.Project.Id, model.Step.Id, model.Specialty.Id, model.USR);

            var TotalHoras = 0.0;
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            TotalHoras -= Calculate_NoteHours(model);
            return TotalHoras;
        }

        private Double Calculate_ResourceNotes3(NoteModel model)
        {
            DataTable result = _noteDAO.Get_ResourceNotes(model.Project.Id, model.Step.Id, model.Specialty.Id, model.USR);

            var TotalHoras = 0.0;
            string where = $"ID <> {model.Id}";
            DataRow[] resultRows = result.Select(where);

            foreach (DataRow item in resultRows)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(item["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(item["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(item["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            TotalHoras += Calculate_NoteHours(model);
            return TotalHoras;
        }



        private Double Calculate_SpecialtyHours2(NoteModel model)
        {
            var result = _noteDAO.Get_SpecialtyNotes(model.Project.Id, model.Specialty.Id);

            var TotalHoras = 0.0;
            for (var i = 0; i < result.Rows.Count; i++)
            {
                var apon = new NoteModel
                {
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                TotalHoras += Calculate_NoteHours(apon);
            }

            TotalHoras -= Calculate_NoteHours(model);
            return TotalHoras;
        }

        private DateTime? Adjust_Date(DateTime? date)
        {
            if (!date.HasValue)
                return null;
            else
                return date;
        }

        private List<NoteModel> Load_Model(DataTable result)
        {
            try
            {
                var list = new List<NoteModel>();

                for (var i = 0; i < result.Rows.Count; i++)
                {
                    var model = new NoteModel()
                    {
                        Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                        User = new UserModel { Name = result.Rows[i]["User.Name"].ToString() },

                        Description = result.Rows[i]["Description"].ToString(),
                        ProvidedHours = "168:00",

                        Requester = result.Rows[i]["Requester"].ToString(),

                        StatusPDA = "Ativo",

                        Date = Convert.ToDateTime(result.Rows[i]["Date"].ToString()),
                        InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                        FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString()),

                        Project = new ProjectModel()
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Project.Id"].ToString()),
                            Nome = result.Rows[i]["Project.Nome"].ToString(),
                            Codigo = result.Rows[i]["Project.Codigo"].ToString(),
                            Tag = result.Rows[i]["Project.Tag"].ToString(),

                            Cliente = new ClientModel
                            {
                                Id = Convert.ToInt32(result.Rows[i]["Client.Id"].ToString()),
                                Name = result.Rows[i]["Client.Name"].ToString()
                            },
                            TipoProjeto = new ProjectTypeModel
                            {
                                Id = Convert.ToInt32(result.Rows[i]["ProjectType.Id"].ToString()),
                                Nome = result.Rows[i]["ProjectType.Name"].ToString()
                            }
                        },
                        Specialty = new SpecialtyModel()
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Specialty.Id"]),
                            Name = result.Rows[i]["Specialty.Name"].ToString()
                        },

                        Step = new StepModel
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Step.Id"].ToString()),
                            Nome = result.Rows[i]["Step.Name"].ToString()
                        },
                    };


                    /*Extras*/
                    if (result.Rows[i]["IntervalHour"] == DBNull.Value || result.Rows[i]["IntervalHour"] == null)
                        model.IntervalHour = null;
                    else
                        model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());

                    if (!String.IsNullOrEmpty(result.Rows[i]["Ticket.Id"].ToString()))
                    {
                        model.Ticket = new TicketModel()
                        {
                            Id = Convert.ToInt32(result.Rows[i]["Ticket.Id"]),
                            Code = Convert.ToInt32(result.Rows[i]["Ticket.NumTck"].ToString())
                        };
                    }
                    else
                        model.Ticket = new TicketModel()
                        {
                            Id = 0,
                            Code = 0
                        };

                    list.Add(model);
                }

                list = Load_NoteHours(list);

                return list;

            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        private List<NoteModel> Load_DayNotes(DataTable result)
        {
            var modelList = new List<NoteModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new NoteModel
                {
                    Id = Convert.ToInt32(result.Rows[i]["Id"].ToString()),
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                if (!String.IsNullOrEmpty(result.Rows[i]["IntervalHour"].ToString()))
                    model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());

                modelList.Add(model);
            }
            return modelList;
        }

        private List<NoteModel> Load_NoteHours(List<NoteModel> list)
        {
            var total = new TimeSpan();

            foreach (var item in list)
            {
                var period = item.FinishHour.Value - item.InitHour.Value;

                if (item.IntervalHour.HasValue)
                    period = period.Add(-item.IntervalHour.Value.TimeOfDay);

                item.TotalLine = $"{((int)period.TotalHours).ToString().PadLeft(2, '0')}:{period:mm}";
                total = total.Add(period);
            }

            foreach (var item in list)
            {
                item.TotalHours = $"{((int)total.TotalHours).ToString().PadLeft(2, '0')}:{total:mm}";
                item.IndicatedHours = item.TotalHours;
            }

            return list;                                                                                                                                                                                                                                                                         //    if ((horaFinal - horaInicial).TotalHours >= 8 && (model.IntervalHour == null || intervalo.TotalHours < 1))    
        }

        private Double Calculate_NoteHours(NoteModel model)
        {
            TimeSpan InitialHour, Interval, FinishHour;

            InitialHour = TimeSpan.Parse(model.InitHour.Value.ToLongTimeString());
            FinishHour = TimeSpan.Parse(model.FinishHour.Value.ToLongTimeString());

            if (model.IntervalHour.HasValue)
                Interval = TimeSpan.Parse(model.IntervalHour.Value.ToLongTimeString());
            else
                Interval = new TimeSpan();

            return (FinishHour.TotalHours - Interval.TotalHours - InitialHour.TotalHours);
        }
        #endregion
    }
}
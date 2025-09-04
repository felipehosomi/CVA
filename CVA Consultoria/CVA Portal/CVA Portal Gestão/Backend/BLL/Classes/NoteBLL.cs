using System;
using System.Collections.Generic;
using System.Data;
using DAO.Classes;
using MODEL.Classes;
using System.Linq;

namespace BLL.Classes
{
    public class NoteBLL
    {
        #region Atributos
        private NoteDAO _noteDAO { get; set; }
        private UserBLL _userBLL { get; set; }
        private StatusBLL _statusBLL { get; set; }
        private ProjectBLL _projectBLL { get; set; }
        private ProjectStepBLL _projectStepBLL { get; set; }
        private CalendarBLL _calendarBLL { get; set; }
        private SubPeriodBLL _subPeriodBLL { get; set; }
        private CollaboratorBLL _collaboratorBLL { get; set; }
        private SpecialtyBLL _specialtyBLL { get; set; }
        private AuthorizationBLL _authorizationBLL { get; set; }
        #endregion

        #region Construtor
        public NoteBLL()
        {
            _noteDAO = new NoteDAO();
            _userBLL = new UserBLL();
            _projectStepBLL = new ProjectStepBLL();
            _calendarBLL = new CalendarBLL();
            _subPeriodBLL = new SubPeriodBLL();
            _collaboratorBLL = new CollaboratorBLL();
            _specialtyBLL = new SpecialtyBLL();
            _authorizationBLL = new AuthorizationBLL();
        }
        #endregion

        //------------------------------------------------------------------------------------//
        public List<NoteModel> Get_UserNotes(int id)
        {
            var result = _noteDAO.Get_UserNotes(id);

            return LoadModel(result);
        }













        public List<NoteModel> Get_ProjectNotes(int id)
        {
            var result = _noteDAO.Get_ProjectNotes(id);
            var modelList = new List<NoteModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new NoteModel
                {
                    Value = result.Rows[i]["Value"].ToString(),
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString())
                };

                if (!String.IsNullOrEmpty(result.Rows[i]["IntervalHour"].ToString()))
                    model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());

                modelList.Add(model);
            }
            return modelList;
        }

        public List<NoteModel> Get_StepNotes(int id1, int id2)
        {
            var result = _noteDAO.Get_StepNotes(id1, id2);
            var modelList = new List<NoteModel>();

            for (int i = 0; i < result.Rows.Count; i++)
            {
                var model = new NoteModel
                {
                    Value = result.Rows[i]["Value"].ToString(),
                    InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
                    FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString()),
                };
                if (!String.IsNullOrEmpty(result.Rows[i]["IntervalHour"].ToString()))
                    model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());
                modelList.Add(model);
            }
            return modelList;
        }

        public List<NoteModel> Search(NoteFilterModel model)
        {
            try
            {
                model.InitialDate = Adjust_Date(model.InitialDate);
                model.FinishDate = Adjust_Date(model.FinishDate);

                var result = _noteDAO.Search(model);

                return LoadModel(result);

                //List<NoteModel> notesList = LoadModel(result);
                //if (notesList.Count > 0)
                //{
                //    TimeSpan time = new TimeSpan();
                //    var horas = 0;
                //    var minutos = 0;
                //    foreach (var note in notesList)
                //    {
                //        if (note.InitHour != null && note.FinishHour != null)
                //        {
                //            TimeSpan intervalTime = new TimeSpan();

                //            if (note.IntervalHour.HasValue)
                //                intervalTime = TimeSpan.Parse(note.IntervalHour.Value.ToLongTimeString());
                //            else
                //                note.IntervalHour = new DateTime();


                //            TimeSpan hoursSUM = ((note.FinishHour.Value - note.InitHour.Value) - intervalTime);
                //            var aux = Convert.ToInt32(hoursSUM.TotalMinutes);
                //            note.TotalLine = hoursSUM.ToString("hh':'mm");
                //            minutos += aux;
                //            time += hoursSUM;
                //        }
                //    }

                //    var a = "00:00:00";
                //    var totalHours = string.Empty;
                //    foreach (var note in notesList)
                //        totalHours = CalculateTotalHours(a, note.TotalLine);

                //    var hours = TimeSpan.FromHours(Convert.ToDouble(totalHours));
                //    var total = (int)hours.TotalHours + hours.ToString(@"\:mm\:ss");

                //    horas = Convert.ToInt32(minutos / 60);
                //    minutos = minutos - (Convert.ToInt32(minutos / 60) * 60);

                //    var totHoras = "";
                //    var totMinutos = "";

                //    if (horas < 10)
                //        totHoras = "0" + horas.ToString();
                //    else
                //        totHoras = horas.ToString();

                //    if (minutos < 10)
                //        totMinutos = "0" + minutos.ToString();
                //    else
                //        totMinutos = minutos.ToString();


                //    foreach (var note in notesList)
                //        note.TotalHours = totHoras + ":" + totMinutos;
                //}
                
            }
            catch (Exception)
            {
                throw;
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
                this._projectBLL = new ProjectBLL();
                var Col = _userBLL.GetCollaborator(model.User.Id);

                var isValid = ValidateModel(model, Col);
                if (isValid != null)
                    return isValid;

                model.Value = CalculaValorApontamento(model, Col);
                if (model.Ticket == null)
                    model.Ticket = new TicketModel();

                if (_noteDAO.Insert(model) > 0)
                {
                    _projectBLL.Update_ProjectCost(model.Project.Id);
                    _projectBLL.Update_StepCost(model.Project.Id, model.Step.Id);

                    return MessageBLL.Generate("Apontamento registrado com sucesso!", 0);
                }

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
                this._projectBLL = new ProjectBLL();
                var Col = _userBLL.GetCollaborator(model.User.Id);

                var isValid = ValidateModel(model, Col);
                if (isValid != null)
                    return isValid;

                model.Value = CalculaValorApontamento(model, Col);
                if (model.Ticket == null)
                    model.Ticket = new TicketModel();


                if (_noteDAO.Update(model) > 0)
                {
                    _projectBLL.Update_ProjectCost(model.Project.Id);
                    _projectBLL.Update_StepCost(model.Project.Id, model.Step.Id);

                    return MessageBLL.Generate("Apontamento atualizado com sucesso", 0);
                }

                else
                    return MessageBLL.Generate("Ocorreu um erro ao salvar o apontamento, informe o administrador do sistema", 99, true);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public MessageModel Remove(int id)
        {
            try
            {
                if (_noteDAO.Remove(id) > 0)
                {
                    var result = _noteDAO.Get(id);
                    var projectId = Convert.ToInt32(result.Rows[0]["projectId"]);

                    Update_ProjectCost(projectId);
                    return MessageBLL.Generate("Apontamento removido", 0);
                }
                else
                    return MessageBLL.Generate("Erro ao excluir apontamento", 99, true);
            }
            catch (Exception exception)
            {
                throw exception.InnerException;
            }
        }
        //------------------------------------------------------------------------------------//



        public void Update_ProjectCost(int id)
        {
            this._projectBLL = new ProjectBLL();
            _projectBLL.Update_ProjectCost(id);

            var projectSteps = _projectStepBLL.Get_ProjectSteps(id);

            foreach (var step in projectSteps)
                _projectBLL.Update_StepCost(id, step.Id);
        }



        //public void CalculateTotalHours(NoteModel listNotes)
        //{
        //    try
        //    {
        //        TimeSpan time = new TimeSpan();
        //        foreach (var note in listNotes.UserNotes)
        //        {
        //            if (note.FinishHour != null && note.InitHour != null)
        //            {
        //                var conv = new TimeSpan(0, 0, 0);
        //                if (note.IntervalHour.HasValue)
        //                    conv = TimeSpan.Parse(note.IntervalHour.Value.ToLongTimeString());
        //                var hoursSUM = ((note.FinishHour.Value - note.InitHour.Value) - conv);
        //                note.TotalLine = hoursSUM.ToString("hh':'mm");
        //                time += hoursSUM;
        //            }
        //        }
        //        var hours = (time.TotalMinutes / 60).ToString().Split(',')[0];
        //        var minutos = time.TotalMinutes % 60;
        //        listNotes.IndicatedHours = String.Format("{0}:{1}", hours, minutos);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public string CalculateTotalHours(string hour, string sumHour)
        //{
        //    try
        //    {
        //        var h = hour;
        //        hour = (TimeSpan.Parse(h) + TimeSpan.Parse(sumHour)).ToString();
        //        return TimeSpan.Parse(hour).TotalHours.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public NoteModel AddValues(DataTable notes, int i)
        //{
        //    var noteModel = new NoteModel();
        //    noteModel.Description = notes.Rows[i]["Description"].ToString();
        //    noteModel.Id = Convert.ToInt32(notes.Rows[i]["Id"].ToString());
        //    noteModel.Step = new StepModel
        //    {
        //        Id = Convert.ToInt32(notes.Rows[i]["Fase"].ToString())
        //    };
        //    noteModel.Project = new ProjectModel()
        //    {
        //        Id = Convert.ToInt32(notes.Rows[i]["Project.Id"].ToString()),
        //        Nome = notes.Rows[i]["Project.Nome"].ToString(),
        //        Codigo = notes.Rows[i]["Project.Codigo"].ToString(),
        //        Tag = notes.Rows[i]["Project.Tag"].ToString()
        //    };
        //    noteModel.Requester = notes.Rows[i]["Requester"].ToString();
        //    noteModel.Date = Convert.ToDateTime(notes.Rows[i]["Date"].ToString());
        //    noteModel.Specialty = new SpecialtyModel() { Name = notes.Rows[i]["Specialty.Name"].ToString(), Id = Convert.ToInt32(notes.Rows[i]["Specialty.Id"]) };
        //    noteModel.Status = new StatusModel
        //    {
        //        Id = Convert.ToInt32(notes.Rows[i]["StatusId"].ToString()),
        //        Descricao = notes.Rows[i]["Status"].ToString()

        //    };

        //    if (!string.IsNullOrEmpty(notes.Rows[i]["Ticket.Id"].ToString()))
        //    {
        //        noteModel.Ticket = new TicketModel()
        //        {
        //            Id = Convert.ToInt32(notes.Rows[i]["Ticket.Id"]),
        //            Code = Convert.ToInt32(notes.Rows[i]["Ticket.NumTck"].ToString())
        //        };
        //    };
        //    return noteModel;
        //}

        public DateTime FindValidDay(DateTime date, List<Calendar1Model> feriados)
        {
            int count = 0;
            int validator = -1;


            bool notValid = true;

            // WHILE - Quantidade de dias a serem calculados
            while (count < (-2 * validator))
            {
                date = date.AddDays(1 * validator);

                // Enquanto for fim de semana ou feriado, vai indo pro próximo dia
                while (notValid)
                {
                    switch (date.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            date = validator > 0 ? date.AddDays(1) : date.AddDays(-2);
                            break;
                        case DayOfWeek.Saturday:
                            date = validator > 0 ? date.AddDays(2) : date.AddDays(-1);
                            break;
                    }
                    if (feriados != null)
                    {
                        Calendar1Model holidayModel = feriados.FirstOrDefault(h => h.Date == date);
                        if (holidayModel != null)
                        {
                            if (holidayModel.Date.DayOfWeek != DayOfWeek.Saturday && holidayModel.Date.DayOfWeek != DayOfWeek.Sunday)
                            {
                                date = date.AddDays(1 * validator);
                            }
                        }
                        else
                            notValid = false;

                    }
                    else
                        notValid = false;

                }
                notValid = true;
                count++;
            }
            return date;
        }

        //public List<NoteModel> LoadModel(DataTable result)
        //{
        //    var modelList = new List<NoteModel>();

        //    for (var i = 0; i < result.Rows.Count; i++)
        //    {
        //        var model = new NoteModel()
        //        {
        //            Id = Convert.ToInt32(result.Rows[i]["Id"]),
        //            Date = Convert.ToDateTime(result.Rows[i]["Date"]),

        //            User = new UserModel
        //            {
        //                Name = result.Rows[i]["Collaborator"].ToString()
        //            },

        //            Project = new ProjectModel
        //            {
        //                Codigo = result.Rows[i]["ProjectCode"].ToString(),
        //                Tag = result.Rows[i]["ProjectTag"].ToString(),
        //                Nome = result.Rows[i]["ProjectName"].ToString(),

        //                TipoProjeto = new ProjectTypeModel
        //                {
        //                    Nome = result.Rows[i]["ProjectType"].ToString()
        //                }
        //            },

        //            Cliente = result.Rows[i]["Client"].ToString(),
        //            InitHour = Convert.ToDateTime(result.Rows[i]["InitHour"].ToString()),
        //            FinishHour = Convert.ToDateTime(result.Rows[i]["FinishHour"].ToString()),
        //            Description = result.Rows[i]["Description"].ToString(),
        //            Requester = result.Rows[i]["Solicitante"].ToString(),

        //            Specialty = new SpecialtyModel
        //            {
        //                Name = result.Rows[i]["Specialty"].ToString()
        //            },

        //            Team = result.Rows[i]["Team"].ToString(),

        //        };

        //        if (!String.IsNullOrEmpty(result.Rows[i]["IntervalHour"].ToString()))
        //            model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());
        //        if (!String.IsNullOrEmpty(result.Rows[i]["TicketId"].ToString()))
        //            model.Ticket = new TicketModel { Id = Convert.ToInt32(result.Rows[i]["TicketId"]) };

        //        modelList.Add(model);
        //    }
        //    return modelList;
        //}

        public List<NoteModel> LoadSavedNotes(DataTable result)
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
                if (String.IsNullOrEmpty(result.Rows[i]["IntervalHour"].ToString()))
                    model.IntervalHour = null;
                else
                    model.IntervalHour = Convert.ToDateTime(result.Rows[i]["IntervalHour"].ToString());

                modelList.Add(model);
            }
            return modelList;
        }


        public List<NoteModel> FiltrarInterno(NoteFilterModel model)
        {
            if (model.UserId != 0)
                model.UserId = _collaboratorBLL.GetByUserID(model.UserId).Id;

            return Search(model);
        }


        private Double CalculaHorasApontamento(NoteModel model)
        {
            TimeSpan inicio, intervalo, fim;

            inicio = TimeSpan.Parse(model.InitHour.Value.ToLongTimeString());
            if (model.IntervalHour.HasValue)
                intervalo = TimeSpan.Parse(model.IntervalHour.Value.ToLongTimeString());
            else
                intervalo = new TimeSpan();
            fim = TimeSpan.Parse(model.FinishHour.Value.ToLongTimeString());

            return (fim.TotalHours - intervalo.TotalHours - inicio.TotalHours);
        }

        private string CalculaValorApontamento(NoteModel model, CollaboratorModel Col)
        {
            var totalHoras = CalculaHorasApontamento(model);

            var specialties = _specialtyBLL.Get_All();
            foreach (var item in specialties)
            {
                if (item.Id == model.Specialty.Id)
                {
                    model.Value = Convert.ToString(Math.Round((Convert.ToDouble(item.Value) * totalHoras), 2));
                    break;
                }
            }

            var collaboratorSpecialties = _collaboratorBLL.GetSpecialtiesForCollaborator(Col.Id);
            foreach (var item in collaboratorSpecialties)
            {
                if (item.Id == model.Specialty.Id)
                {
                    model.Value = Convert.ToString(Math.Round((Convert.ToDouble(item.Value) * totalHoras), 2));
                    break;
                }
            }

            var projectRules = _projectBLL.Get_SpecialtyRules(model.Project.Id);
            if (projectRules.Count > 0)
            {
                foreach (var item in projectRules)
                {
                    if (item.SpecialtyId == model.Specialty.Id && item.CollaboratorId == Col.Id)
                    {
                        var valor = float.Parse(item.Value);
                        var horas = totalHoras;
                        var custo = (valor * horas).ToString("N2");
                        model.Value = Convert.ToString(custo);
                        break;
                    }
                }
            }
            return model.Value;
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

            //Valida horas sobrepostas-----------------------------------------------------------------//
            var savedNotes = LoadSavedNotes(_noteDAO.Get_DayNotes(model.User.Id, model.Date.Date));    //
                                                                                                       //
            foreach (var item in savedNotes)                                                           //
            {                                                                                          //
                if (item.Id != model.Id)                                                               //
                {                                                                                      //
                    if (model.InitHour == item.InitHour)  //                                           //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                   //
                                                                                                       //
                    if ((model.InitHour < item.InitHour) && (model.FinishHour > item.InitHour))        //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                   //
                                                                                                       //
                    if ((model.InitHour > item.InitHour) && (model.InitHour < item.FinishHour))        //
                        return MessageBLL.Generate("Horário já apontado", 99, true);                   //
                }                                                                                      //
            }                                                                                          //
            //----------------------------------OK-----------------------------------------------------//

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
            var diaValido = FindValidDay(DateTime.Today, feriados);                                            //
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
            var apontamentosSalvos = LoadSavedNotes(_noteDAO.Get_DayNotes(model.User.Id, model.Date.Date.Date));  //
                                                                                                                  //
            var totalTrabalhado = CalculaHorasApontamento(model);                                                 //
            foreach (var item in apontamentosSalvos)                                                              //
            {                                                                                                     //
                if (item.Id != model.Id)                                                                          //
                    totalTrabalhado += CalculaHorasApontamento(item);                                             //
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
            }                                                                                                     //
            //-----------------------------------OK---------------------------------------------------------------//

            //Validações diversas-----------------------------------------------------------------------//
            this._projectBLL = new ProjectBLL();                                                        //
            if (_projectBLL.IsAMS(model.Project.Id) == "1" && model.Ticket == null)                     //
                return MessageBLL.Generate("Projeto do tipo AMS: informe o N° do chamado.", 99, true);  //
            //-------------------------------------OK---------------------------------------------------//

            return null;
        }

        private List<NoteModel> LoadModel(DataTable result)
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
                        IndicatedHours = "<verificar>",
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

                            Cliente = new ClientModel {
                                Id = Convert.ToInt32(result.Rows[i]["Client.Id"].ToString()),
                                Name = result.Rows[i]["Client.Name"].ToString()
                            },
                            TipoProjeto = new ProjectTypeModel {
                                Id = Convert.ToInt32(result.Rows[i]["ProjectType.Id"].ToString()),
                                Nome = result.Rows[i]["ProjectType.Name"].ToString() }
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
                    };

                    list = Calculate_NoteHours(list);

                    list.Add(model);
                }

                list = Calculate_NoteHours(list);

                return list;

            }
            catch(Exception ex)
            {
                throw ex.InnerException;
            }
        }
        
        private DateTime? Adjust_Date(DateTime? date)
        {
            if (!date.HasValue)
                return null;
            else
                return date;
        }

        private List<NoteModel> Calculate_NoteHours(List<NoteModel> list)
        {
            TimeSpan FinishHour, InitHour, Interval, TotalHours = new TimeSpan();                                                                                              //

            foreach (var item in list)
            {
                InitHour = TimeSpan.Parse(item.InitHour.Value.ToLongTimeString());                                                                        //
                FinishHour = TimeSpan.Parse(item.FinishHour.Value.ToLongTimeString());                                                                        //
                                                                                                                                                              //
                if (item.IntervalHour != null)                                                                                                               //
                    Interval = TimeSpan.Parse(item.IntervalHour.Value.ToLongTimeString());                                                                  //
                else
                    Interval = new TimeSpan();

                item.TotalLine = (FinishHour - Interval - InitHour).ToString();
                TotalHours += TimeSpan.Parse(item.TotalLine);
            }

            foreach (var item in list)
            {
                item.TotalHours = TotalHours.ToString();
            }

            return list;                                                                                                                                                                                                                                                                               //    if ((horaFinal - horaInicial).TotalHours >= 8 && (model.IntervalHour == null || intervalo.TotalHours < 1))    
        }
        #endregion
    }
}
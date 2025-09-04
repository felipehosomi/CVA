using System;
using System.Collections.Generic;
using DAO.Classes;
using MODEL.Classes;

namespace BLL.Classes
{
    public class CalendarBLL
    {
        #region Atributos
        private CalendarDAO CalendarDAO { get; set; }
        private StatusBLL StatusBLL { get; set; }
        #endregion

        #region Construtor
        public CalendarBLL()
        {
            this.CalendarDAO = new CalendarDAO();
        }
        #endregion

        public MessageModel Save(CalendarModel model)
        {
            try
            {
                var isValid = ValidateModel(model);
                if (isValid.Error == null)
                {
                    if (model.Id == 0)
                    {
                        var calendarId = CalendarDAO.Insert(model);
                        if (calendarId > 0)
                        {
                            model.Id = calendarId;
                            if (SaveHolidays(model).Error == null)
                            {
                                return MessageBLL.Generate("Calendário salvo com sucesso", calendarId);
                            }
                        }
                        return MessageBLL.Generate("Erro ao gerar calendário", 99, true);
                    }
                    else
                    {
                        int ok = CalendarDAO.Update(model);
                        if (ok > 0)
                        {
                            if (UpdateHolidays(model).Error == null)
                            {
                                return MessageBLL.Generate("Calendário salvo com sucesso", model.Id);
                            }
                        }
                        return MessageBLL.Generate("Erro ao gerar calendário", 99, true);
                    }
                }
                return isValid;
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        private MessageModel UpdateHolidays(CalendarModel calendar)
        {
            try
            {
                CalendarDAO.DeleteHolidays(calendar.Id);
                if (calendar.Holidays != null)
                {
                    foreach (var holiday in calendar.Holidays)
                    {
                        holiday.CalendarId = calendar.Id;
                        holiday.User = calendar.User;
                        holiday.Status = new StatusModel { Id = calendar.Status.Id };

                        if (CalendarDAO.SaveHolidays(holiday) == 0)
                        {
                            return MessageBLL.Generate("Falha ao inserir feriado", 99, true);
                        }
                    }
                }
                return MessageBLL.Generate("Não existem feriados para adicionar", 50);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        private MessageModel SaveHolidays(CalendarModel calendar)
        {
            try
            {
                if (calendar.Holidays != null)
                {
                    foreach (var holiday in calendar.Holidays)
                    {
                        holiday.CalendarId = calendar.Id;
                        holiday.User = calendar.User;
                        holiday.Status = new StatusModel { Id = calendar.Status.Id };

                        if (CalendarDAO.SaveHolidays(holiday) == 0)
                        {
                            return MessageBLL.Generate("Falha ao inserir feriado", 99, true);
                        }
                    }
                }
                return MessageBLL.Generate("Não existem feriados para adicionar", 50);
            }
            catch (Exception ex)
            {
                return MessageBLL.Generate(ex.Message, 99, true);
            }
        }

        public List<CalendarModel> Get()
        {
            try
            {
                return CalendarDAO.Get_All();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CalendarModel GetById(int calendarID)
        {
            try
            {
                CalendarModel calendarModel = CalendarDAO.Get(calendarID);
                calendarModel.Holidays = CalendarDAO.Get_Holidays(calendarID);
                return calendarModel;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public List<Calendar1Model> Get_All_Holidays(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                return CalendarDAO.Get_All_Holidays(dateFrom, dateTo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<StatusModel> GetSpecificStatus()
        {
            this.StatusBLL = new StatusBLL();
            return StatusBLL.Get(CalendarModel.oObjectType);
        }

        #region Private Methods
        private MessageModel ValidateModel(CalendarModel model)
        {
            try
            {
                if (model.Description == null)
                    model.Description = "";
                if (model.FinishDate < model.InitialDate)
                    return MessageBLL.Generate("A data final não pode ser inferior a data inicial", 99, true);
                if (model.InitialDate == DateTime.MinValue)
                    return MessageBLL.Generate("Obrigatório o preenchimento da data inicial", 99, true);
                if (model.Name == null)
                    return MessageBLL.Generate("Obrigatório o preenchimento do nome do calendário", 99, true);
                if (model.Status.Id == 0)
                    return MessageBLL.Generate("Obrigatório a seleção de um status válido", 99, true);

                return MessageBLL.Generate("Campos validado com sucesso", 0);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

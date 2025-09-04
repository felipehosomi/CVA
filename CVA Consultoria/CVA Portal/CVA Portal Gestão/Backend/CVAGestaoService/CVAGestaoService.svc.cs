using CVAGestaoService.Contracts;
using CVAGestaoService.Services;
using MODEL.Classes;

using System.Collections.Generic;
using System;

namespace CVAGestaoService
{
    public class CVAGestaoService : ILogin, ICalendar, IStatus, IUser, IProject, ISpecialty, ICollaborator, IGenre,
                                    IMaritalStatus, IUf, IClient, IExpense,
                                    IProjectStep, IOpportunity, INote, IPercentProject,
                                    IExpenseType, IPoliticExpense, IUnitMeter, IProfile, IUserView, IPeriod,
                                    IAMSTicket, IEmailSender, IProjectType, IPricing,
                                    IProjectParameters, IStatusReport, IChangeRequest, IAuthorization
    {
        #region Private Instances
        private LoginContract _loginContract { get; set; }
        private CalendarContract _calendarContract { get; set; }
        private StatusContract _statusContract { get; set; }
        private UserContract _userContract { get; set; }
        private ProjectContract _projectContract { get; set; }
        private ProjectTypeContract _projectTypeContract { get; set; }
        private SpecialtyContract _specialtyContract { get; set; }
        private CollaboratorContract _collaboratorContract { get; set; }
        private GenreContract _genreContract { get; set; }
        private MaritalStatusContract _maritalStatusContract { get; set; }
        private UfContract _ufContract { get; set; }
        private PricingContract _pricingContract { get; set; }
        private ClientContract _clientContract { get; set; }
        private ProjectStepContract _projectStepContract { get; set; }
        private OportunittyContract _oportunittyContract { get; set; }
        private NoteContract _noteContract { get; set; }
        private PercentProjectContract _percentProjectContract { get; set; }
        private ExpenseTypeContract _expenseTypeContract { get; set; }
        private PoliticExpenseContract _politicExpenseContract { get; set; }
        private UnitMeterContract _unitMeterContract { get; set; }
        private ExpenseContract _expenseContract { get; set; }
        private ChangeRequestContract _changeRequestContract { get; set; }
        private ProfileContract _profileContract { get; set; }
        private UserViewContract _userViewContract { get; set; }
        private PeriodContract _periodContract { get; set; }
        private AMSTicketContract _AMSTicketContract { get; set; }
        private EmailSenderContract _emailSenderContract { get; set; }
        private ProjectParametersContract _projectParametersContract { get; set; }
        private StatusReportContract _statusReportContract { get; set; }
        private AuthorizationContract _authorizationContract { get; set; }
        #endregion

        public CVAGestaoService()
        {
            this._loginContract = new LoginContract();
            this._calendarContract = new CalendarContract();
            this._statusContract = new StatusContract();
            this._userContract = new UserContract();
            this._projectContract = new ProjectContract();
            this._projectTypeContract = new ProjectTypeContract();
            this._specialtyContract = new SpecialtyContract();
            this._collaboratorContract = new CollaboratorContract();
            this._genreContract = new GenreContract();
            this._maritalStatusContract = new MaritalStatusContract();
            this._ufContract = new UfContract();
            this._pricingContract = new PricingContract();
            this._clientContract = new ClientContract();
            this._projectStepContract = new ProjectStepContract();
            this._oportunittyContract = new OportunittyContract();
            this._noteContract = new NoteContract();
            this._percentProjectContract = new PercentProjectContract();
            this._expenseTypeContract = new ExpenseTypeContract();
            this._politicExpenseContract = new PoliticExpenseContract();
            this._unitMeterContract = new UnitMeterContract();
            this._expenseContract = new ExpenseContract();
            this._changeRequestContract = new ChangeRequestContract();
            this._profileContract = new ProfileContract();
            this._userViewContract = new UserViewContract();
            this._periodContract = new PeriodContract();
            this._AMSTicketContract = new AMSTicketContract();
            this._emailSenderContract = new EmailSenderContract();
            this._projectParametersContract = new ProjectParametersContract();
            this._statusReportContract = new StatusReportContract();
            this._authorizationContract = new AuthorizationContract();
        }


        #region Login Methods
        public UserModel LogIn(string email, string password)
        {
            return _loginContract.LogIn(email, password);
        }
        public MessageModel LogOff(int userId)
        {
            return _loginContract.LogOff(userId);
        }
        public UserModel FirstAccess(UserModel user)
        {
            return _loginContract.FirstAccess(user);
        }
        public MessageModel SendEmail(string emailAddress)
        {
            return _emailSenderContract.SendEmail(emailAddress);
        }
        #endregion

        #region Calendar Methods
        public MessageModel SaveCalendarHeader(CalendarModel calendar)
        {
            return _calendarContract.SaveCalendarHeader(calendar);
        }
        public CalendarModel GetCalendar_ById(int calendarID)
        {
            return _calendarContract.Get_ByID(calendarID);
        }
        public List<CalendarModel> GetCalendar()
        {
            return _calendarContract.GetCalendar();
        }

        public List<StatusModel> CalendarStatus()
        {
            return _calendarContract.GetSpecificStatus();
        }
        #endregion

        #region Status Method
        public List<StatusModel> GetStatus(int objectId)
        {
            return _statusContract.GetStatus(objectId);
        }
        #endregion

        #region User Methods
        public UserModel GetUser(int id)
        {
            return _userContract.GetUser(id);
        }

        public MessageModel SaveUser(UserModel user)
        {
            return _userContract.Save(user);
        }

        public List<StatusModel> GetUserStatus()
        {
            return _userContract.GetSpecificStatus();
        }

        public List<UserModel> GetUsers()
        {
            return _userContract.GetUsers();
        }

        public MessageModel UpdateUser_ByUser(UserModel user)
        {
            return _userContract.Update_ByUser(user);
        }
        #endregion

        #region Change Request Methods
        public ChangeRequestModel ChangeRequest_Get(int id)
        {
            return _changeRequestContract.ChangeRequest_Get(id);
        }

        public List<ChangeRequestModel> ChangeRequest_Get_for_Project(int id)
        {
            return _changeRequestContract.ChangeRequest_Get_for_Project(id);
        }

        public MessageModel ChangeRequest_Save(ChangeRequestModel model)
        {
            return _changeRequestContract.ChangeRequest_Save(model);
        }
        #endregion


        #region Project Methods
        public string Project_Generate_Number(int id)
        {
            return _projectContract.Generate_Number(id);
        }

        public ProjectModel Project_Get_StatusReportParcial(int id)
        {
            return _projectContract.Get_StatusReportParcial(id);
        }

        public MessageModel Remove_Step(StepModel model)
        {
            return _projectContract.Remove_Step(model);
        }
        public List<StatusModel> ProjectStatus()
        {
            return _projectContract.GetSpecificStatus();
        }

        public List<StepModel> Project_GetSteps()
        {
            return _projectContract.GetSteps();
        }



        public List<ProjectModel> GetActiveProjects()
        {
            return _projectContract.GetActiveProjects();
        }

        public List<ProjectModel> LoadCombo_Project()
        {
            return _projectContract.LoadCombo_Project();
        }

        public List<CollaboratorModel> GetCollaboratorByFilters(string name, string cpf, string cnpj, int sector, int specialty, int status)
        {
            return _collaboratorContract.GetCollaboratorByFilters(name, cpf, cnpj, sector, specialty, status);
        }

        public List<ProjectModel> Project_Get_ByClient(int id)
        {
            return _projectContract.Get_ByClient(id);
        }
        public List<ProjectModel> Project_Get_ByCollaborator(int id)
        {
            return _projectContract.Get_ByCollaborator(id);
        }

        public List<ProjectModel> Project_Get_ByClientAndCollaborator(int idClient, int idCollaborator)
        {
            return _projectContract.Get_ByClientAndCollaborator(idClient, idCollaborator);
        }

        public List<ProjectModel> Filter_Projects(int clientId, string code)
        {
            return _projectContract.Filter_Projects(clientId, code);
        }
        public ProjectModel Get(int id)
        {
            return _projectContract.Get(id);
        }
        public List<ProjectModel> Get_ByUser(int id)
        {
            return _projectContract.Get_ByUser(id);
        }
        public List<ProjectModel> Project_Get_All()
        {
            return _projectContract.Get_All();
        }
        public MessageModel Project_Save(ProjectModel model)
        {
            return _projectContract.Project_Save(model);
        }

        public MessageModel RemoveSpecialtyRule(int idProject, int idSpecialty, int idCollaborator)
        {
            return _projectContract.RemoveSpecialtyRule(idProject, idSpecialty, idCollaborator);
        }
        #endregion

        #region Specialty Methods
        public MessageModel SaveSpecialty(SpecialtyModel specialty)
        {
            return _specialtyContract.Save(specialty);
        }
        public List<StatusModel> SpecialtyStatus()
        {
            return _specialtyContract.GetSpecificStatus();
        }
        public List<SpecialtyModel> GetSpecialtys()
        {
            return _specialtyContract.Get();
        }
        public List<SpecialtyModel> GetSpecialtyByColaborator(CollaboratorModel collaborator)
        {
            return _specialtyContract.GetByCollaborator(collaborator);
        }

        public List<SpecialtyTypeModel> Specialty_Get_TiposEspecialidade()
        {
            return _specialtyContract.Get_TiposEspecialidade();
        }

        public SpecialtyModel GetSpecialty_ByID(int specialtyID)
        {
            return _specialtyContract.GetId(specialtyID);
        }
        public List<SpecialtyModel> Specialty_Get_All()
        {
            return _specialtyContract.Get_All();
        }
        #endregion

        #region Collaborator Methods
        public CollaboratorModel Collaborator_Get(int id)
        {
            return _collaboratorContract.Get(id);
        }
        public CollaboratorModel ImportarDadosColaborador()
        {
            return _collaboratorContract.ImportarDadosColaborador();
        }



        public MessageModel Collaborator_Insert(CollaboratorModel model)
        {
            return _collaboratorContract.Insert(model);
        }

        public MessageModel Collaborator_Update(CollaboratorModel model)
        {
            return _collaboratorContract.Update(model);
        }

        public MessageModel Collaborator_Remove_Specialty(SpecialtyModel model, int idUser)
        {
            return _collaboratorContract.Remove_Specialty(model, idUser);
        }


        public List<StatusModel> CollaboratorStatus()
        {
            return _collaboratorContract.GetSpecificStatus();
        }

        public List<SpecialtyModel> Collaborator_Get_Specialties(int id)
        {
            return _collaboratorContract.Get_Specialties(id);
        }

        public List<SpecialtyModel> GetSpecialtiesForCollaborator(int id)
        {
            return _collaboratorContract.GetSpecialtiesForCollaborator(id);
        }

        public List<CollaboratorModel> GetCollaborator()
        {
            return _collaboratorContract.Get();
        }


        public List<CollaboratorModel> LoadCombo_Collaborator()
        {
            return _collaboratorContract.LoadCombo_Collaborator();
        }

        public List<CollaboratorModel> Collaborator_Get_NotUser()
        {
            return _collaboratorContract.Collaborator_Get_NotUser();
        }

        public List<CollaboratorModel> Get_CollaboratorBySpecialty(int id)
        {
            return _collaboratorContract.Get_CollaboratorBySpecialty(id);
        }



        public List<CollaboratorModel> GetPMs()
        {
            return _collaboratorContract.GetPMs();
        }

        public List<CollaboratorModel> Collaborator_Get_Active()
        {
            return _collaboratorContract.Get_Active();
        }


        public List<CollaboratorTypeModel> GetCollaboratorTypes()
        {
            return _collaboratorContract.GetActiveTypes();
        }

        public List<CollaboratorModel> CollaboratorFromSpecialty(SpecialtyModel specialty)
        {
            return _collaboratorContract.GetFromSpecialty(specialty);
        }

        public CollaboratorModel GetCollaboratorById(int collaboratorId)
        {
            return _collaboratorContract.GetByKey(collaboratorId);
        }

        public List<CollaboratorModel> GetCollaboratorNotUser()
        {
            return _collaboratorContract.Get_NotUser();
        }
        #endregion

        #region Genres Methods
        public List<GenreModel> GetGenre()
        {
            return _genreContract.Get();
        }
        #endregion

        #region Marital Status Methods
        public List<MaritalStatusModel> GetMaritalStatus()
        {
            return _maritalStatusContract.Get();
        }
        #endregion

        #region Uf Methods
        public List<UfModel> GetUf()
        {
            return _ufContract.Get();
        }
        #endregion



       

        #region Client Methods
        public MessageModel SaveClient(ClientModel client)
        {
            return _clientContract.Save(client);
        }
        public List<StatusModel> GetClientStatus()
        {
            return _clientContract.GetSpecificStatus();
        }
        //public List<ClientModel> GetClient()
        //{
        //    return _clientContract.Get();
        //}

        public List<ClientModel> Client_Search(string name)
        {
            return _clientContract.Search(name);
        }

        public List<ClientModel> LoadCombo_Client()
        {
            return _clientContract.LoadCombo_Client();
        }


        public List<ContactModel> GetClientContacts(int id)
        {
            return _clientContract.GetContacts(id);
        }
        public ClientModel GetClientBy_ID(int clientId)
        {
            return _clientContract.GetClientBy_ID(clientId);
        }
        #endregion


        #region Project Step Methods
        public MessageModel SaveProjectStep(ProjectStepModel projectStep)
        {
            return _projectStepContract.Save(projectStep);
        }

        public List<StepModel> Get_ProjectSteps(int id)
        {
            return _projectStepContract.Get_ProjectSteps(id);
        }

        public List<ProjectStepModel> GetProjectStep(int isProject)
        {
            return _projectStepContract.Get(isProject);
        }

        public List<StatusModel> GetProjectStepStatus()
        {
            return _projectStepContract.GetSpecificStatus();
        }

        public List<ProjectStepModel> GetAllProjectStep()
        {
            return _projectStepContract.GetAll();
        }

        public ProjectStepModel GetProjectStep_ByID(int id)
        {
            return _projectStepContract.Get_ByID(id);
        }
        #endregion

        #region Oportunitty Methods
        public string NewCodeGenerator()
        {
            return _oportunittyContract.NewCodeGenerator();
        }

        public List<StepModel> Oportunitty_GetSteps()
        {
            return _oportunittyContract.GetSteps();
        }

        public MessageModel SaveOportunitty(OpportunityModel oportunitty)
        {
            return _oportunittyContract.Save(oportunitty);
        }

        public MessageModel ConvertOportunittyToProject(OpportunityModel oportunitty)
        {
            return _oportunittyContract.ConvertToProject(oportunitty);
        }

        public List<OpportunityModel> GetOpportunities()
        {
            return _oportunittyContract.Get_All();
        }
        public OpportunityModel GetOportunittyById(int id)
        {
            return _oportunittyContract.Get(id);
        }

        public List<OpportunityModel> Search(string code, int clientId)
        {
            return _oportunittyContract.Search(code, clientId);
        }

        #endregion

        #region Note Methods
        public MessageModel Save(NoteModel note)
        {
            return _noteContract.Save(note);
        }


        public List<NoteModel> Get_UserNotes(int id)
        {
            return _noteContract.Get_UserNotes(id);
        }
        public MessageModel Note_Remove(int id)
        {
            return _noteContract.Note_Remove(id);
        }


        public List<NoteModel> Note_Search(NoteFilterModel model)
        {
            return _noteContract.Note_FiltrarInterno(model);
        }


        public List<AuthorizedHoursModel> GetAuthorizedHours()
        {
            return _noteContract.GetAuthorizedHours();
        }
        public List<AuthorizedHoursModel> GetAuthorizedHoursByCollaborator(int collaboratorId)
        {
            return _noteContract.GetAuthorizedHoursByCollaborator(collaboratorId);
        }
        #endregion

        #region Percent Project Methods
        public List<PercentProjectModel> GetPercentProject()
        {
            return _percentProjectContract.Get();
        }
        #endregion

        #region Expense Type Methods
        public MessageModel SaveExpenseType(ExpenseTypeModel expenseType)
        {
            return _expenseTypeContract.Save(expenseType);
        }
        public List<StatusModel> ExpenseTypeStatus()
        {
            return _expenseTypeContract.GetSpecificStatus();
        }
        public List<ExpenseTypeModel> GetExpenseTypes()
        {
            return _expenseTypeContract.Get();
        }
        public List<ExpenseTypeModel> GetAllExpenseTypes()
        {
            return _expenseTypeContract.GetAll();
        }
        public ExpenseTypeModel GetExpenseType(int id)
        {
            return _expenseTypeContract.Get(id);
        }
        #endregion

        #region Politic Expense Methods
        public List<PoliticExpenseModel> GetPoliticExpenseByProject(int projectId, int user)
        {
            return _politicExpenseContract.GetBy_Project(projectId, user);
        }
        #endregion

        #region Unit Meter Methods
        public List<UnitMeterModel> GetUnitMeter()
        {
            return _unitMeterContract.Get();
        }
        #endregion

        #region Expense Methods
        public MessageModel Expense_Save(ExpenseModel model)
        {
            return _expenseContract.Save(model);
        }

        public MessageModel Expense_Remove(int id)
        {
            return _expenseContract.Remove(id);
        }

        public List<ExpenseModel> GetExpense_ByUserID(int userId)
        {
            return _expenseContract.GetByUser(userId);
        }

        public ExpenseModel Expense_Get(int id)
        {
            return _expenseContract.Get(id);
        }

        public List<ExpenseModel> Expense_Search(int col, int cli, int prj, DateTime? de, DateTime? ate)
        {
            return _expenseContract.Search(col, cli, prj, de, ate);
        }
        #endregion

        #region Profile Methods
        public MessageModel SaveProfile(ProfileModel profile)
        {
            return _profileContract.Save(profile);
        }

        public List<StatusModel> ProfileGetStatus()
        {
            return _profileContract.GetSpecificStatus();
        }

        public List<ProfileModel> GetProfiles()
        {
            return _profileContract.Get();
        }

        public ProfileModel GetProfile_ByID(int profileID)
        {
            return _profileContract.Get_ByID(profileID);
        }
        #endregion

        #region User View Methods
        public List<UserViewModel> GetUserView()
        {
            return _userViewContract.Get();
        }
        #endregion

        #region Period Methods


        public MessageModel OpenPeriod()
        {
            return _periodContract.Open();
        }
        public MessageModel SaveSubPeriod(SubPeriodModel model)
        {
            return _periodContract.SaveSubPeriod(model);
        }

        public List<SubPeriodModel> GetSubPeriods(int? colId, int? clientId, int? projectId, DateTime? dateFrom, DateTime? dateTo)
        {
            return _periodContract.GetSubPeriods(colId, clientId, projectId, dateFrom, dateTo);
        }

        public MessageModel SetStatusSubPeriod(int periodId, int statusId)
        {
            return _periodContract.SetStatusSubPeriod(periodId, statusId);
        }

        public MessageModel SetStatusSubPeriodList(string periodIdList, int statusId)
        {
            return _periodContract.SetStatusSubPeriodList(periodIdList, statusId);
        }
        #endregion


        #region Authorization Methods
        public List<AuthorizedDayModel> Get_DiasAutorizados(int idCol)
        {
            return _authorizationContract.Get_DiasAutorizados(idCol);
        }

        public MessageModel AddDiaAutorizado(AuthorizedDayModel model)
        {
            return _authorizationContract.AddDiaAutorizado(model);
        }

        public MessageModel RemoveDiaAutorizado(int id)
        {
            return _authorizationContract.RemoveDiaAutorizado(id);
        }

        public List<AuthorizedHoursModel> Get_HorasAutorizadas(int idCol)
        {
            return _authorizationContract.Get_HorasAutorizadas(idCol);
        }

        public MessageModel AddHorasAutorizadas(AuthorizedHoursModel model)
        {
            return _authorizationContract.AddHorasAutorizadas(model);
        }

        public MessageModel RemoveHorasAutorizadas(int id)
        {
            return _authorizationContract.RemoveHorasAutorizadas(id);
        }

        public List<HoursLimitModel> Get_LimiteHoras(int idCol)
        {
            return _authorizationContract.Get_LimiteHoras(idCol);
        }

        public MessageModel AddLimiteHoras(HoursLimitModel model)
        {
            return _authorizationContract.AddLimiteHoras(model);
        }

        public MessageModel RemoveLimiteHoras(int id)
        {
            return _authorizationContract.RemoveLimiteHoras(id);
        }
        #endregion

        #region Ticket Methods
        public List<AMSTicketModel> GetTicketsByProject(int clientId)
        {
            return _AMSTicketContract.GetTicketsByProject(clientId);
        }
        #endregion

        #region ProjectType Methods
        public ProjectTypeModel ProjectType_Get(int id)
        {
            return _projectTypeContract.Get(id);
        }

        public List<ProjectTypeModel> ProjectType_Get_All()
        {
            return _projectTypeContract.Get_All();
        }

        public MessageModel ProjectType_Insert(ProjectTypeModel model)
        {
            return _projectTypeContract.Insert(model);
        }

        public MessageModel ProjectType_Update(ProjectTypeModel model)
        {
            return _projectTypeContract.Update(model);
        }

        public MessageModel ProjectType_Remove(int id)
        {
            return _projectTypeContract.Remove(id);
        }
        #endregion


        #region Pricing Methods
        public PricingModel Pricing_Get(int id)
        {
            return _pricingContract.Get(id);
        }

        public PricingModel Pricing_Get_Info(int id)
        {
            return _pricingContract.Get_Info(id);
        }

        public PricingModel Pricing_Get_By_Project(int id)
        {
            return _pricingContract.Get_By_Project(id);
        }

        public PricingModel Pricing_Get_By_Opportunitty(int id)
        {
            return _pricingContract.Get_By_Opportunitty(id);
        }

        public MessageModel Pricing_Insert(PricingModel model, int id)
        {
            return _pricingContract.Insert(model, id);
        }

        public MessageModel Pricing_Update(PricingModel model, int id)
        {
            return _pricingContract.Update(model, id);
        }

        public MessageModel Pricing_Opportunitty_Insert(PricingModel model, int id)
        {
            return _pricingContract.Opportunitty_Insert(model, id);
        }

        public MessageModel Pricing_Opportunitty_Update(PricingModel model, int id)
        {
            return _pricingContract.Opportunitty_Update(model, id);
        }
        #endregion

        #region ProjectParameters Methods
        public List<ProjectParametersModel> ProjectParameters_Get_All()
        {
            return _projectParametersContract.Get_All();
        }

        public MessageModel ProjectParameters_Save(ProjectParametersModel model)
        {
            return _projectParametersContract.Save(model);
        }
        #endregion

        #region StatusReport Methods
        public MessageModel StatusReport_Save(StatusReportModel model)
        {
            return _statusReportContract.Save(model);
        }

        public List<StatusReportModel> StatusReport_Get_All(int id)
        {
            return _statusReportContract.Get_All(id);
        }
        public string[] StatusReport_Get_ParcialHours(int id, DateTime data)
        {
            return _statusReportContract.Get_ParcialHours(id, data);
        }
        #endregion


    }
}
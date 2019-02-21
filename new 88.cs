using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using VidalinkWebApi.DAL;
using VidalinkWebApi.DTO;
using WebAppAdminPSP.Filters;
using WebAppAdminPSP.Models;
using WebAppAdminPSP.Models.ViewModel;


namespace WebAppAdminPSP.Controllers
{
    [Authorize]
    public class AgendaController : BaseController
    {

        [HttpPost]
        public ActionResult ExcluirAgendas(string CustomerId, int? IdPrestador, string Agendas)
        {
            try
            {
                PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;

                decimal UserIdCancelamento = prestadorLogado.UsuId;
                int IdMotivoCancelamento = 1;

                string[] arrAgd = Agendas.Split('|');
                //IdUsuPaciente
                AgendamentoDAL dal = new AgendamentoDAL();
                foreach (var item in arrAgd)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] arrUsu = item.Split('#');
                        int idAgd = Convert.ToInt32(arrUsu[0]);
                        decimal? idUsuPaciente = null;
                        if (!string.IsNullOrEmpty(arrUsu[1]))
                            idUsuPaciente = Convert.ToDecimal(arrUsu[1]);

                        #region "PBI - Product Backlog Item 55849:Melhorias consultar agenda - Cancelar agenda
                        //PBI 55849: Melhorias consultar agenda - Cancelar agenda
                        //Cancelar agendamento --> Agendamento: Mantém a escala e remove o agendamento.
                        if (!idUsuPaciente.HasValue)
                        {
                            //Escala: Não faz nada.
                            continue;
                        }
                        #endregion

                        int sisId = Convert.ToInt32(ConfigurationManager.AppSettings["IdSistemaPaciente"].ToString());
                        Session["SisIdPaciente"] = sisId;
                        if (idUsuPaciente != null)
                        {
                            HttpClient clientLogin;
                            clientLogin = new HttpClient();
                            clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Seguranca/");
                            clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("Autenticar?login={0}&senha={1}&sisid={2}", "CONVIDADO", "4C8056E2E8EE3969E053090A0A0AAE22", sisId.ToString()), null).Result;
                            Session["tokenApi"] = responseClientLogin.Content.ReadAsAsync<string>().Result;

                            string TipoCampo = GetValueCamposCadastro(Session["tokenApi"].ToString(), sisId, CustomerId, "RELATIONSHIP") == "1" ? "T" : "C";

                            //TODO: Desabilitados na sprint 13 pois não foram tratados
                            //bool _PermiteComunicacao = PermiteComunicacao(CustomerId, (int)agendaDTO.IdTipoProcedimento.Value);
                            //if (_PermiteComunicacao)
                            //{
                            //    string MensagemCancelamentoSobDemanda = ConfigurationManager.AppSettings["MensagemCancelamentoSobDemanda"];
                            //    string mensagem = string.Format(MensagemCancelamentoSobDemanda, agendaDTO.DtAgenda.ToString("dd/MM/yyyy"));
                            //    EnviarSms(mensagem, RetornaNumeroCelularUsuario(CustomerId, Convert.ToInt32(idUsuPaciente), TipoCampo, sisId), Convert.ToInt32(idUsuPaciente));

                            //    EnviarEmail("Cancelamento de Procedimento", idAgd, TemplateEmailEnum.CancelamentoPrestador, CustomerId, Convert.ToInt32(idUsuPaciente), sisId);
                            //}
                        }
                        AgendaDTO agendaDTO = dal.ObterAgenda(idAgd).FirstOrDefault();

                        dal.CancelarAgendamentoProcedimentoPaciente(CustomerId, idAgd, (int)idUsuPaciente.Value, GetUserLogin(), (int)UserIdCancelamento, IdMotivoCancelamento, "");

                        IntegrarAgenda(CustomerId, idAgd);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Erro = "Falha na exclusão das agendas", Mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Erro = "", Mensagem = "Agenda(s) excluída(s) com sucesso" }, JsonRequestBehavior.AllowGet);
        }
            try
            {
                PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;

                decimal UserIdCancelamento = prestadorLogado.UsuId;
                int IdMotivoCancelamento = 1;

                string[] arrAgd = Agendas.Split('|');
                //IdUsuPaciente
                AgendamentoDAL dal = new AgendamentoDAL();
                foreach (var item in arrAgd)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] arrUsu = item.Split('#');
                        int idAgd = Convert.ToInt32(arrUsu[0]);
                        decimal? idUsuPaciente = null;
                        if (!string.IsNullOrEmpty(arrUsu[1]))
                            idUsuPaciente = Convert.ToDecimal(arrUsu[1]);

                        AgendaDTO agendaDTO = dal.ObterAgenda(idAgd).FirstOrDefault();

                        int sisId = Convert.ToInt32(ConfigurationManager.AppSettings["IdSistemaPaciente"].ToString());
                        Session["SisIdPaciente"] = sisId;
                        if (idUsuPaciente != null)
                        {
                            HttpClient clientLogin;
                            clientLogin = new HttpClient();
                            clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Seguranca/");
                            clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                            System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("Autenticar?login={0}&senha={1}&sisid={2}", "CONVIDADO", "4C8056E2E8EE3969E053090A0A0AAE22", sisId.ToString()), null).Result;
                            Session["tokenApi"] = responseClientLogin.Content.ReadAsAsync<string>().Result;

                            string TipoCampo = GetValueCamposCadastro(Session["tokenApi"].ToString(), sisId, CustomerId, "RELATIONSHIP") == "1" ? "T" : "C";

                            //TODO: Desabilitados na sprint 13 pois não foram tratados
                            //bool _PermiteComunicacao = PermiteComunicacao(CustomerId, (int)agendaDTO.IdTipoProcedimento.Value);
                            //if (_PermiteComunicacao)
                            //{
                            //    string MensagemCancelamentoSobDemanda = ConfigurationManager.AppSettings["MensagemCancelamentoSobDemanda"];
                            //    string mensagem = string.Format(MensagemCancelamentoSobDemanda, agendaDTO.DtAgenda.ToString("dd/MM/yyyy"));
                            //    EnviarSms(mensagem, RetornaNumeroCelularUsuario(CustomerId, Convert.ToInt32(idUsuPaciente), TipoCampo, sisId), Convert.ToInt32(idUsuPaciente));

                            //    EnviarEmail("Cancelamento de Procedimento", idAgd, TemplateEmailEnum.CancelamentoPrestador, CustomerId, Convert.ToInt32(idUsuPaciente), sisId);
                            //}
                        }

                        dal.ExcluirAgenda(CustomerId, UserIdCancelamento, GetUserLogin(), IdMotivoCancelamento, idAgd, idUsuPaciente);

                        IntegrarAgenda(CustomerId, idAgd);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { Erro = "Falha na exclusão das agendas", Mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Erro = "", Mensagem = "Agenda(s) excluída(s) com sucesso" }, JsonRequestBehavior.AllowGet);
        }
        
        [CustomAuthorizationAttribute(TagName = "mnu_cons_agenda")]
        public ActionResult ConsultarAgendaPrestador()
        {
            PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;

            ProcedimentoModel model = new ProcedimentoModel();
            model.TipoProcedimentos = new List<TipoProcedimentoModel>();
            model.CustomerId = prestadorLogado.CustomerId;
            model.UsuId = (int)prestadorLogado.UsuId;
            model.Prestadores = new List<PrestadorModel>();

            CarregarPrestadoresModel(prestadorLogado, model);

            MotivoDAL motivoDAL = new MotivoDAL();
            ViewBag.MotivosAusencia = new SelectList((IEnumerable<MotivoDTO>)motivoDAL.ListarMotivos(3), "Id", "Descricao");

            FillBreadCumbInfo(model, "mnu_cons_agenda");

            return View(model);
        }

                public ActionResult ConsultarAgendaPrestadorFiltro(ProcedimentoModel model)
        {
            AgendamentoDAL dal = new AgendamentoDAL();
            List<ProcedimentoAgendaModel> procedimentos = new List<ProcedimentoAgendaModel>();
            model.DtInicio = Convert.ToDateTime(model.stDtInicio + " 00:00:00");
            model.DtFim = Convert.ToDateTime(model.stDtFim + " 23:59:59");

            string Nome = Request["Nome"];
            string Cpf = Request["Cpf"];
            string IgnorarData = Request["ChkIgnorarData"];
            int Agrupamento = int.Parse(Request["Agrupamento"]);



            AgendaDTOCollection lista = dal.ListarProcedimentosAgendas(model.CustomerId,
                model.IdTipoProcedimento,
                model.IdPrestador,
                model.IdStatus,
                model.DtInicio,
                model.DtFim,
                Nome,
                Cpf.Replace(".", "").Replace("-", ""),
                IgnorarData,
                Agrupamento);

            List<DateTime> allDates = new List<DateTime>();

            if (string.IsNullOrEmpty(IgnorarData))
            {
                for (DateTime date = model.DtInicio; date <= model.DtFim; date = date.AddDays(1))
                    allDates.Add(date);
            }
            else
            {
                var listaGroup = lista.GroupBy(x => x.DtAgendaSemHora);
                foreach (var item in listaGroup)
                {
                    allDates.Add(item.Key);
                }

            }

            int countDispo = 0;
            int countAgd = 0;
            int countAcomp = 0;
            foreach (var dt in allDates)
            {
                ProcedimentoAgendaModel pVazio = new ProcedimentoAgendaModel() { DataAgendaSemHora = dt, Locais = new List<LocalModel>(), exibir = true };

                pVazio.qtdDisponiveis = string.Format("({0}) Disponíveis", 0);
                pVazio.qtdAgendados = string.Format("({0}) Agendados", 0);
                pVazio.qtdAcompanhamentos = string.Format("({0}) Acompanhamentos", 0);

                if (lista.Exists(x => x.DtAgendaSemHora == dt))
                {
                    #region procedimentos

                    IEnumerable<string> tmpList = lista.FindAll(x => x.DtAgendaSemHora == dt).Select(x => x.TipoProcData).Distinct();

                    foreach (string tmp in tmpList)
                    {
                        string[] arr = tmp.Split('|');

                        ProcedimentoAgendaModel p = new ProcedimentoAgendaModel()
                        {
                            DsTipoProcedimento = arr[2].ToString(),
                            IdTipoProcedimento = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : 0,
                            DataAgendaSemHora = !string.IsNullOrEmpty(arr[0]) ? Convert.ToDateTime(arr[0]) : DateTime.MinValue,
                            Locais = new List<LocalModel>(),
                            exibir = false
                        };
                        List<AgendaDTO> agendasTmp = lista.FindAll(x => x.TipoProcData == tmp);

                        if (model.IdStatus != 2)
                        {
                            p.exibir = true;
                            countDispo = 0;
                            List<AgendaDTO> agendasDispoAux = agendasTmp.FindAll(x => x.IdStatus == 3 && x.FlAcompanhamento == "N");
                            List<AgendaDTO> agendasAgendamento = agendasTmp.FindAll(x => x.IdStatus == 1 && x.FlAcompanhamento == "N");
                            List<AgendaDTO> agendasDispo = new List<AgendaDTO>();

                            foreach (AgendaDTO agendamento in agendasAgendamento)
                            {
                                foreach (AgendaDTO dispon in agendasDispoAux)
                                {
                                    if (dispon.DtAgenda != agendamento.DtAgenda)
                                    {
                                        agendasDispo.Add(dispon);
                                    }
                                    else
                                    {
                                        agendasTmp.Remove(dispon);
                                    }
                                }
                            }


                            countAgd = agendasTmp.FindAll(x => x.IdStatus == 1 && x.FlAcompanhamento == "N").Count;


                            foreach (var ad in agendasDispo)
                            {
                                countDispo = countDispo + Convert.ToInt32(ad.QtdPrevAtendimentos);
                            }
                            countAgd = agendasTmp.FindAll(x => x.IdStatus == 1 && x.FlAcompanhamento == "N").Count;
                            countAcomp = agendasTmp.FindAll(x => x.FlAcompanhamento == "S").Count;

                            p.qtdDisponiveis = string.Format("({0}) Disponíveis", countDispo);
                            p.qtdAgendados = string.Format("({0}) Agendados", countAgd);
                            p.qtdAcompanhamentos = string.Format("({0}) Acompanhamentos", countAcomp);
                        }


                        for (int i = 0; i < agendasTmp.Count; i++)
                        {
                            LocalModel loc = new LocalModel()
                            {
                                CustomerId = agendasTmp[i].CustomerId,
                                DsLocalizacao = agendasTmp[i].DsLocalizacao,
                                IdLocalizacao = agendasTmp[i].IdLocalizacao
                            };
                            if (!p.Locais.Exists(x => x.IdLocalizacao == agendasTmp[i].IdLocalizacao))
                            {
                                loc.Agendas = new List<AgendaModel>();
                                var agd = new AgendaModel(agendasTmp[i]);
                                loc.Agendas.Add(agd);
                                p.Locais.Add(loc);
                            }
                            else
                            {
                                int? idx = p.Locais.FindIndex(x => x.IdLocalizacao == agendasTmp[i].IdLocalizacao);
                                var agd = new AgendaModel(agendasTmp[i]);
                                p.Locais[idx.Value].Agendas.Add(agd);
                            }
                        }
                        procedimentos.Add(p);
                    }

                    #endregion procedimentos
                }
                else
                {
                    procedimentos.Add(pVazio);
                }
            }


            var prestador = new PrestadorModel();
            if (Session["IdentificacaoCliente"] != null)
                prestador = (PrestadorModel)Session["IdentificacaoCliente"];

            ViewBag.DsCategoriaPrestador = prestador.DsCategoriaPrestador;

            //Caso o médico logado seja diferente do selecionado
            //Impedir que este remova a agenda do médico selecionado
            ViewBag.PermiteExcluirAgenda = "S";//ainda não aplicar (model.IdPrestador != prestador.IdPrestador && prestador.IdCategoriaPrestador == 1) ? "N" : "S";

            return PartialView(procedimentos);
        }

        public ActionResult ListaPrestador(string customerId, int? UsuId, int? IdCategoriaPrestador)
        {
            ProcedimentoDAL dal = new ProcedimentoDAL();
            PrestadorDTOCollection lista = dal.ListarPrestadores(customerId, UsuId, null, IdCategoriaPrestador, null, null);
            ViewBag.Prestadores = new SelectList((IEnumerable<PrestadorDTO>)lista, "UsuId", "DsNomePrestador");

            return PartialView();
        }

        [HttpGet]
        [CustomAuthorizationAttribute(TagName = "mnu_disp_agenda")]
        public ActionResult Index()
        {
            PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;
            ProcedimentoDAL prc = new ProcedimentoDAL();
            AgendaModel model = new AgendaModel();
            model.CustomerId = prestadorLogado.CustomerId;
            model.Prestadores = new List<PrestadorModel>();
            model.Especialidades = new List<EspecialidadeModel>();

            List<PrestadorDTO> lPrest = new List<PrestadorDTO>();


            if (prestadorLogado.DsCategoriaPrestador == "ADMINISTRADOR" ||
                prestadorLogado.DsCategoriaPrestador == "BACKOFFICE" ||
                prestadorLogado.DsCategoriaPrestador == "RECEPCAO" ||
                prestadorLogado.DsCategoriaPrestador == "CENTRO CIRURGICO")
            {
                //Exibir todos os prestadores, exceto os das categorias abaixo
                string[] categoriasExcecao = new string[] { "ADMINISTRADOR", "BACKOFFICE", "RECEPCAO", "CONTACT CENTER" };
                lPrest = prc.ListarPrestadores(model.CustomerId, null, null, null, null, null).Where(x => !categoriasExcecao.Contains(x.DsCategoriaPrestador)).OrderBy(x => x.DsNomePrestador).ToList(); ;
            }
            else
                lPrest = prc.ListarPrestadores(model.CustomerId, null, prestadorLogado.IdPrestador, null, null, null);

            if (lPrest != null)
            {
                foreach (var item in lPrest)
                    model.Prestadores.Add(new PrestadorModel(item));
            }

            model.IdPrestador = prestadorLogado.IdPrestador;
            model.QtdPrevAtendimentos = 1;

            model.TipoProcedimentos = ListarTipoProcedimentos(model.CustomerId, model.UsuId, null);

            FillBreadCumbInfo(model, "mnu_disp_agenda");

            return View(model);
        }

        private AgendamentoDAL.StatusAgenda InsereAgenda(AgendaModel model)
        {

            ProcedimentoDAL procDal = new ProcedimentoDAL();
            PrestadorDTOCollection lPrest = procDal.ListarPrestadores(model.CustomerId, null, model.IdPrestador, null, null, null);

            model.UsuId = (int)lPrest.FirstOrDefault().UsuId;

            int tempoPrevisto = model.Periodo;
            string strDt = model.DtAgenda.ToString("dd/MM/yyyy");
            string[] hhArr = model.RangeHorario.Split('|');
            string[] recs = model.Recursos.Split(';');

            AgendaDTOCollection agendas = new AgendaDTOCollection();

            foreach (string hhMi in hhArr)
            {
                if (!string.IsNullOrEmpty(hhMi))
                {
                    DateTime dtAgenda = Convert.ToDateTime(strDt + " " + hhMi + ":00");

                    AgendaDTO agenda = new AgendaDTO();
                    agenda.CustomerId = model.CustomerId;
                    agenda.DtAgenda = dtAgenda;
                    agenda.IdPrestador = model.IdPrestador;
                    agenda.TiposProcedimentos = model.TiposProcedimentos;
                    agenda.TempoAtendimento = tempoPrevisto;
                    agenda.IdLocalizacao = (model.IdLocalizacao == 0) ? null : model.IdLocalizacao;

                    agenda.QtdPrevAtendimentos = Convert.ToInt32(model.QtdPrevAtendimentos);

                    agenda.DsOrientacaoPaciente = model.DsOrientacaoesPaciente;

                    agenda.Recursos = new AgendaRecursoDTOCollection();
                    foreach (string rec in recs)
                    {
                        if (!string.IsNullOrEmpty(rec))
                        {
                            AgendaRecursoDTO recurso = new AgendaRecursoDTO() { CustomerId = model.CustomerId, IdRecurso = Convert.ToInt32(rec) };
                            agenda.Recursos.Add(recurso);
                        }
                    }

                    agendas.Add(agenda);
                }
            }

            AgendamentoDAL dal = new AgendamentoDAL();
            //dal.IncluirAgenda(agendas);

            AgendamentoDAL.StatusAgenda retornoAgenda = dal.IncluirAgenda(agendas, model.UsuId, GetUserLogin());

            return retornoAgenda;

        }

        [HttpPost]
        public JsonResult GravarAgenda(AgendaModel model)
        {
            try
            {


                // Integração de agendamento foi desabilitado conforme conferência realizada em 14/08/2017
                //try
                //{
                //    foreach (var agenda in agendas)
                //    {
                //        IntegrarAgenda(agenda.CustomerId, agenda.IdAgenda);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    var inner = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                //    new LogDAL().LogarErro(ex.Message, ex.StackTrace, inner, GetUser().ToString());
                //}

                var retornoAgenda = InsereAgenda(model);

                string msgRetorno = string.Empty;


                if (retornoAgenda.Sucesso)
                {
                    msgRetorno = "Agenda(s) incluída(s) com sucesso.";

                    //Verifica se houve alguma incosistência
                    if (retornoAgenda.MensagensErro.Count() > 0)
                    {
                        msgRetorno = "Agenda(s) incluída(s) com a(s) inconsistência(s) abaixo: </br>";

                        foreach (var m in retornoAgenda.MensagensErro)
                            msgRetorno += m + "</br>";
                    }
                }
                else
                {
                    foreach (var m in retornoAgenda.MensagensErro)
                        msgRetorno += m + "</br>";

                    throw new Exception(msgRetorno);
                }





                return Json(new { Erro = "", Mensagem = msgRetorno }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new { Erro = "Falha na gravação da(s) agenda(s). ", Mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        private ApiResultData<AgendaDTO> IntegrarAgenda(string CustomerId, decimal IdAgenda)
        {
            HttpClient clientLogin;
            clientLogin = new HttpClient();
            clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Integracao/");
            clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("DisponibilizarAgenda?CustomerId={0}&IdAgenda={1}", CustomerId, IdAgenda.ToString(System.Globalization.CultureInfo.InvariantCulture)), null).Result;
            ApiResultData<AgendaDTO> retorno = responseClientLogin.Content.ReadAsAsync<ApiResultData<AgendaDTO>>().Result;

            return retorno;
        }

        [HttpPost]
        public JsonResult ListarHorarios(string customerId, int IdPrestador, string idTipoProcedimento, int? periodo)
        {
            ProcedimentoDAL prc = new ProcedimentoDAL();
            PrestadorDTOCollection lPrest = prc.ListarPrestadores(customerId, null, IdPrestador, null, null, null);
            var periodoConfig = ConfigurationManager.AppSettings["IntervaloAgendamento"] != null ?
              int.Parse(ConfigurationManager.AppSettings["IntervaloAgendamento"]) : 30;
            int usuId = (int)lPrest.FirstOrDefault().UsuId;
            string iniJornada = lPrest.FirstOrDefault().IniJornada;
            string fimJornada = lPrest.FirstOrDefault().FimJornada;

            //tratamento de cadastro sem jornada
            iniJornada = (string.IsNullOrEmpty(iniJornada)) ? "08:00" : iniJornada;
            fimJornada = (string.IsNullOrEmpty(fimJornada)) ? "19:00" : fimJornada;

            var ids = idTipoProcedimento.Split(',');
            string orientacao = string.Empty;
            int QtdProcedimentos = ids.Length;
            TipoProcedimentoModel tipoPro = null;

            if (periodo > 0)
                periodoConfig = periodo.Value;

            if (ids.Length == 1 && !string.IsNullOrEmpty(ids[0]))
            {
                tipoPro = ListarTipoProcedimentos(customerId, usuId, null).Find(x => x.IdTipoProcedimento == int.Parse(ids[0]));
                orientacao = tipoPro != null ? tipoPro.DsOrientacaoProcedimento : string.Empty;
            }
            CultureInfo provider = CultureInfo.CurrentCulture;
            List<SelectListItem> lista = new List<SelectListItem>();

            DateTime dtAux;
            DateTime minDt = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy") + " " + iniJornada + ":00", provider);
            DateTime maxDt = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy") + " " + fimJornada + ":00", provider);

            dtAux = minDt;
            while (dtAux <= maxDt)
            {
                int tarde = dtAux.Hour;

                lista.Add(new SelectListItem() { Text = dtAux.ToString("HH:mm"), Value = (tarde > 13) ? "Tarde" : "Manhã" });
                if (tipoPro != null)
                {
                    dtAux = dtAux.AddMinutes(tipoPro.QtTempoPrevisto);
                    periodoConfig = tipoPro.QtTempoPrevisto;
                }
                else
                    dtAux = dtAux.AddMinutes(periodoConfig);
            }

            return Json(new { lista = lista, DsOrientacaoPaciente = orientacao, QtdProcedimentos = QtdProcedimentos, Periodo = periodoConfig }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RecuperarLocalizacao(string customerId, string TiposProcedimentos, string dtInicio, string dtFim, string prestadorId, int periodo)
        {
            try
            {
                ProcedimentoDAL tipoProcedimentoDAL = new ProcedimentoDAL();
                int minutes = periodo;

                DateTime _dtInicio = Convert.ToDateTime(dtInicio);
                DateTime _dtFim = Convert.ToDateTime(dtFim).AddMinutes(minutes);

                //Lista dos Tipos de Recurso
                List<TipoRecursoModel> tipos = new List<TipoRecursoModel>();
                var listaProcedimentos = TiposProcedimentos.Split(',');
                for (int i = 0; i < listaProcedimentos.Length; i++)
                {
                    tipos.AddRange(ListarTiposRecurso(customerId, int.Parse(listaProcedimentos[i]), prestadorId));
                }

                //Lista das localidades que possuem todos os recursos disponíveis para agendamento
                List<string> lstlocal = new List<string>();

                //Lista de todos os recursos disponíveis
                List<RecursosModel> recursosTmp = new List<RecursosModel>();

                #region Seleção de recursos
                foreach (var item in tipos.GroupBy(t => t.IdTipoRecurso).Select(t => t.FirstOrDefault()))
                {
                    //Recuperar os recursos disponíveis do tipo de recurso
                    List<RecursosModel> recs = ListarRecursosDisponiveis(customerId, item.IdTipoRecurso, _dtInicio, _dtFim, "A", "S");
                    foreach (RecursosModel r in recs)
                    {
                        r.QtdExigida = item.QtdExigida;
                        recursosTmp.Add(r);
                    }

                    //Lista com as localidades
                    IEnumerable<string> tmpList = recs.Select(x => x.DsLocalizacao).Distinct();
                    if (lstlocal.Count <= 0)
                    {
                        foreach (string s in tmpList)
                        {
                            lstlocal.Add(s);
                        }
                    }
                    else
                    {
                        //caso qtd exigida não esteja disponível o local selecionado não será utilizado
                        List<string> interSecList = lstlocal.Intersect(tmpList).ToList();
                        //Manter apenas os locais que possuem todos os recursos
                        lstlocal = interSecList;
                    }
                }
                #endregion Seleção de recursos

                //Lista de recursos disponíveis que podem ser utilizados no agendamento
                List<RecursosModel> recursos = new List<RecursosModel>();

                foreach (string item in lstlocal)
                {
                    recursos.AddRange(recursosTmp.FindAll(x => x.DsLocalizacao == item));
                }

                if (recursos == null || recursos.Count <= 0)
                {
                    //Caso não existam recursos disponíveis
                    throw new Exception("Não existem recursos disponíveis para o período selecionado");
                }

                #region Montar lista de retorno
                List<LocalRecursoModel> locaisTiposRecursos = new List<LocalRecursoModel>();
                foreach (RecursosModel rec in recursos)
                {
                    LocalRecursoModel l = new LocalRecursoModel() { DsLocalizacao = rec.DsLocalizacao, CustomerId = rec.CustomerId, IdLocalizacao = rec.IdLocalizacao, TipoRecursos = new List<TipoRecursoModel>() };
                    TipoRecursoModel t = new TipoRecursoModel() { CustomerId = rec.CustomerId, IdTipoRecurso = rec.IdTipoRecurso, DsTipoRecurso = rec.DsTipoRecurso, QtdExigida = rec.QtdExigida, Recursos = new List<RecursosModel>() };
                    t.Recursos.Add(rec);

                    if (!locaisTiposRecursos.Exists(x => x.DsLocalizacao == l.DsLocalizacao))
                    {
                        l.TipoRecursos.Add(t);
                        locaisTiposRecursos.Add(l);
                    }
                    else
                    {
                        int idxLocal = locaisTiposRecursos.FindIndex(x => x.DsLocalizacao == l.DsLocalizacao);
                        if (!locaisTiposRecursos[idxLocal].TipoRecursos.Exists(x => x.IdTipoRecurso == t.IdTipoRecurso))
                        {
                            locaisTiposRecursos[idxLocal].TipoRecursos.Add(t);
                        }
                        else
                        {
                            int idx = locaisTiposRecursos[idxLocal].TipoRecursos.FindIndex(x => x.IdTipoRecurso == t.IdTipoRecurso);
                            locaisTiposRecursos[idxLocal].TipoRecursos[idx].Recursos.Add(rec);
                        }
                    }
                }
                #endregion Montar lista de retorno

                return PartialView(locaisTiposRecursos);
            }
            catch (Exception e)
            {
                return Json(new { Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult RecuperarLocalizacaoSobDemanda(string customerId, int IdTipoProcedimento, string dtInicio, string dtFim, int tempoAtendimento)
        {
            try
            {
                //Lista dos Tipos de Recurso
                List<TipoRecursoModel> tipos = ListarTiposRecurso(customerId, IdTipoProcedimento, null);

                //Lista das localidades que possuem todos os recursos disponíveis para agendamento
                List<string> lstlocal = new List<string>();

                //Lista de todos os recursos disponíveis
                List<RecursosModel> recursosTmp = new List<RecursosModel>();

                #region Seleção de recursos
                foreach (var item in tipos)
                {
                    //Recuperar os recursos disponíveis do tipo de recurso
                    List<RecursosModel> recs = ListarRecursosDisponiveisSobDemanda(customerId, item.IdTipoRecurso);
                    foreach (RecursosModel r in recs)
                    {
                        r.QtdExigida = item.QtdExigida;
                        recursosTmp.Add(r);
                    }

                    //Lista com as localidades
                    IEnumerable<string> tmpList = recs.Select(x => x.DsLocalizacao).Distinct();
                    if (lstlocal.Count <= 0)
                    {
                        foreach (string s in tmpList)
                        {
                            lstlocal.Add(s);
                        }
                    }
                    else
                    {
                        //caso qtd exigida não esteja disponível o local selecionado não será utilizado
                        List<string> interSecList = lstlocal.Intersect(tmpList).ToList();
                        //Manter apenas os locais que possuem todos os recursos
                        lstlocal = interSecList;
                    }
                }
                #endregion Seleção de recursos

                //Lista de recursos disponíveis que podem ser utilizados no agendamento
                List<RecursosModel> recursos = new List<RecursosModel>();

                foreach (string item in lstlocal)
                {
                    recursos.AddRange(recursosTmp.FindAll(x => x.DsLocalizacao == item));
                }

                if (recursos == null || recursos.Count <= 0)
                {
                    //Caso não existam recursos disponíveis
                    throw new Exception("Não existem recursos disponíveis para o período selecionado");
                }

                #region Montar lista de retorno
                List<LocalRecursoModel> locaisTiposRecursos = new List<LocalRecursoModel>();
                foreach (RecursosModel rec in recursos)
                {
                    LocalRecursoModel l = new LocalRecursoModel() { DsLocalizacao = rec.DsLocalizacao, CustomerId = rec.CustomerId, IdLocalizacao = rec.IdLocalizacao, TipoRecursos = new List<TipoRecursoModel>() };
                    TipoRecursoModel t = new TipoRecursoModel() { CustomerId = rec.CustomerId, IdTipoRecurso = rec.IdTipoRecurso, DsTipoRecurso = rec.DsTipoRecurso, QtdExigida = rec.QtdExigida, Recursos = new List<RecursosModel>() };
                    t.Recursos.Add(rec);

                    if (!locaisTiposRecursos.Exists(x => x.DsLocalizacao == l.DsLocalizacao))
                    {
                        l.TipoRecursos.Add(t);
                        locaisTiposRecursos.Add(l);
                    }
                    else
                    {
                        int idxLocal = locaisTiposRecursos.FindIndex(x => x.DsLocalizacao == l.DsLocalizacao);
                        if (!locaisTiposRecursos[idxLocal].TipoRecursos.Exists(x => x.IdTipoRecurso == t.IdTipoRecurso))
                        {
                            locaisTiposRecursos[idxLocal].TipoRecursos.Add(t);
                        }
                        else
                        {
                            int idx = locaisTiposRecursos[idxLocal].TipoRecursos.FindIndex(x => x.IdTipoRecurso == t.IdTipoRecurso);
                            locaisTiposRecursos[idxLocal].TipoRecursos[idx].Recursos.Add(rec);
                        }
                    }
                }
                #endregion Montar lista de retorno

                return PartialView("RecuperarLocalizacao", locaisTiposRecursos);
            }
            catch (Exception e)
            {
                return Json(new { Erro = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ReplicarAgenda(AgendaModel model, string dtIni, string dtFim, string replicar)
        {
            DateTime _dtInicio = Convert.ToDateTime(dtIni);
            DateTime _dtFim = Convert.ToDateTime(dtFim);
            DateTime dtAux = _dtInicio;
            int diasEntreDatas = (int)((TimeSpan)(_dtFim - _dtInicio)).TotalDays;

            var jMsg = Json(new { Erro = "", Mensagem = "" }, JsonRequestBehavior.AllowGet);

            List<string> msgErro = new List<string>();
            bool sucessoRetorno = false;
            string msgRetorno = string.Empty;

            string[] diasSemana = replicar.Split('|');
            while (dtAux <= _dtFim)
            {
                if (diasSemana.Contains(MyDayOfWeek(dtAux.DayOfWeek)))
                {
                    model.DtAgenda = dtAux;

                    var retornoAgenda = InsereAgenda(model);

                    if (retornoAgenda.Sucesso)
                        sucessoRetorno = true;

                    foreach (var msg in retornoAgenda.MensagensErro)
                        msgErro.Add(msg);
                }

                dtAux = dtAux.AddDays(1);
            }



            if (sucessoRetorno)
                return Json(new { Erro = "", Mensagem = "Agenda replicada para o range: " + _dtInicio.ToString("dd/MM/yyyy") + " a " + _dtFim.ToString("dd/MM/yyyy"), ListaErros = msgErro }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { Erro = "Falha", Mensagem = "Agenda não replicada para o range: " + _dtInicio.ToString("dd/MM/yyyy") + " a " + _dtFim.ToString("dd/MM/yyyy"), ListaErros = msgErro }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CarregarTipoProcedimentos(string customerId, int? idPrestador, int? idEspecialidade)
        {
            int? usuId = null;
            ProcedimentoDAL dal = new ProcedimentoDAL();
            PrestadorDTOCollection prs = dal.ListarPrestadores(customerId, null, idPrestador, null, null, null);
            List<TipoProcedimentoModel> lista = new List<TipoProcedimentoModel>();

            if (prs != null && prs.Count > 0)
            {
                usuId = (int)prs.FirstOrDefault().UsuId;
                lista = ListarTipoProcedimentos(customerId, usuId, idEspecialidade).OrderBy(x => x.DsTipoProcedimento).ToList(); ;
            }
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CarregarEspecialidades(string customerId, int? idPrestador)
        {
            ProcedimentoDAL dal = new ProcedimentoDAL();
            EspecialidadeDTOCollection esps = dal.ListarEspecialidadesPrestador(customerId, null, idPrestador);
            List<EspecialidadeModel> lista = new List<EspecialidadeModel>();

            foreach (var item in esps)
            {
                lista.Add((EspecialidadeModel)item);
            }
            return Json(new { lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConfirmarAusencia(int motivoId, string observacao, string agendas)
        {
            AgendamentoDAL dal = new AgendamentoDAL();
            string[] arrAgd = agendas.Split('|');

            foreach (var item in arrAgd)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] arrUsu = item.Split('#');
                    int idAgd = Convert.ToInt32(arrUsu[0]);
                    decimal? idUsuPaciente = null;
                    if (!string.IsNullOrEmpty(arrUsu[1]))
                        idUsuPaciente = Convert.ToDecimal(arrUsu[1]);
                    if (idUsuPaciente != null)
                    {
                        dal.ConfirmaPresencaAusencia(GetCustomerId(), GetUserLogin(), (int)GetUser(), Convert.ToInt32(idUsuPaciente), idAgd, motivoId, observacao);
                        IntegrarAgenda(GetCustomerId(), idAgd);
                    }
                }

            }
            return Json(new { });
        }

        [HttpPost]
        public JsonResult ConfirmarPresenca(string agendas)
        {
            AgendamentoDAL dal = new AgendamentoDAL();
            string[] arrAgd = agendas.Split('|');

            foreach (var item in arrAgd)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string[] arrUsu = item.Split('#');
                    int idAgd = Convert.ToInt32(arrUsu[0]);
                    decimal? idUsuPaciente = null;
                    if (!string.IsNullOrEmpty(arrUsu[1]))
                        idUsuPaciente = Convert.ToDecimal(arrUsu[1]);
                    if (idUsuPaciente != null)
                    {
                        dal.ConfirmaPresencaAusencia(GetCustomerId(), GetUserLogin(), (int)GetUser(), Convert.ToInt32(idUsuPaciente), idAgd, null, null);
                        IntegrarAgenda(GetCustomerId(), idAgd);
                    }
                }

            }
            return Json(new { });
        }

        public ActionResult MensagemIntegracao()
        {
            return View();
        }

        #region metodos privados
        private List<TipoProcedimentoModel> ListarTipoProcedimentos(string customerId, int? usuId, int? idEspecialidade)
        {
            List<TipoProcedimentoModel> lista = new List<TipoProcedimentoModel>();
            ProcedimentoDAL dal = new ProcedimentoDAL();
            TipoProcedimentoDTOCollection col = dal.ListarTipoProcedimentoPrestador(customerId, usuId, null);

            foreach (TipoProcedimentoDTO item in col)
            {
                if (idEspecialidade == null)
                    lista.Add(new TipoProcedimentoModel() { CustomerId = customerId, IdTipoProcedimento = item.IdTipoProcedimento, DsTipoProcedimento = item.DsDescricaoProcedimento, QtTempoPrevisto = item.QtTempoPrevisto, DsOrientacaoProcedimento = item.DsOrientacaoProcedimento, FlHabilitaConfirmacao = item.FlHabilitaConfirmacao });
                else
                {
                    if (item.IdEspecialidade == idEspecialidade)
                        lista.Add(new TipoProcedimentoModel() { CustomerId = customerId, IdTipoProcedimento = item.IdTipoProcedimento, DsTipoProcedimento = item.DsDescricaoProcedimento, QtTempoPrevisto = item.QtTempoPrevisto, DsOrientacaoProcedimento = item.DsOrientacaoProcedimento, FlHabilitaConfirmacao = item.FlHabilitaConfirmacao });
                }
            }

            if (lista.Count > 1)
                lista = lista.GroupBy(l => l.IdTipoProcedimento).Select(l => l.FirstOrDefault()).ToList();

            return lista;
        }

        private List<TipoRecursoModel> ListarTiposRecurso(string customerId, int? IdTipoProcedimento, string prestadorId)
        {
            List<TipoRecursoModel> lista = new List<TipoRecursoModel>();

            ProcedimentoDAL dal = new ProcedimentoDAL();
            TipoRecursoDTOCollection tiposRecurso = dal.ListarTipoRecurso(customerId, IdTipoProcedimento, null, null, prestadorId);

            foreach (var item in tiposRecurso)
            {
                lista.Add(new TipoRecursoModel(item));
            }

            return lista;
        }

        private List<RecursosModel> ListarRecursosDisponiveis(string customerId, int IdTipoRecurso, DateTime dtInicio, DateTime dtFim, string flStatus, string flTodosRecursos)
        {
            List<RecursosModel> lista = new List<RecursosModel>();

            ProcedimentoDAL dal = new ProcedimentoDAL();
            RecursoDTOCollection recursos = dal.ListarRecursosDisponiveis(customerId, null, null, null, IdTipoRecurso, dtInicio.AddSeconds(1), dtFim, flStatus, flTodosRecursos);

            foreach (var item in recursos)
            {
                lista.Add(new RecursosModel(item));
            }
            return lista;
        }

        private List<RecursosModel> ListarRecursosDisponiveisSobDemanda(string customerId, int IdTipoRecurso)
        {
            List<RecursosModel> lista = new List<RecursosModel>();

            ProcedimentoDAL dal = new ProcedimentoDAL();
            RecursoDTOCollection recursos = dal.ListarRecursosDisponiveisSobDemanda(customerId, null, null, null, IdTipoRecurso);

            foreach (var item in recursos)
            {
                lista.Add(new RecursosModel(item));
            }
            return lista;
        }

        private string MyDayOfWeek(DayOfWeek dia)
        {
            switch (dia)
            {
                case DayOfWeek.Friday:
                    return "SEX";
                case DayOfWeek.Monday:
                    return "SEG";
                case DayOfWeek.Saturday:
                    return "SAB";
                case DayOfWeek.Sunday:
                    return "DOM";
                case DayOfWeek.Thursday:
                    return "QUI";
                case DayOfWeek.Tuesday:
                    return "TER";
                case DayOfWeek.Wednesday:
                    return "QUA";
                default:
                    return string.Empty;

            }
        }

        private DayOfWeek DiaDaSemana(string dia)
        {
            switch (dia)
            {
                case "SEG":
                    return DayOfWeek.Monday;
                case "TER":
                    return DayOfWeek.Tuesday;
                case "QUA":
                    return DayOfWeek.Wednesday;
                case "QUI":
                    return DayOfWeek.Thursday;
                case "SEX":
                    return DayOfWeek.Friday;
                case "SAB":
                    return DayOfWeek.Saturday;
                case "DOM":
                    return DayOfWeek.Sunday;
            }

            return DayOfWeek.Sunday;
        }
        #endregion metodos privados

        public ActionResult ConfirmarCancelamentoAgendamento(decimal idAgenda, int usuId, int sisId)
        {
            AgendamentoDAL agendamentoDAL = new AgendamentoDAL();
            MotivoDAL motivoDAL = new MotivoDAL();
            AgendaDTO agenda = agendamentoDAL.ObterAgenda(idAgenda).FirstOrDefault();
            agenda.IdUsuPaciente = usuId;
            agenda.SisId = sisId;
            ViewBag.MotivosAusencia = new SelectList((IEnumerable<MotivoDTO>)motivoDAL.ListarMotivos(1), "Id", "Descricao");
            return PartialView(agenda);
        }

        [HttpPost]
        public JsonResult ConfirmarCancelamentoAgendamentoPost(decimal idAgenda, int? idMotivoCancelamento, int usuId, int sisId, string ObservacaoCancelamento)
        {
            AgendaDTO agendaVM = new AgendaDTO();
            string customerId = GetCustomerId();
            PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;
            AgendamentoDAL agendamentoDAL = new AgendamentoDAL();

            try
            {
                AgendamentoDAL agdDal = new AgendamentoDAL();
                AgendaDTO agendaDTO = agendamentoDAL.ObterAgenda(idAgenda).FirstOrDefault();
                bool _PermiteComunicacao = PermiteComunicacao(customerId, (int)agendaDTO.IdTipoProcedimento.Value);

                agendamentoDAL.CancelarAgendamentoProcedimentoPaciente(customerId, idAgenda, Convert.ToInt32(prestadorLogado.UsuId), GetUserLogin(), (int)GetUser(), idMotivoCancelamento, ObservacaoCancelamento);
                IntegrarAgenda(customerId, idAgenda);

                try
                {
                    HttpClient clientLogin;
                    clientLogin = new HttpClient();
                    clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Seguranca/");
                    clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("Autenticar?login={0}&senha={1}&sisid={2}", "CONVIDADO", "4C8054008D8E3301E053090A0A0ACA5C", ConfigurationManager.AppSettings["IdSistema"].ToString()), null).Result;
                    Session["tokenApi"] = responseClientLogin.Content.ReadAsAsync<string>().Result;

                    if (_PermiteComunicacao)
                    {
                        //TODO: Desabilitados na sprint 13 pois não foram tratados
                        //EnviarEmail("Cancelamento de Procedimento", idAgenda, TemplateEmailEnum.CancelamentoPrestador, customerId, usuId, sisId);

                        //if (UsuarioAceitaSms(usuId, sisId))
                        //{
                        //EnviarSmsCancelamentoAgendamento(idAgenda);
                        //}

                        //if (UsuarioAceitaEmail(usuId, sisId))
                        //{

                        //}
                    }
                }
                catch (Exception exMail)
                {
                    //Falha no envio de e-mail de cancelamento de agenda
                    var log = new LogDAL();
                    log.IncluirLogOperacao(customerId, GetUserLogin(), (int)GetUser(), VidalinkWebApi.DTO.Enum.LogOperacao.CANC_AGENDAMENTO, "Falha no envio de e-mail de cancelamento de agenda - " + exMail.Message, VidalinkWebApi.DTO.Enum.TipoRegistro.AGENDA, idAgenda.ToString());
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ConfirmarAgendamento(decimal idAgenda, int usuId, int sisId, string customerCode, string customerId, int idProcedimento)
        {
            AgendamentoDAL agendamentoDAL = new AgendamentoDAL();
            MotivoDAL motivoDAL = new MotivoDAL();
            AgendaDTO agenda = agendamentoDAL.ObterAgenda(idAgenda).FirstOrDefault();
            agenda.IdUsuPaciente = usuId;
            agenda.SisId = sisId;
            agenda.IdProcedimento = idProcedimento;
            ViewBag.MotivosAusencia = new SelectList((IEnumerable<MotivoDTO>)motivoDAL.ListarMotivos(1), "Id", "Descricao");
            return PartialView(agenda);
        }

        [HttpPost]
        public JsonResult ConfirmarAgendamentoPost(decimal idAgenda, int usuId, int sisId, string customerCode, string customerId, int idProcedimento)
        {
            AgendaDTO agendaVM = new AgendaDTO();
            AgendamentoDAL agendamentoDAL = new AgendamentoDAL();
            try
            {
                agendamentoDAL.ConfirmarAgendamento(idProcedimento, idAgenda, GetCustomerId(), GetUserLogin(), (int)GetUser());
                HttpClient clientLogin;
                clientLogin = new HttpClient();
                clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Seguranca/");
                clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("Autenticar?login={0}&senha={1}&sisid={2}", "CONVIDADO", "4C8054008D8E3301E053090A0A0ACA5C", ConfigurationManager.AppSettings["IdSistema"].ToString()), null).Result;
                Session["tokenApi"] = responseClientLogin.Content.ReadAsAsync<string>().Result;
                EnviarEmail("Confirmação de Agendamento", idAgenda, TemplateEmailEnum.ConfirmacaoAgendamento, customerId, usuId, sisId);
                IntegrarAgenda(customerId, idAgenda);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AgendamentoSobDemanda()
        {
            var prestadorLogado = Utilitarios.GetUser();
            var prc = new ProcedimentoDAL();
            var model = new AgendaModel();
            var IdPrestadorPrincipal = Request["IdPrestadorPrincipal"] != null ? int.Parse(Request["IdPrestadorPrincipal"]) : 0;
            int? IdProcedimentoAnterior = null;
            model.CustomerId = prestadorLogado.CustomerId;
            model.Prestadores = new List<PrestadorModel>();
            model.PrestadoresSecundarios = new List<PrestadorModel>();
            model.Especialidades = new List<EspecialidadeModel>();
            model.SisId = Convert.ToInt32(Request["SisId"]);

            if (!string.IsNullOrEmpty(Request["IdProcedimentoAnterior"]))
                IdProcedimentoAnterior = int.Parse(Request["IdProcedimentoAnterior"]);

            List<PrestadorDTO> lPrest = new List<PrestadorDTO>();

            //Exibir todos os prestadores, exceto os das categorias abaixo
            string[] categoriasExcecao = new string[] { "ADMINISTRADOR", "BACKOFFICE", "RECEPCAO", "CONTACT CENTER" };
            lPrest = prc.ListarPrestadores(model.CustomerId, null, null, null, null, null).Where(x => !categoriasExcecao.Contains(x.DsCategoriaPrestador)).OrderBy(x => x.DsNomePrestador).ToList(); ;

            if (lPrest != null)
            {
                foreach (var item in lPrest)
                    model.Prestadores.Add(new PrestadorModel(item));
            }

            if (IdPrestadorPrincipal > 0)
                model.PrestadoresSecundarios = model.Prestadores.FindAll(x => x.IdPrestador != IdPrestadorPrincipal);

            //Id do Prestador logado
            model.IdPrestador = prestadorLogado.IdPrestador;
            model.IdProcedimentoAnterior = IdProcedimentoAnterior;
            model.QtdPrevAtendimentos = 1;

            model.TipoProcedimentos = prc.ListarTipoProcedimento(model.CustomerId, null, null, null, "A").OrderBy(x => x.DsNomeProcedimento).Select(t => (TipoProcedimentoModel)t).ToList();
            model.IdUsuPaciente = Convert.ToDecimal(Request["UsuId"]);

            FillBreadCumbInfo(model, "mnu_agendamendo_sob_demanda");

            return PartialView(model);
        }

        [HttpPost]
        public ActionResult GravarAgendaSobDemanda(AgendaModel model)
        {

            GravarAgendamentoSobDemandaVM gravarAgendamentoSobDemandaVM = new GravarAgendamentoSobDemandaVM();
            gravarAgendamentoSobDemandaVM.Sucesso = true;
            gravarAgendamentoSobDemandaVM.Mensagem = "Agenda gravada com sucesso";

            try
            {
                string[] recs = model.Recursos.Split(';');

                if (recs != null)
                    recs = recs.Where(r => !string.IsNullOrEmpty(r)).ToArray();

                DateTime dtAgenda = Convert.ToDateTime(model.DtAgenda);
                var tipoPro = new ProcedimentoDAL().ListarTipoProcedimento(model.CustomerId, null, int.Parse(model.IdTipoProcedimento)).FirstOrDefault();
                string prestadores = Request["IdPrestador"];

                if (string.IsNullOrEmpty(prestadores))
                {
                    prestadores = model.IdPrestadorPrincipal.ToString();
                }
                else
                {
                    prestadores = model.IdPrestadorPrincipal.ToString() + "," + prestadores;
                }

                AgendaSobDemandaDTO agenda = new AgendaSobDemandaDTO();
                agenda.CustomerId = model.CustomerId;
                agenda.Data = dtAgenda;
                agenda.Prestadores = prestadores;
                agenda.IdTipoProcedimento = int.Parse(model.IdTipoProcedimento);
                agenda.TempoProcedimento = tipoPro.QtTempoPrevisto;
                agenda.IdLocal = (int)model.IdLocalizacao;
                agenda.IdPaciente = Convert.ToInt32(model.IdUsuPaciente);
                agenda.IdUsuAcao = (int)GetUser();
                agenda.Orientacao = model.DsOrientacaoesPaciente;
                agenda.SisId = model.SisId;
                agenda.FlCancelarConflitos = model.FlCancelarConflitos == "S" ? "S" : "N";
                agenda.IdProcedimentoAnterior = model.IdProcedimentoAnterior;
                agenda.DsObservacaoAgendamento = model.DsObservacaoAgendamento;

                agenda.IdRecursos = string.Join(",", recs);

                string agendaString = JsonConvert.SerializeObject(agenda);
                var content = new StringContent(agendaString, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(UriServiceApi + "/api/Agendamento/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetToken());
                System.Net.Http.HttpResponseMessage response = client.PostAsync(string.Format("GravarAgendaSobDemanda?UserLogin={0}&UserId={1}", GetUserLogin(), GetUser()), content).Result;
                var retorno = response.Content.ReadAsAsync<ApiResultData<RetornoAgendamentoDemandaDTO>>().Result;

                if (!retorno.IsValid)
                {
                    throw new Exception(retorno.Message);
                }
            }
            catch (Exception e)
            {
                gravarAgendamentoSobDemandaVM.Sucesso = false;
                gravarAgendamentoSobDemandaVM.Mensagem = e.Message;
                new LogDAL().LogarErro(e.Message, e.StackTrace, e.InnerException != null ? e.InnerException.Message : string.Empty, GetUser().ToString());
                return PartialView(gravarAgendamentoSobDemandaVM);
            }

            return PartialView(gravarAgendamentoSobDemandaVM);
        }

        public void EnviarNotificacoesCancelamentoDemanda(RetornoAgendamentoDemandaDTO retornoAgendamentoDemandaDTO, string customerId, int IdUsuPaciente, int SisId, string Celular)
        {
            // TODO: REMOVER ESTE MÉTODO
            string[] AvisoCancelamentoPacientes = retornoAgendamentoDemandaDTO.AgendamentoCanceladosComPaciente.Split(',');

            for (int i = 0; i < AvisoCancelamentoPacientes.Length; i++)
            {
                if (!string.IsNullOrEmpty(AvisoCancelamentoPacientes[i]))
                {
                    AgendamentoDAL dal = new AgendamentoDAL();
                    AgendaDTO agendaDTO = dal.ObterAgenda(Convert.ToInt32(AvisoCancelamentoPacientes[i])).FirstOrDefault();

                    HttpClient clientLogin;
                    int sisIdAdmin = Convert.ToInt32(ConfigurationManager.AppSettings["IdSistema"].ToString());
                    clientLogin = new HttpClient();
                    clientLogin.BaseAddress = new Uri(UriServiceApi + "/api/Seguranca/");
                    clientLogin.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.Http.HttpResponseMessage responseClientLogin = clientLogin.PostAsync(string.Format("Autenticar?login={0}&senha={1}&sisid={2}", "CONVIDADO", "4C8054008D8E3301E053090A0A0ACA5C", sisIdAdmin.ToString()), null).Result;
                    Session["tokenApi"] = responseClientLogin.Content.ReadAsAsync<string>().Result;

                    string TipoCampo = GetValueCamposCadastro(Session["tokenApi"].ToString(), SisId, customerId, "RELATIONSHIP") == "1" ? "T" : "C";

                    if (agendaDTO.IdTipoProcedimento != null)
                    {
                        bool PermiteComunicacao = this.PermiteComunicacao(customerId, (int)agendaDTO.IdTipoProcedimento.Value);
                        if (PermiteComunicacao)
                        {
                            //TODO: Desabilitados na sprint 13 pois não foram tratados
                            //EnviarSms(string.Format("Procedimento {0} dia {1} foi cancelado", agendaDTO.DsNomeProcedimento, agendaDTO.DtAgenda), RetornaNumeroCelularUsuario(customerId, Convert.ToInt32(IdUsuPaciente), TipoCampo, SisId), Convert.ToInt32(IdUsuPaciente));
                            //EnviarEmail("Cancelamento de Procedimento", Convert.ToInt32(AvisoCancelamentoPacientes[i]), TemplateEmailEnum.CancelamentoPrestadorSobDemanda, customerId, Convert.ToInt32(IdUsuPaciente), SisId);
                        }
                    }
                }
            }
        }

        [HttpPost]
        public ActionResult ListarConflitos(DateTime DataAgenda, int TempoProcedimento, string ArrayPrestadores, string ArrayRecursos)
        {
            var dal = new AgendamentoDAL();

            return PartialView("_ListaConflitos", dal.ListarConflitosSobDemanda(GetCustomerId(), DataAgenda, TempoProcedimento, ArrayPrestadores, ArrayRecursos));
        }

        [HttpPost]
        public JsonResult CarregarPrestadoresAdicionais(int? IdPrestadorPrincipal, string CustomerId)
        {
            ProcedimentoDAL dal = new ProcedimentoDAL();
            List<PrestadorDTO> lista = new List<PrestadorDTO>();
            try
            {

                lista = dal.ListarPrestadores(CustomerId, null, null, null, null, null);
                lista = lista.FindAll(x => x.IdPrestador != IdPrestadorPrincipal);
                lista.Sort((s1, s2) => s1.DsNomePrestador.CompareTo(s2.DsNomePrestador));
            }
            catch (Exception ex)
            {
                return Json(new { Erro = true, Mensagem = "Falha ao carregar os prestadores" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Erro = false, Mensagem = "", Prestadores = lista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AtualizarObservacaoAgendamento(AgendaDTO Agenda)
        {
            AgendamentoDAL dal = new AgendamentoDAL();
            try
            {
                Agenda.CustomerId = GetCustomerId();
                int retorno = dal.AtualizarObservacaoAgendamento(Agenda);
                IntegrarAgenda(Agenda.CustomerId, Agenda.IdAgenda);
            }
            catch (Exception ex)
            {
                new LogDAL().LogarErro(ex.Message, ex.StackTrace, ex.InnerException != null ? ex.InnerException.Message : string.Empty, GetUser().ToString());
                return Json(new { Erro = true, Mensagem = "Falha ao atualizar observação." }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Erro = false, Mensagem = "" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RecuperarCalendarioAgenda(string CustomerId, DateTime dataAnalise, string arrayIdPrestadores, string arrayIdRecursos)
        {
            AgendamentoDAL dal = new AgendamentoDAL();
            DataTable dt = dal.RecuperarCalendarioAgenda(CustomerId, dataAnalise, arrayIdPrestadores, arrayIdRecursos);

            return Json(new { Erro = false, Mensagem = "", CalendarioAgenda = JsonConvert.SerializeObject(dt) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CancelarAgendas(string CustomerId, int? IdPrestador, string Agendas)
        {
            try
            {
                var prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;

                var userIdCancelamento = prestadorLogado.UsuId;
                var idMotivoCancelamento = 1;

                var arrAgd = Agendas.Split('|');

                var dal = new AgendamentoDAL();
                foreach (var item in arrAgd)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;

                    int idAgd = Convert.ToInt32(item);

                    dal.ExcluirAgenda(CustomerId, userIdCancelamento, GetUserLogin(), idMotivoCancelamento, idAgd, null);

                    IntegrarAgenda(CustomerId, idAgd);
                }
            }
            catch (Exception e)
            {
                return Json(new { Erro = "Falha no cancelamento das agendas", Mensagem = e.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Erro = "", Mensagem = "Agenda(s) cancelada(s) com sucesso" }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizationAttribute(TagName = "mnu_canc_agenda")]
        public ActionResult CancelarEscalas()
        {
            PrestadorModel prestadorLogado = Session["IdentificacaoCliente"] as PrestadorModel;

            ProcedimentoModel model = new ProcedimentoModel();
            model.TipoProcedimentos = new List<TipoProcedimentoModel>();
            model.CustomerId = prestadorLogado.CustomerId;
            model.UsuId = (int)prestadorLogado.UsuId;
            model.Prestadores = new List<PrestadorModel>();

            CarregarPrestadoresModel(prestadorLogado, model);

            FillBreadCumbInfo(model, "mnu_canc_agenda");

            return View(model);
        }

        private void CarregarPrestadoresModel(PrestadorModel prestadorLogado, ProcedimentoModel model)
        {
            ProcedimentoDAL prc = new ProcedimentoDAL();

            if (prestadorLogado != null)
            {
                if (prestadorLogado.IdCategoriaPrestador == 1 || //Médico
                    prestadorLogado.IdCategoriaPrestador == 2 || //ENFERMEIRO
                    prestadorLogado.IdCategoriaPrestador == 4 || //CONTACT CENTER
                    prestadorLogado.IdCategoriaPrestador == 5 || //BACKOFFICE
                    prestadorLogado.IdCategoriaPrestador == 6 || //CENTRO CIRURGICO
                    prestadorLogado.IdCategoriaPrestador == 7 || //RECEPCAO
                    prestadorLogado.IdCategoriaPrestador == 8    //ADMINISTRADOR
                    )
                {
                    List<PrestadorDTO> lPrest = prc.ListarPrestadores(model.CustomerId, null, null, null, null, null).OrderBy(x => x.DsNomePrestador).ToList();

                    foreach (var item in lPrest)
                    {
                        if (item.IdCategoriaPrestador != 5)
                            model.Prestadores.Add(new PrestadorModel(item));
                    }
                    if (prestadorLogado.IdCategoriaPrestador == 1) //Médico
                        model.IdPrestador = prestadorLogado.IdPrestador;
                }
                else
                {
                    List<PrestadorDTO> lPrest = prc.ListarPrestadores(model.CustomerId, null, prestadorLogado.IdPrestador, null, null, null).OrderBy(x => x.DsNomePrestador).ToList();
                    if (lPrest != null)
                        model.Prestadores.Add(new PrestadorModel(lPrest.FirstOrDefault()));

                }
            }

            if (model.Prestadores.Count == 1)
            {
                model.IdPrestador = model.Prestadores.FirstOrDefault().IdPrestador;
            }
        }

        public ActionResult CancelarEscalasFiltro(ProcedimentoModel model)
        {
            var dal = new AgendamentoDAL();
            var procedimentos = new List<ProcedimentoAgendaModel>();
            model.DtInicio = Convert.ToDateTime(model.stDtInicio + " 00:00:00");
            model.DtFim = Convert.ToDateTime(model.stDtFim + " 23:59:59");

            string Nome = Request["Nome"];
            string Cpf = Request["Cpf"];
            string IgnorarData = Request["ChkIgnorarData"];
            int Agrupamento = 0; //DATA

            AgendaDTOCollection lista = dal.ListarProcedimentosAgendas(model.CustomerId,
                model.IdTipoProcedimento,
                model.IdPrestador,
                model.IdStatus,
                model.DtInicio,
                model.DtFim,
                string.Empty,
                string.Empty,
                IgnorarData,
                Agrupamento);

            List<DateTime> allDates = new List<DateTime>();

            if (string.IsNullOrEmpty(IgnorarData))
            {
                for (DateTime date = model.DtInicio; date <= model.DtFim; date = date.AddDays(1))
                    allDates.Add(date);
            }
            else
            {
                var listaGroup = lista.GroupBy(x => x.DtAgendaSemHora);
                foreach (var item in listaGroup)
                {
                    allDates.Add(item.Key);
                }

            }
            
            foreach (var dt in allDates)
            {
                var pVazio = new ProcedimentoAgendaModel() { DataAgendaSemHora = dt, Locais = new List<LocalModel>(), exibir = true };

                pVazio.qtdDisponiveis = string.Format("({0}) Disponíveis", 0);
                pVazio.qtdAgendados = string.Format("({0}) Agendados", 0);
                pVazio.qtdAcompanhamentos = string.Format("({0}) Acompanhamentos", 0);

                if (lista.Exists(x => x.DtAgendaSemHora == dt))
                {
                    #region procedimentos
                    IEnumerable<string> tmpList = lista.FindAll(x => x.DtAgendaSemHora == dt).Select(x => x.TipoProcData).Distinct();
                    foreach (string tmp in tmpList)
                    {
                        string[] arr = tmp.Split('|');

                        var p = new ProcedimentoAgendaModel()
                        {
                            DsTipoProcedimento = arr[2].ToString(),
                            IdTipoProcedimento = !string.IsNullOrEmpty(arr[1]) ? Convert.ToInt32(arr[1]) : 0,
                            DataAgendaSemHora = !string.IsNullOrEmpty(arr[0]) ? Convert.ToDateTime(arr[0]) : DateTime.MinValue,
                            Locais = new List<LocalModel>(),
                            exibir = false
                        };
                        var agendasTmp = lista.FindAll(x => x.TipoProcData == tmp);

                        if (model.IdStatus != 2)
                        {
                            p.exibir = true;
                            List<AgendaDTO> agendasDispo = agendasTmp.FindAll(x => x.IdStatus == 3 && x.FlAcompanhamento == "N");
                            foreach (var ad in agendasDispo)
                            {
                                p.disponiveis += Convert.ToInt32(ad.QtdPrevAtendimentos);
                                p.agendasDisponiveis += ad.IdAgenda + "|";
                            }
                            p.agendados = agendasTmp.FindAll(x => (x.IdStatus == 1 || x.IdStatus == 6) && x.FlAcompanhamento == "N").Count;
                            p.acompanhamentos = agendasTmp.FindAll(x => x.FlAcompanhamento == "S").Count;
                        }


                        for (int i = 0; i < agendasTmp.Count; i++)
                        {
                            LocalModel loc = new LocalModel()
                            {
                                CustomerId = agendasTmp[i].CustomerId,
                                DsLocalizacao = agendasTmp[i].DsLocalizacao,
                                IdLocalizacao = agendasTmp[i].IdLocalizacao
                            };
                            if (!p.Locais.Exists(x => x.IdLocalizacao == agendasTmp[i].IdLocalizacao))
                            {
                                loc.Agendas = new List<AgendaModel>();
                                var agd = new AgendaModel(agendasTmp[i]);
                                loc.Agendas.Add(agd);
                                p.Locais.Add(loc);
                            }
                            else
                            {
                                int? idx = p.Locais.FindIndex(x => x.IdLocalizacao == agendasTmp[i].IdLocalizacao);
                                var agd = new AgendaModel(agendasTmp[i]);
                                p.Locais[idx.Value].Agendas.Add(agd);
                            }
                        }
                        procedimentos.Add(p);
                    }

                    #endregion procedimentos
                }
                else
                {
                    procedimentos.Add(pVazio);
                }
            }

            procedimentos = procedimentos.Where(x => x.disponiveis != 0).ToList();

            var prestador = new PrestadorModel();
            if (Session["IdentificacaoCliente"] != null)
                prestador = (PrestadorModel)Session["IdentificacaoCliente"];

            ViewBag.DsCategoriaPrestador = prestador.DsCategoriaPrestador;

            //Caso o médico logado seja diferente do selecionado
            //Impedir que este remova a agenda do médico selecionado
            ViewBag.PermiteExcluirAgenda = "S";//ainda não aplicar (model.IdPrestador != prestador.IdPrestador && prestador.IdCategoriaPrestador == 1) ? "N" : "S";

            return PartialView(procedimentos);
        }

    }
}
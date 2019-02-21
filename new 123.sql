-- ============================ 
-- Query todas as propostas independente de subvenção e de estado
DECLARE @dt_inicio	SMALLDATETIME = '20170101',
	@dt_fim		SMALLDATETIME = '20181231'



SELECT 		
		ca.cd_status,
		'Nome da Corretora'							= cp_corr.nm_pessoa,
		'Codigo da Seguradora'							= '0478-2',
		'Data da Proposta'								= ISNULL(CONVERT(VARCHAR(30),ce.dt_inclusao,103),CONVERT(VARCHAR(30),ce.dt_emissao,103)),
		'Data Inicio Vigencia'							= CONVERT(VARCHAR(30),ce.dt_inicio_vigencia,103),			
		'Status'										= cst.nm_status,
		'Numero da Proposta'							= ce.cd_proposta,
		'# Endosso' = ce.nr_endosso,
		'CPF ou CNPJ do Segurado'						=  dbo.corpfc_formata_cnpjcfp(cp_cli.nr_cnpj_cpf,cp_cli.cd_tipo_pessoa),
		'Nome do Segurado'								= UPPER(dbo.corpfc_remove_acentos([edn].[f_remove_caracteres_especiais](upper(ltrim(rtrim(cp_cli.nm_pessoa)))))),
		'Caracterização da Cobertura'					= ccp.nm_descricao,
		'Nível de Cobertura de Produtividade'			= ( SELECT CAST((COALESCE(cfgi.pe_nivel_cobertura,crnc.nr_nivel_cobertura, 0)) AS INT)
															FROM dbo.corp_fruta_grao_itens						cfgi2	WITH (NOLOCK) 
																INNER JOIN dbo.corp_endosso						ce2		WITH (NOLOCK) ON ce2.id_endosso = cfgi2.id_endosso		
																LEFT  JOIN dbo.corp_fruta_grao_variedade		cfgv2	WITH (NOLOCK) ON (cfgi2.id_fruta_grao_variedade = cfgv2.id_fruta_grao_variedade)
																LEFT JOIN  dbo.corp_fruta_grao					cfg2	WITH (NOLOCK) ON cfg2.id_fruta_grao = cfgv2.id_fruta_grao 
																LEFT  JOIN dbo.corp_fruta_grao_cultura_susep	cfgcs2	WITH (NOLOCK) ON cfg2.id_fruta_grao_cultura_susep = cfgcs2.id_fruta_grao_cultura_susep
																LEFT JOIN  dbo.corp_rural_nivel_cobertura		crnc	WITH (NOLOCK) ON  crnc.id_rural_nivel_cobertura = cfgi2.id_rural_nivel_cobertura
															WHERE cfgi2.id_endosso = ce.id_endosso), --Identificar

		'Tipo de Cultivo'									= cfg.nm_fruta_grao,

		'Percentual de Subvenção'						= cast(isnull((	SELECT TOP 1 cerc.pe_subvencao	
																	FROM dbo.corp_endosso_rural_contrato cerc
																		INNER JOIN dbo.corp_rural_contrato_subvencao crcs WITH (NOLOCK) ON crcs.id_contrato_subvencao = cerc.id_contrato_subvencao
																		INNER JOIN dbo.corp_fruta_grao_itens		 cfgi WITH (NOLOCK) ON cfgi.id_endosso = cerc.id_endosso),0) as numeric),
		'Subvenção Federal R$'							= REPLACE(CONVERT(VARCHAR,ISNULL((  SELECT SUM((CASE	WHEN  ce3.cd_tipo_endosso IN (0,1,5) THEN (1 * cep.qt_total) 
																WHEN  ce3.cd_tipo_endosso IN (2,3)	THEN ((-1) * cep.qt_total) 
																ELSE  0  END))
															FROM dbo.corp_endosso ce3  WITH (NOLOCK)
																INNER JOIN dbo.corp_endosso_parcela cep WITH (NOLOCK) ON cep.id_endosso = ce3.id_endosso
															WHERE ce3.cd_proposta = ce.cd_proposta 
																	AND cep.id_tp_subvencao = 1 ),0)),'.',','),
		'Subvenção Estadual R$'							= REPLACE(CONVERT(VARCHAR,ISNULL(( SELECT SUM((CASE	WHEN  ce3.cd_tipo_endosso IN (0,1,5) THEN (1 * cep.qt_total) 
																WHEN  ce3.cd_tipo_endosso IN (2,3)	THEN ((-1) * cep.qt_total) 
																ELSE  0  END))
															FROM dbo.corp_endosso ce3  WITH (NOLOCK)
																INNER JOIN dbo.corp_endosso_parcela cep WITH (NOLOCK) ON cep.id_endosso = ce3.id_endosso
															WHERE ce3.cd_proposta = ce.cd_proposta 
																	AND cep.id_tp_subvencao = 2 ),0)),'.',','),
		'Premio Total  R$'								= REPLACE(CONVERT(VARCHAR,ce.vl_tarifario),'.',','),
		'Custo de Emissao de Apolice'					= '0,00', 		
		'Codigo da Atividade (Banco Central)'			= (CASE len(convert(varchar,ISNULL(cfgi.cd_bacen,0)))  
																WHEN 14 then convert(varchar,ISNULL(cfgi.cd_bacen,0))
															ELSE 
																CASE len(convert(varchar,ISNULL(cfg.id_atividade_bacen,0))) 
																	WHEN 14 then convert(varchar,ISNULL(cfg.id_atividade_bacen,0))
																	ELSE convert(varchar,ISNULL(cfgi.cd_bacen,0))
																END 
															END),
		'Codigo do Municipio (Banco Central)'			= ISNULL(crs.cd_bacen,''), 
		'Utilizacao do Zoneamento Agricola do MAPA'		= 'SIM', 
		'Cultura organica'								= 'NÃO', 
		'Beneficiario do PRONAMP'						= 'NÃO',
		'Razao Social da Seguradora'					= CONVERT(VARCHAR(60),dbo.corpfc_remove_acentos([edn].[f_remove_caracteres_especiais](UPPER(LTRIM(RTRIM(c_emp.nm_empresa)))))),
		'CNPJ da Seguradora'							= dbo.corpfc_formata_cnpjcfp(c_emp.nr_cnpj,2),		
		'Data da Apólice'								= (CASE WHEN ISNULL(CONVERT(VARCHAR(1), ca.dv_documento_oficial),'0') = '1' THEN CONVERT(VARCHAR,ca.dt_emissao,103) ELSE '' END) , --Deixar em branco quando se tratar apenas de proposta
		'Numero da Apólice'								= (CASE WHEN ISNULL(CONVERT(VARCHAR(1), ca.dv_documento_oficial),'0') = '1' THEN CONVERT(VARCHAR,ca.cd_apolice) ELSE '' END), --Deixar em branco quando se tratar apenas de proposta
		'Nome da Propriedade Rural'						= CONVERT(VARCHAR(60),dbo.corpfc_remove_acentos([edn].[f_remove_caracteres_especiais](UPPER(LTRIM(RTRIM(cpro.nm_propriedade)))))),
		'Logradouro da Propriedade Rural'				= ISNULL(cpro.nm_endereco,cend.nm_endereco),
		'Numero da Propriedade Rural'					= ISNULL(cend.nr_rua_endereco, 'N\A'), --número
		'Complemento do Endereco da Propriedade Rural'  = ISNULL(cpro.nm_complemento,cend.nm_complemento),

		'Municipio da Propriedade Rural'				= COALESCE(LTRIM(RTRIM(SUBSTRING(cl.nm_local, 0, (CHARINDEX('-',cl.nm_local))))),cpro.cd_municipio,cend.nm_cidade,'N\A'),		
		'Estado da Propriedade Rural'					= COALESCE(cuf_item.nm_uf, cuf_cend.nm_uf,cuf_cpro.nm_uf),

		'CEP da Propriedade Rural'						= ISNULL(cpro.nm_cep,cend.nm_cep),
		'Logradouro do Segurado'						= dbo.corpfc_remove_acentos([edn].[f_remove_caracteres_especiais](upper(ltrim(rtrim(cend_cli.nm_endereco))))),
		'Numero do Endereco do Segurado'				= ISNULL(cend_cli.nr_rua_endereco,'N\A'),
		'Complemento do Endereco do Segurado'			= ISNULL(cend_cli.nm_complemento,'N\A'),
		'CEP do Endereco do Segurado'					= ISNULL(cend_cli.nm_cep,'N\A'),
		'Municipio do Endereco do Segurado'				= CASE WHEN (ISNUMERIC(cend_cli.nm_cidade) = 1) THEN ISNULL(clocal_cli.nm_local,'N\A') ELSE ISNULL(cend_cli.nm_cidade,'N\A') END,
		'Estado do Endereco do Segurado'				= CASE WHEN (ISNUMERIC(cend_cli.nm_cidade) = 1) THEN ISNULL(cuf_cli2.nm_uf, 'N\A') ELSE ISNULL(cuf_cli.nm_uf, 'N\A') END ,
		'Produtividade Estimada'						= REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR,CAST((ISNULL(cfgi.qt_produto_kilo_alqueire_estimado,0) * 60) AS NUMERIC(12,2))))),'.',','),
		'Area Segurada'									= ISNULL((SELECT SUM(cfgi.nr_area_risco) FROM dbo.corp_fruta_grao_itens cfgi WITH (NOLOCK)  WHERE cfgi.id_endosso = ce.id_endosso),0),
		'Produtividade Segurada'						= REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR,CAST((ISNULL(cfgi.qt_produto_kilo_alqueire,0) * 60) AS NUMERIC(12,2))))),'.',','),
		'Importancia Segurada'							= (SELECT SUM(cfgic.vl_is)	FROM dbo.corp_fruta_grao_itens cfgi WITH (NOLOCK) /* VALOR DE IS BÁSICA */
																INNER JOIN dbo.corp_fruta_grao_itens_coberturas cfgic	WITH (NOLOCK) ON cfgic.id_item = cfgi.id_item
																INNER JOIN dbo.corp_produto_cobertura			cpc		WITH (NOLOCK) ON cpc.id_produto_cobertura = cfgic.id_produto_cobertura
															WHERE cfgi.id_endosso = ce.id_endosso AND cpc.dv_basica = 1),
		'Numero do Endosso'								= (CASE WHEN ISNULL(CONVERT(VARCHAR(1), ca.dv_documento_oficial),'0') = '1' THEN CONVERT(VARCHAR,ce.nr_endosso) ELSE '' END),  --Deixar em branco quando se tratar epans de proposta
		'Numero do Processo do Produto na SUSEP'		= LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(ISNULL(cp.nr_processo_susep,''),'.',''),'/',''),'-',''))),
		'Grau da Latitude'								= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_grau_latitude),''),2),'.',','),	
		'Minuto da Latitude'							= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_minuto_latitude),''),2),'.',','),
		'Segundo da Latitude'							= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_segundo_latitude),''),2),'.',','),	
		'Orientacao da Latitude	'						= LTRIM(RTRIM((CASE ISNULL(cpro.nm_posicao_latitude,'S') WHEN 1 THEN 'S' ELSE 'S' END))),
		'Grau da Longitude'								= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_grau_longitude),''),2),'.',','),	
		'Minuto da Longitude'							= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_minuto_longitude),''),2),'.',','),	
		'Segundo da Longitude'							= REPLACE(LEFT(ISNULL(CONVERT(VARCHAR,cpro.nr_segundo_longitude),''),2),'.',','),	
		'Orientacao da Longitude'						= LTRIM(RTRIM((CASE ISNULL(cpro.nm_posicao_longitude,'W') WHEN 4 THEN 'W' ELSE 'W' END))),
		'Lote de Apolices/Endossos'						= '',
		'Lote do Registro Anterior'						= '',
		'Em Fiscalização'								= '',cfgi.id_local

FROM dbo.corp_apolice											ca				WITH (NOLOCK) 
		INNER JOIN dbo.corp_sub_estipulante						cse				WITH (NOLOCK) ON cse.id_apolice = ca.id_apolice
		INNER JOIN dbo.corp_endosso								ce				WITH (NOLOCK) ON ce.id_sub = cse.id_sub 
		INNER JOIN dbo.corp_ramo								cr				WITH (NOLOCK) ON cr.id_ramo = ca.id_ramo
		LEFT  JOIN dbo.corp_pessoas								cp_cli			WITH (NOLOCK) ON cp_cli.id_pessoa = ca.id_pessoa_cliente

		LEFT  JOIN dbo.corp_sub_corretor                        csc	           WITH (NOLOCK) ON ce.id_endosso = csc.id_endosso 
		LEFT  JOIN dbo.corp_pessoas                             cp_corr        WITH (NOLOCK) ON csc.id_pessoa_corretor=cp_corr.id_pessoa 


		LEFT  JOIN dbo.corp_fruta_grao_itens					cfgi			WITH (NOLOCK) ON cfgi.id_endosso = ce.id_endosso
		LEFT  JOIN dbo.corp_fruta_grao_variedade				cfgv			WITH (NOLOCK) ON cfgv.id_fruta_grao_variedade = cfgi.id_fruta_grao_variedade
		LEFT  JOIN dbo.corp_fruta_grao							cfg				WITH (NOLOCK) ON cfg.id_fruta_grao = cfgv.id_fruta_grao 
		LEFT  JOIN dbo.corp_produto_ramo						cpr				WITH (NOLOCK) ON cpr.id_ramo = cr.id_ramo and ca.cd_produto = cpr.cd_produto
		LEFT  JOIN dbo.corp_produto								cp				WITH (NOLOCK) ON cp.cd_produto = cpr.cd_produto
		LEFT  JOIN dbo.corp_empresa								c_emp			WITH (NOLOCK) ON c_emp.cd_empresa = cp.cd_empresa
		
		LEFT  JOIN dbo.corp_propriedade							cpro			WITH (NOLOCK) ON cpro.id_endosso = ce.id_endosso 
		LEFT  JOIN dbo.corp_endereco							cend			WITH (NOLOCK) ON cend.id_endereco = cpro.id_endereco
		LEFT  JOIN dbo.corp_uf									cuf_cpro		WITH (NOLOCK) ON cuf_cpro.cd_uf = cpro.cd_uf
		LEFT  JOIN dbo.corp_uf									cuf_cend		WITH (NOLOCK) ON cuf_cend.cd_uf = cend.cd_uf
		LEFT  JOIN dbo.corp_uf									cuf_item		WITH (NOLOCK) ON cuf_item.cd_uf = cfgi.cd_uf
		LEFT  JOIN dbo.corp_endereco							cend_cli		WITH(NOLOCK)  ON cend_cli.id_endereco	= (	SELECT TOP 1 cend_cli2.id_endereco
																															FROM dbo.corp_endereco cend_cli2 WITH(NOLOCK) 
																															WHERE cend_cli2.id_pessoa = cp_cli.id_pessoa																													
																															ORDER BY cend_cli2.id_endereco desc )
		LEFT  JOIN dbo.corp_localizacao							clocal_cli	    WITH (NOLOCK) ON convert(varchar(20), clocal_cli.id_local) = convert(varchar(20), cend_cli.nm_cidade)
		LEFT  JOIN dbo.corp_uf									cuf_cli2		WITH (NOLOCK) ON cuf_cli2.cd_uf = clocal_cli.cd_uf

		LEFT  JOIN dbo.corp_uf									cuf_cli			WITH (NOLOCK) ON cuf_cli.cd_uf = cend_cli.cd_uf
		LEFT  JOIN dbo.corp_cobertura_plano						ccp				WITH (NOLOCK) ON ccp.id_cobertura_plano = cp.id_cobertura_plano

		LEFT  JOIN dbo.corp_localizacao							cl				WITH (NOLOCK) ON cl.id_local = cfgi.id_local			
		LEFT  JOIN dbo.corp_produtividade_nivel_cobertura		crs				WITH (NOLOCK) ON cl.id_local = crs.id_local 
																									AND crs.id_fruta_grao =  cfg.id_fruta_grao 
		LEFT  JOIN corp_status									cst				WITH (NOLOCK) ON ca.cd_status = cst.cd_status
		
			

WHERE 
	ca.cd_tp_origem = 1 
	AND cr.nm_tabela_itens = 'corpvw_itens_fruta_grao'  
	--AND ISNULL(ca.cd_status,13) IN (7,13,601,602,606,619,1000,1003,1004,10073,1002,1004,10058,10061,10062,10063,10064,10065,10066,10067,10068,10069,10070,10071,10072,10076,10075,10074,10079) 
	AND ce.cd_tipo_endosso not in(4,6,7,8,9)
	AND ce.dt_emissao BETWEEN @dt_inicio AND @dt_fim
	and csc.dv_corretor_lider=1

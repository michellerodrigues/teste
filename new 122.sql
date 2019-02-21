script de alteração do banco mapa de homologação (e produção) somente schema

--Inclusão da tabela campoSinistro
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CampoSinistro]    Script Date: 18/05/2018 11:54:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CampoSinistro](
	[CampoSinistroId] [int] IDENTITY(1,1) NOT NULL,
	[NomeCampoSinistro] [varchar](45) NOT NULL,
	[DescricaoNegocioCampo] [varchar](45) NOT NULL,
	[DescricaoCorrecao] [varchar](300) NULL,
	[TamanhoMinimoCampo] [int] NOT NULL DEFAULT ((0)),
	[TamanhoMaximo] [int] NOT NULL DEFAULT ((255)),
 CONSTRAINT [PK_CampoSinistroId] PRIMARY KEY CLUSTERED 
(
	[CampoSinistroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


--Celulas Sinistro
/****** Object:  Table [dbo].[CelulasSinistro]    Script Date: 18/05/2018 11:54:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CelulasSinistro](
	[CelulaSinistroId] [int] IDENTITY(1,1) NOT NULL,
	[EnderecoCelula] [varchar](10) NOT NULL,
	[NomeObjeto] [varchar](30) NULL,
	[LabelCampo] [varchar](90) NULL,
	[MapaId] [int] NOT NULL,
	[CampoSinistroId] [int] NOT NULL,
	[SubstringTamanho] [int] NOT NULL DEFAULT ((0)),
	[ObjetoOLE] [bit] NOT NULL DEFAULT ((0)),
	[Obrigatorio] [bit] NOT NULL DEFAULT ((1)),
	[SomenteNumeros] [bit] NOT NULL DEFAULT ((0)),
	[CelulaAlternativa] [varchar](30) NULL,
	[LimparCaracteresEspeciais] [bit] NOT NULL DEFAULT ((0)),
	[PermitirZero] [bit] NOT NULL DEFAULT ((0)),
	[ValorDefault] [varchar](255) NULL DEFAULT (NULL),
 CONSTRAINT [PK_CelulaSinistroId] PRIMARY KEY CLUSTERED 
(
	[CelulaSinistroId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Parametros]    Script Date: 18/05/2018 11:54:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Parametros](
	[ParametrosId] [int] IDENTITY(1,1) NOT NULL,
	[NomeParametro] [varchar](90) NOT NULL,
	[DescricaoParametro] [varchar](255) NULL,
	[ValorParametro] [varchar](max) NOT NULL,
	[TipoPlanilhaId] [int] NOT NULL,
 CONSTRAINT [PK_ParametrosId] PRIMARY KEY CLUSTERED 
(
	[ParametrosId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TipoPlanilha]    Script Date: 18/05/2018 11:54:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoPlanilha](
	[TipoPlanilhaId] [int] IDENTITY(1,1) NOT NULL,
	[NomePlanilha] [varchar](90) NOT NULL,
 CONSTRAINT [PK_TipoPlanilhaId] PRIMARY KEY CLUSTERED 
(
	[TipoPlanilhaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[CelulasSinistro]  WITH CHECK ADD FOREIGN KEY([CampoSinistroId])
REFERENCES [dbo].[CampoSinistro] ([CampoSinistroId])
GO
ALTER TABLE [dbo].[CelulasSinistro]  WITH CHECK ADD FOREIGN KEY([CampoSinistroId])
REFERENCES [dbo].[CampoSinistro] ([CampoSinistroId])
GO
GO
ALTER TABLE [dbo].[CelulasSinistro]  WITH CHECK ADD FOREIGN KEY([MapaId])
REFERENCES [dbo].[Mapa] ([MapaId])
GO
ALTER TABLE [dbo].[CelulasSinistro]  WITH CHECK ADD FOREIGN KEY([MapaId])
REFERENCES [dbo].[Mapa] ([MapaId])
GO
GO
ALTER TABLE [dbo].[MapaBase]  WITH CHECK ADD FOREIGN KEY([TipoPlanilhaId])
REFERENCES [dbo].[TipoPlanilha] ([TipoPlanilhaId])
GO
ALTER TABLE [dbo].[MapaBase]  WITH CHECK ADD FOREIGN KEY([TipoPlanilhaId])
REFERENCES [dbo].[TipoPlanilha] ([TipoPlanilhaId])
GO
ALTER TABLE [dbo].[Parametros]  WITH CHECK ADD FOREIGN KEY([TipoPlanilhaId])
REFERENCES [dbo].[TipoPlanilha] ([TipoPlanilhaId])
GO
ALTER TABLE [dbo].[Parametros]  WITH CHECK ADD FOREIGN KEY([TipoPlanilhaId])
REFERENCES [dbo].[TipoPlanilha] ([TipoPlanilhaId])
GO

--Celulas
alter table Corretoras add ValorDefault varchar(255) not null default null;
alter table Corretoras drop column NomeCorretora;
alter table Corretoras add NomeCorretoraPlanilha varchar(max) not null default '';
alter table Corretoras add NomeCorretoraI4PRO varchar(max) not null default '';
alter table Corretoras add CNPJI4PRO varchar(max) not null default '';
alter table MapaBase add TipoPlanilhaId (int) NOT NULL DEFAULT ((1));


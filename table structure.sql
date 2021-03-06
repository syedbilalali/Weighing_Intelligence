USE [WIWEB_AveryDB_New]
GO
/****** Object:  Table [dbo].[SMS]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SMS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[SMSNo] [nvarchar](max) NULL,
	[DataId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SMS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SiteParameterSettings]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteParameterSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[IsGateEntry] [int] NULL,
	[Cameras] [int] NULL,
	[AlphaNumericDisplay] [int] NULL,
	[Sensors] [int] NULL,
	[Barriers] [int] NULL,
	[PASystem] [int] NULL,
	[RFIDReader] [int] NULL,
	[ConnectivityToCustomer] [int] NULL,
	[NoSpectalCharacterForTruck] [int] NULL,
	[AxleWeighting] [int] NULL,
	[TMS] [int] NULL,
	[AuthorizedForTARE] [int] NULL,
	[ToleranceCheckforCustQty] [int] NULL,
	[ToleranceCheckforSupQty] [int] NULL,
	[CreateDate] [datetime] NULL,
	[WeightMachineId] [nvarchar](max) NULL,
	[PlantCodeId] [nvarchar](max) NULL,
 CONSTRAINT [PK_SiteParameterSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShiftTiming]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShiftTiming](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoOfShift] [int] NULL,
	[ShiftName] [nvarchar](max) NULL,
	[ShifTimeFrom] [time](7) NULL,
	[ShiftTimeTo] [time](7) NULL,
 CONSTRAINT [PK_ShiftTiming] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AMCType] [nvarchar](max) NULL,
	[AMCContactNo] [nvarchar](max) NULL,
	[AMCValidUpto] [datetime] NULL,
	[AMCReminder] [int] NULL,
	[StampingDate] [datetime] NULL,
	[StampingReminder] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_ServiceMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlantMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlantMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [nvarchar](max) NULL,
	[PlantCode] [nvarchar](max) NULL,
	[PlantName] [nvarchar](max) NULL,
	[PlantAddress1] [nvarchar](max) NULL,
	[PlantAddress2] [nvarchar](max) NULL,
	[PlantContactPerson] [nvarchar](max) NULL,
	[Designation] [nvarchar](max) NULL,
	[ContactMobile] [nvarchar](max) NULL,
	[ContactEmail] [nvarchar](max) NULL,
	[NoOfMachine] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_PlantMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordPolicy]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordPolicy](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MinLength] [nvarchar](max) NULL,
	[MinNoOfCapitals] [nvarchar](max) NULL,
	[MinNoOfSmall] [nvarchar](max) NULL,
	[MinNoOfSpecials] [nvarchar](max) NULL,
	[MinNoOfNumeric] [nvarchar](max) NULL,
 CONSTRAINT [PK_PasswordPolicy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PackingMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PackingMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PackingCode] [nvarchar](max) NULL,
	[PackingName] [nvarchar](max) NULL,
	[PackingUOM] [nvarchar](max) NULL,
	[PackingWT] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_PackingMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaterialClassification]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaterialClassification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaterialClassificationCode] [nvarchar](max) NULL,
	[MaterialClassificationDesc] [nvarchar](max) NULL,
	[Supplier_VendorCode] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_MaterialClassification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MachineWorkingParameters]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MachineWorkingParameters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCodeId] [int] NULL,
	[MachineId] [int] NULL,
	[IPPort] [nvarchar](max) NULL,
	[PortNo] [nvarchar](max) NULL,
	[ModeofComs] [int] NULL,
	[StabilityNos] [nvarchar](max) NULL,
	[StabilityRange] [nvarchar](max) NULL,
	[ZeroInterlock] [int] NULL,
	[ZeroInterlockRange] [nvarchar](max) NULL,
	[TransactionNoPrefix] [nvarchar](max) NULL,
	[TareCheck] [int] NULL,
	[StoredTare] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_MachineWorkingParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[PlantCode] [nvarchar](max) NULL,
	[LogTitle] [nvarchar](max) NULL,
	[LogDescription] [nvarchar](max) NULL,
	[URL] [nvarchar](max) NULL,
	[LogDate] [datetime] NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EMail]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EMail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameOfPerson] [nvarchar](max) NULL,
	[EmailAddress] [nvarchar](max) NULL,
	[DataId] [nvarchar](max) NULL,
 CONSTRAINT [PK_EMail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DynamicFieldNames]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DynamicFieldNames](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantId] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[FieldName] [nvarchar](max) NULL,
	[FieldValue] [nvarchar](max) NULL,
	[IsMandatory1] [bit] NULL,
	[IsMandatory2] [bit] NULL,
 CONSTRAINT [PK_DynamicFieldNames] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataForSMS_EMAIL]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataForSMS_EMAIL](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DataId] [int] NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_DataForSMS_EMAIL] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[GSTNo] [nvarchar](max) NULL,
	[PanNo] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactMobile] [nvarchar](max) NULL,
	[ContactEmail] [nvarchar](max) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CompanyMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[CompanyLogo] [nvarchar](max) NULL,
	[CompanyAddress1] [nvarchar](max) NULL,
	[CompanyAddress2] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactMobile] [nvarchar](max) NULL,
	[ContactEmail] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CompanyMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CameraMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CameraMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCodeID] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[CameraIndentification] [nvarchar](max) NULL,
	[CameraIP] [nvarchar](max) NULL,
	[CameraPort] [nvarchar](max) NULL,
	[CameraUserName] [nvarchar](max) NULL,
	[CameraPwd] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_CameraMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BarrierMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BarrierMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCodeId] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[BarrierIdentification] [nvarchar](max) NULL,
	[BarrierIP] [nvarchar](max) NULL,
	[BarrierPort] [nvarchar](max) NULL,
	[BarrierScheme] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_BarrierMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AverageTareSchema]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AverageTareSchema](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_AverageTareSchema] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlphaDisplayMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlphaDisplayMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCodeId] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[AlphaDisplayIdentification] [nvarchar](max) NULL,
	[AlphaDisplayIP] [nvarchar](max) NULL,
	[AlphaDisplayPort] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_AlphaDisplayMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WeightMachineMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeightMachineMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCodeId] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[MachineName] [nvarchar](max) NULL,
	[Capacity] [nvarchar](max) NULL,
	[WeighingUnit] [nvarchar](max) NULL,
	[Resolution] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[PlatformSize] [nvarchar](max) NULL,
	[MachineNo] [nvarchar](max) NULL,
	[Indicator] [nvarchar](max) NULL,
	[LCType] [nvarchar](max) NULL,
	[NoOfLoadCells] [nvarchar](max) NULL,
	[LoadCellSerialNos] [nvarchar](max) NULL,
	[EquipmentId] [int] NULL,
	[InvoiceNo] [nvarchar](max) NULL,
	[DespatchDate] [datetime] NULL,
	[InstallationDate] [datetime] NULL,
	[WarrentyUpto] [datetime] NULL,
	[ReasonWarrentyUptoDate] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_WeightMachineMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VisitDetails]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VisitDate] [datetime] NULL,
	[ReasonOfVisit] [nvarchar](max) NULL,
	[PartsReplaced] [nvarchar](max) NULL,
	[OtherWorkDone] [nvarchar](max) NULL,
 CONSTRAINT [PK_VisitDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VehicleClassification]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VehicleClassification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassificationCode] [nvarchar](max) NULL,
	[Make] [nvarchar](max) NULL,
	[Model] [nvarchar](max) NULL,
	[NoOfAxies] [decimal](18, 0) NULL,
	[BodyType] [nvarchar](max) NULL,
	[KerbWt] [decimal](18, 2) NULL,
	[ManufactureYear] [int] NULL,
	[GrossWeight] [decimal](18, 2) NULL,
	[UOMWeight] [char](2) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_VehicleClassification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserWeightMachineMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserWeightMachineMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[WeightMachineId] [int] NULL,
 CONSTRAINT [PK_UserWeightMachineMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[UserType] [int] NULL,
	[Plantcode] [nvarchar](max) NULL,
	[WeighbridgeID] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserClassifications]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserClassifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserType] [nvarchar](25) NULL,
	[MasterFileUpdation] [bit] NULL,
	[MasterRecordDeletion] [bit] NULL,
	[PendingRecordDeletion] [bit] NULL,
	[TransactionDeletion] [bit] NULL,
	[Configuration] [bit] NULL,
	[PasswordPolicy] [bit] NULL,
	[PasswordReset] [bit] NULL,
	[UserCreation] [bit] NULL,
	[GateEntry] [bit] NULL,
	[RFIDIssue] [bit] NULL,
	[Weighment] [bit] NULL,
	[DatabaseOperation] [bit] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_UserClassifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TruckMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TruckMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TruckRegNo] [nvarchar](15) NULL,
	[ClassificationCode] [nvarchar](max) NULL,
	[StoredTareWeight] [nvarchar](max) NULL,
	[TareValidityDate] [nvarchar](max) NULL,
	[AverageTareScheme] [nvarchar](max) NULL,
	[CurrentAverageTareValue] [nvarchar](max) NULL,
	[UOMWeight] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_TruckMaster] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTransporter]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTransporter](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[GSTNo] [nvarchar](max) NULL,
	[PanNo] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactMobile] [nvarchar](max) NULL,
	[ContactEmail] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Transporter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTransactions]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TripId] [int] NULL,
	[TripType] [int] NULL,
	[IsMultiProduct] [bit] NULL,
	[GateEntryNo] [nvarchar](max) NULL,
	[TruckNo] [nvarchar](max) NULL,
	[MaterialCode] [nvarchar](max) NULL,
	[MaterialName] [nvarchar](max) NULL,
	[MaterialCalssificationCode] [nvarchar](max) NULL,
	[MaterialClassificationName] [nvarchar](max) NULL,
	[SupplierCode] [nvarchar](max) NULL,
	[SupplierName] [nvarchar](max) NULL,
	[TransporterCode] [nvarchar](max) NULL,
	[TransporterName] [nvarchar](max) NULL,
	[PackingCode] [nvarchar](max) NULL,
	[PackingName] [nvarchar](max) NULL,
	[PackingQty] [int] NULL,
	[ChallanNo] [nvarchar](max) NULL,
	[ChallanDate] [datetime] NULL,
	[ChallanWeight] [nvarchar](max) NULL,
	[PONo] [nvarchar](max) NULL,
	[PODate] [datetime] NULL,
	[POMaterials] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[TransactionStatus] [int] NULL,
	[FirstWeight] [numeric](18, 2) NULL,
	[FirstWtDateTime] [datetime] NULL,
	[SecondWeight] [numeric](18, 2) NULL,
	[SecondWtDateTime] [datetime] NULL,
	[NetWeight] [numeric](18, 2) NULL,
	[CreateDate] [datetime] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[Shift] [nvarchar](max) NULL,
	[WeighbridgeId] [nvarchar](max) NULL,
	[PlantCode] [nvarchar](max) NULL,
	[CompanyCode] [nvarchar](max) NULL,
	[FrontImage1] [varbinary](max) NULL,
	[FrontImage2] [varbinary](max) NULL,
	[BackImage1] [varbinary](max) NULL,
	[BackImage2] [varbinary](max) NULL,
	[TopImage1] [varbinary](max) NULL,
	[TopImage2] [varbinary](max) NULL,
	[WeighingUnit] [nvarchar](max) NULL,
	[RFIDCARDNO] [nvarchar](max) NULL,
	[RFIDTAGUID] [nvarchar](max) NULL,
	[CARDISSUEDATETIME] [datetime] NULL,
 CONSTRAINT [PK_tblTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblTransactionMaterials]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTransactionMaterials](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NULL,
	[MaterialCode] [nvarchar](max) NULL,
	[MaterialName] [nvarchar](max) NULL,
	[Weight] [nvarchar](max) NULL,
	[CreteDate] [datetime] NULL,
 CONSTRAINT [PK_tblTransactionMaterials] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTIMESETTINGS]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTIMESETTINGS](
	[Time_Reach_WB] [int] NULL,
	[Time_Stay_WB] [int] NULL,
	[Time_MoveOut_WB] [int] NULL,
	[Time_PB_Opening] [int] NULL,
	[Time_PB_Closing] [int] NULL,
	[Time_SteadyWt] [int] NULL,
	[Time_Balance] [int] NULL,
	[Time_B_Opening_Feedback] [int] NULL,
	[Time_B_Closing_Feedback] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblTagUid]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblTagUid](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RFIDCARDNO] [nvarchar](max) NULL,
	[RFIDTAGUID] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSupplier]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSupplier](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Address1] [nvarchar](max) NULL,
	[Address2] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[Country] [nvarchar](max) NULL,
	[GSTNo] [nvarchar](max) NULL,
	[PanNo] [nvarchar](max) NULL,
	[ContactPerson] [nvarchar](max) NULL,
	[ContactMobile] [nvarchar](max) NULL,
	[ContactEMail] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSensorMaster]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSensorMaster](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCode] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[SensorIdentification] [nvarchar](max) NULL,
	[SensorIP] [nvarchar](max) NULL,
	[SensorPort] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblRFIDCARDDATA]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRFIDCARDDATA](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RFIDCARDNO] [nvarchar](max) NULL,
	[RFIDTAGUID] [nvarchar](max) NULL,
	[TripId] [int] NULL,
	[TripType] [int] NULL,
	[IsMultiProduct] [bit] NULL,
	[GateEntryNo] [nvarchar](max) NULL,
	[TruckNo] [nvarchar](max) NULL,
	[MaterialCode] [nvarchar](max) NULL,
	[MaterialName] [nvarchar](max) NULL,
	[MaterialCalssificationCode] [nvarchar](max) NULL,
	[MaterialClassificationName] [nvarchar](max) NULL,
	[SupplierCode] [nvarchar](max) NULL,
	[SupplierName] [nvarchar](max) NULL,
	[TransporterCode] [nvarchar](max) NULL,
	[TransporterName] [nvarchar](max) NULL,
	[PackingCode] [nvarchar](max) NULL,
	[PackingName] [nvarchar](max) NULL,
	[PackingQty] [int] NULL,
	[ChallanNo] [nvarchar](max) NULL,
	[ChallanDate] [datetime] NULL,
	[ChallanWeight] [nvarchar](max) NULL,
	[PONo] [nvarchar](max) NULL,
	[PODate] [datetime] NULL,
	[POMaterials] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[WEIGHSTATUS] [nvarchar](max) NULL,
	[RFIDCARDENABLED] [bit] NULL,
	[CARDISSUEDATETIME] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMaterial]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMaterial](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MaterialCode] [nvarchar](max) NULL,
	[MaterialDesc] [nvarchar](max) NULL,
	[PackingCodeId] [nvarchar](max) NULL,
	[MaterialClassificationCodeId] [nvarchar](max) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblMachineWorkingParameters]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblMachineWorkingParameters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlantCode] [nvarchar](max) NULL,
	[MachineId] [nvarchar](max) NULL,
	[IPPort] [nvarchar](max) NULL,
	[PortNo] [nvarchar](max) NULL,
	[ModeOfComs] [nvarchar](max) NULL,
	[StabilityNos] [nvarchar](max) NULL,
	[StabilityRange] [nvarchar](max) NULL,
	[ZeroInterlock] [int] NULL,
	[ZeroInterlockRange] [nvarchar](max) NULL,
	[TransactionNoPrefix] [nvarchar](max) NULL,
	[TareCheck] [int] NULL,
	[StoredTare] [int] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_tblMachineWorkingParameters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblGateEntryRecords]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblGateEntryRecords](
	[GatePassNo] [int] IDENTITY(1,1) NOT NULL,
	[TruckNo] [nvarchar](max) NULL,
	[EntryDate] [datetime] NULL,
	[SecurityName] [nvarchar](max) NULL,
	[SecurityMarks] [nvarchar](max) NULL,
 CONSTRAINT [PK_tblGateEntryRecords] PRIMARY KEY CLUSTERED 
(
	[GatePassNo] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SQLDatabase]    Script Date: 01/30/2020 12:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SQLDatabase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProviderName] [nvarchar](max) NULL,
	[ServerName] [nvarchar](max) NULL,
	[DatabaseName] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NULL,
	[UserPassword] [nvarchar](max) NULL,
	[BackupLocation] [nvarchar](max) NULL,
	[BackupInterval] [int] NULL,
 CONSTRAINT [PK_SQLDatabase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SP_RFIDCARD_MASTER_tab]    Script Date: 01/30/2020 12:29:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--create table commodity_tab
--(
--ID INT not null,
--NAME NVARCHAR(40),
--ACTIVE bit
--)

CREATE PROCEDURE [dbo].[SP_RFIDCARD_MASTER_tab]
(
    @RFIDCARDNO nvarchar(max),
	@RFIDTAGUID nvarchar(max),
	@TripId int,
	@TripType int,
	@IsMultiProduct bit,
	@GateEntryNo nvarchar(max),
	@TruckNo nvarchar(max),
	@MaterialCode nvarchar(max),
	@MaterialName nvarchar(max),
	@MaterialCalssificationCode nvarchar(max),
	@MaterialClassificationName nvarchar(max),
	@SupplierCode nvarchar(max),
	@SupplierName nvarchar(max),
	@TransporterCode nvarchar(max),
	@TransporterName nvarchar(max),
	@PackingCode nvarchar(max),
	@PackingName nvarchar(max),
	@PackingQty int,
	@ChallanNo nvarchar(max),
	@ChallanDate datetime,
	@ChallanWeight nvarchar(max),
	@PONo nvarchar(max),
	@PODate datetime,
	@POMaterials nvarchar(max),
	@Remarks nvarchar(max),
	@WEIGHSTATUS nvarchar(max),
	@RFIDCARDENABLED bit,
	@CARDISSUEDATETIME datetime
--@ID INT ,
--@NAME NVARCHAR(40),
--@ACTIVE bit,
--@CREATEDDATE DATETIME
)
AS
BEGIN

INSERT INTO tblRFIDCARDDATA(
    RFIDCARDNO,	RFIDTAGUID,	TripId,	TripType,IsMultiProduct,GateEntryNo,
	TruckNo,MaterialCode,MaterialName,MaterialCalssificationCode,MaterialClassificationName,
	SupplierCode,SupplierName,TransporterCode,TransporterName,PackingCode,
	PackingName,PackingQty,ChallanNo,ChallanDate,ChallanWeight,PONo,PODate,POMaterials,
	Remarks,WEIGHSTATUS,RFIDCARDENABLED,CARDISSUEDATETIME)
	VALUES(@RFIDCARDNO,@RFIDTAGUID,@TripId,@TripType,
	@IsMultiProduct,@GateEntryNo,@TruckNo,@MaterialCode,@MaterialName,@MaterialCalssificationCode,
	@MaterialClassificationName,@SupplierCode,@SupplierName,@TransporterCode,@TransporterName,
	@PackingCode,@PackingName,@PackingQty,@ChallanNo,@ChallanDate,@ChallanWeight,@PONo,@PODate,
	@POMaterials,@Remarks,@WEIGHSTATUS,@RFIDCARDENABLED,@CARDISSUEDATETIME)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_DateWiseDashboard]    Script Date: 01/30/2020 12:29:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_DateWiseDashboard]
	(
	@From varchar(10)=null,
	@To  varchar(10)=null,
	@Type varchar(100) =null,
	@Option varchar(100)
	)
	AS
	BEGIN
	if(@Option='DateWise')
	BEGIN
		SELECT * FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] 
		where convert(varchar(10), SecondWtDateTime, 120)  
		between  @From and  @To

	END
	if(@Option='FillMaterial')
	BEGIN
		SELECT distinct  MaterialName FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] 
		where convert(varchar(10), SecondWtDateTime, 120)  
		between  @From and  @To
	END
	if(@Option='FillMaterialData')
	BEGIN
		SELECT * FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] 
		where convert(varchar(10), SecondWtDateTime, 120)  
		between  @From and  @To and MaterialName=@Type
	END
	
	END
GO
/****** Object:  StoredProcedure [dbo].[sp_DailyDashboard]    Script Date: 01/30/2020 12:29:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_DailyDashboard]
	
	AS
	BEGIN
		SELECT * FROM [WIWEB_AveryDB_New].[dbo].[tblTransactions] 
		--where convert(varchar(10), FirstWtDateTime, 120) >= convert(varchar(10), GETDATE(), 120)

	END
GO

USE [PBL3_demo]
GO
/****** Object:  Table [dbo].[AccessRight]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccessRight](
	[accessRightID] [int] NOT NULL,
	[accessRightName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_AccessRight] PRIMARY KEY CLUSTERED 
(
	[accessRightID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[accountID] [int] NOT NULL,
	[accountName] [nvarchar](50) NOT NULL,
	[accountPassword] [nvarchar](50) NOT NULL,
	[accessRightID] [int] NOT NULL,
	[Salt] [nvarchar](200) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[accountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[customerID] [nvarchar](200) NOT NULL,
	[customerName] [nvarchar](200) NOT NULL,
	[customerLocation] [nvarchar](200) NOT NULL,
	[customerPhoneNumber] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[customerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoice]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoice](
	[invoiceID] [int] NOT NULL,
	[parcelID] [int] NOT NULL,
	[customerID] [nvarchar](200) NOT NULL,
	[cost] [float] NULL,
	[outputTime] [datetime] NULL,
 CONSTRAINT [PK_Invoice] PRIMARY KEY CLUSTERED 
(
	[invoiceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parcel]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parcel](
	[parcelID] [int] NOT NULL,
	[parcelName] [nvarchar](200) NOT NULL,
	[parcelMass] [float] NOT NULL,
	[parcelSize] [nvarchar](200) NOT NULL,
	[parcelValue] [float] NOT NULL,
	[createTime] [datetime] NULL,
	[RCustomerID] [nvarchar](200) NOT NULL,
	[SCustomerID] [nvarchar](200) NOT NULL,
	[type] [bit] NULL,
	[shippingMethod] [bit] NOT NULL,
	[parcelStatus] [bit] NULL,
 CONSTRAINT [PK_Parcel] PRIMARY KEY CLUSTERED 
(
	[parcelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Receptionist]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Receptionist](
	[receptionistID] [nvarchar](200) NOT NULL,
	[receptionistName] [nvarchar](200) NOT NULL,
	[receptionistLocation] [nvarchar](200) NOT NULL,
	[receptionistPhoneNumber] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Receptionist] PRIMARY KEY CLUSTERED 
(
	[receptionistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Route]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Route](
	[routeID] [int] NOT NULL,
	[parcelID] [int] NOT NULL,
	[fromWarehouseID] [nvarchar](200) NOT NULL,
	[details] [nvarchar](200) NULL,
	[time] [datetime] NOT NULL,
 CONSTRAINT [PK_Route] PRIMARY KEY CLUSTERED 
(
	[routeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipFee]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipFee](
	[shippingFeeID] [int] NOT NULL,
	[shippingMethod] [bit] NOT NULL,
	[totalCost] [float] NOT NULL,
	[locations] [float] NOT NULL,
	[parcelID] [int] NOT NULL,
 CONSTRAINT [PK_ShipFee] PRIMARY KEY CLUSTERED 
(
	[shippingFeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Warehouse]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Warehouse](
	[warehouseID] [nvarchar](200) NOT NULL,
	[warehouseName] [nvarchar](200) NOT NULL,
	[warehouseAddress] [nvarchar](200) NOT NULL,
	[capacity] [int] NOT NULL,
 CONSTRAINT [PK_Warehouse] PRIMARY KEY CLUSTERED 
(
	[warehouseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WarehouseManager]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseManager](
	[warehouseManagerID] [nvarchar](200) NOT NULL,
	[warehouseManagerName] [nvarchar](200) NOT NULL,
	[warehouseManagerLocation] [nvarchar](200) NOT NULL,
	[warehouseManagerPhoneNumber] [nvarchar](200) NOT NULL,
	[warehouseID] [nvarchar](200) NOT NULL,
	[numberOfEmployee] [int] NOT NULL,
 CONSTRAINT [PK_WarehouseManager] PRIMARY KEY CLUSTERED 
(
	[warehouseManagerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WarehouseStaff]    Script Date: 5/6/2023 4:29:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WarehouseStaff](
	[warehouseStaffID] [nvarchar](200) NOT NULL,
	[warehouseStaffName] [nvarchar](200) NOT NULL,
	[warehouseStaffLocation] [nvarchar](200) NOT NULL,
	[warehouseStaffPhoneNumber] [nvarchar](200) NOT NULL,
	[warehouseID] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_WarehouseStaff] PRIMARY KEY CLUSTERED 
(
	[warehouseStaffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AccessRight] ([accessRightID], [accessRightName]) VALUES (1, N'Receptionist')
INSERT [dbo].[AccessRight] ([accessRightID], [accessRightName]) VALUES (2, N'WarehouseStaff')
INSERT [dbo].[AccessRight] ([accessRightID], [accessRightName]) VALUES (3, N'WarehouseManager')
GO
INSERT [dbo].[Account] ([accountID], [accountName], [accountPassword], [accessRightID], [Salt]) VALUES (1, N'user01', N'123', 1, NULL)
INSERT [dbo].[Account] ([accountID], [accountName], [accountPassword], [accessRightID], [Salt]) VALUES (2, N'user02', N'123', 2, NULL)
INSERT [dbo].[Account] ([accountID], [accountName], [accountPassword], [accessRightID], [Salt]) VALUES (3, N'user03', N'123', 3, NULL)
INSERT [dbo].[Account] ([accountID], [accountName], [accountPassword], [accessRightID], [Salt]) VALUES (4, N'user04', N'123', 1, NULL)
INSERT [dbo].[Account] ([accountID], [accountName], [accountPassword], [accessRightID], [Salt]) VALUES (5, N'user05', N'123', 2, NULL)
GO
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'048203001250', N'Trần Quang Bảo', N'2 Trường Chinh,Liên Chiểu,Đà Nẵng', N'1234556778')
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'048203001263', N'Trần Minh Nhật', N'K25/17 Ngũ Hành Sơn,Mỹ An,Ngũ Hành Sơn,Đà Nẵng', N'0905912809')
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'1', N'1', N'9,9,9', N'9')
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'2', N'2', N'2,2,2', N'2')
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'4', N'4', N'4,4,4', N'4')
INSERT [dbo].[Customer] ([customerID], [customerName], [customerLocation], [customerPhoneNumber]) VALUES (N'5', N'5', N'5,5,5', N'5')
GO
INSERT [dbo].[Parcel] ([parcelID], [parcelName], [parcelMass], [parcelSize], [parcelValue], [createTime], [RCustomerID], [SCustomerID], [type], [shippingMethod], [parcelStatus]) VALUES (100000, N'bàn phím cơ', 2.45, N'130x50x455', 6100000, NULL, N'048203001263', N'048203001250', 1, 1, 0)
INSERT [dbo].[Parcel] ([parcelID], [parcelName], [parcelMass], [parcelSize], [parcelValue], [createTime], [RCustomerID], [SCustomerID], [type], [shippingMethod], [parcelStatus]) VALUES (100001, N'3', 3, N'3', 3, NULL, N'2', N'1', 0, 0, NULL)
INSERT [dbo].[Parcel] ([parcelID], [parcelName], [parcelMass], [parcelSize], [parcelValue], [createTime], [RCustomerID], [SCustomerID], [type], [shippingMethod], [parcelStatus]) VALUES (100002, N'6', 6, N'6', 6, NULL, N'5', N'4', 1, 0, NULL)
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_AccessRight] FOREIGN KEY([accessRightID])
REFERENCES [dbo].[AccessRight] ([accessRightID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_AccessRight]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Customer] FOREIGN KEY([customerID])
REFERENCES [dbo].[Customer] ([customerID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Customer]
GO
ALTER TABLE [dbo].[Invoice]  WITH CHECK ADD  CONSTRAINT [FK_Invoice_Parcel] FOREIGN KEY([parcelID])
REFERENCES [dbo].[Parcel] ([parcelID])
GO
ALTER TABLE [dbo].[Invoice] CHECK CONSTRAINT [FK_Invoice_Parcel]
GO
ALTER TABLE [dbo].[Parcel]  WITH CHECK ADD  CONSTRAINT [FK_Parcel_Customer] FOREIGN KEY([RCustomerID])
REFERENCES [dbo].[Customer] ([customerID])
GO
ALTER TABLE [dbo].[Parcel] CHECK CONSTRAINT [FK_Parcel_Customer]
GO
ALTER TABLE [dbo].[Parcel]  WITH CHECK ADD  CONSTRAINT [FK_Parcel_Customer1] FOREIGN KEY([SCustomerID])
REFERENCES [dbo].[Customer] ([customerID])
GO
ALTER TABLE [dbo].[Parcel] CHECK CONSTRAINT [FK_Parcel_Customer1]
GO
ALTER TABLE [dbo].[Route]  WITH CHECK ADD  CONSTRAINT [FK_Route_Parcel] FOREIGN KEY([parcelID])
REFERENCES [dbo].[Parcel] ([parcelID])
GO
ALTER TABLE [dbo].[Route] CHECK CONSTRAINT [FK_Route_Parcel]
GO
ALTER TABLE [dbo].[Route]  WITH CHECK ADD  CONSTRAINT [FK_Route_Warehouse] FOREIGN KEY([fromWarehouseID])
REFERENCES [dbo].[Warehouse] ([warehouseID])
GO
ALTER TABLE [dbo].[Route] CHECK CONSTRAINT [FK_Route_Warehouse]
GO
ALTER TABLE [dbo].[ShipFee]  WITH CHECK ADD  CONSTRAINT [FK_ShipFee_Parcel] FOREIGN KEY([parcelID])
REFERENCES [dbo].[Parcel] ([parcelID])
GO
ALTER TABLE [dbo].[ShipFee] CHECK CONSTRAINT [FK_ShipFee_Parcel]
GO
ALTER TABLE [dbo].[WarehouseManager]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseManager_Warehouse] FOREIGN KEY([warehouseID])
REFERENCES [dbo].[Warehouse] ([warehouseID])
GO
ALTER TABLE [dbo].[WarehouseManager] CHECK CONSTRAINT [FK_WarehouseManager_Warehouse]
GO
ALTER TABLE [dbo].[WarehouseStaff]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseStaff_Warehouse] FOREIGN KEY([warehouseID])
REFERENCES [dbo].[Warehouse] ([warehouseID])
GO
ALTER TABLE [dbo].[WarehouseStaff] CHECK CONSTRAINT [FK_WarehouseStaff_Warehouse]
GO

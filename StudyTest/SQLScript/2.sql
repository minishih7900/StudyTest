USE [MyDB]
GO

/****** Object:  Table [dbo].[TwLot59]    Script Date: 2020/6/10 上午 10:50:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TwLot59_Orderby](
	[期數] [nvarchar](20) NOT NULL,
	[開獎日期] [nvarchar](20) NOT NULL,
	[號碼1] [nvarchar](2) NULL,
	[號碼2] [nvarchar](2) NULL,
	[號碼3] [nvarchar](2) NULL,
	[號碼4] [nvarchar](2) NULL,
	[號碼5] [nvarchar](2) NULL,
 CONSTRAINT [PK_開獎記錄表_排序_1] PRIMARY KEY CLUSTERED 
(
	[期數] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



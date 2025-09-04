USE SBOAlessi
GO

/****** Object:  Table [dbo].[ImagesEAN13BarCode]    Script Date: 08/03/2018 10:20:53 ******/
DROP TABLE [dbo].[ImagesEAN13BarCode]
GO

/****** Object:  Table [dbo].[ImagesEAN13BarCode]    Script Date: 08/03/2018 10:20:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ImagesEAN13BarCode](
	[ItemCode] [nvarchar](100) NOT NULL,
	[ImageEAN13] [image] NULL,
	[BarCode] [nvarchar](100) NULL,
 CONSTRAINT [PK_ImagesEAN13BarCode] PRIMARY KEY CLUSTERED 
(
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO




--select * from ImagesEAN13BarCode
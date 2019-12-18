USE [Claqueta]
GO

-- USUARIOS --

INSERT INTO [dbo].[Usuarios]
           ([Nombre], [Apellido], [TipoDocumento], [Documento], [FechaDeNacimiento]
		   ,[Telefono], [Email], [Password], [Activo], [Discriminator])
     VALUES
           ('Felipe', 'Prestador', 'DNI', '37999854', '1990-05-05 00:00:00.000',
           '1199998888', 'prestador@claqueta.com.ar', 'contra#123', '1', 'Usuario')
GO

INSERT INTO [dbo].[Usuarios]
           ([Nombre], [Apellido], [TipoDocumento], [Documento], [FechaDeNacimiento]
		   ,[Telefono], [Email], [Password], [Activo], [Discriminator])
     VALUES
           ('Carlos', 'Prestatario', 'DNI', '34462257', '1985-10-02 00:00:00.000',
           '156934782', 'prestatario@claqueta.com.ar', 'contra#123', '1', 'Usuario')
GO

-- ROLES DE USUARIOS (ROL_ID = 1 -> USER) --

INSERT INTO [dbo].[RolUsuarios]
           ([Rol_Id], [Usuario_Id])
     VALUES
           (1, 2)
GO

INSERT INTO [dbo].[RolUsuarios]
           ([Rol_Id], [Usuario_Id])
     VALUES
           (1, 3)
GO

-- PUBLICACIONES --

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (4 -- Categoria
           ,'1,2,3,4,5' -- Disponibilidad
           ,'CABA' -- Ubicacion
           ,'Drones para Videos' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_e5912505-9e97-4599-adcc-9187d059e9e9_dron.jpg' -- Foto
           ,12300 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-11-10 14:07:50.700'
           ,'2019-11-10 14:07:50.700'
           ,0 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (10 -- Categoria
           ,'1,2,3,4' -- Disponibilidad
           ,'CABA' -- Ubicacion
           ,'Servicio de Extra' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_3f211ce4-eebd-4eff-bc07-ff1df5e124aa_extra.jpg' -- Foto
           ,5600 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-10-10 14:07:50.700'
           ,'2019-10-10 14:07:50.700'
           ,0 -- Promocionada
           ,0 -- Visible
           ,'Pendiente' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (5 -- Categoria
           ,'1,2,4,5,6' -- Disponibilidad
           ,'Belgrano' -- Ubicacion
           ,'Edición de Videos' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_87b1c5bc-6224-4676-9b11-637d3c87ac20_editor_videos.jpg' -- Foto
           ,4320 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-09-09 14:07:50.700'
           ,'2019-09-09 14:07:50.700'
           ,0 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (17 -- Categoria
           ,'1,2,3,4,5,6' -- Disponibilidad
           ,'Villa del Parque' -- Ubicacion
           ,'Sonidista' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_49d31600-c167-4f73-99f8-85418dd9f2bd_sonidista.jpg' -- Foto
           ,8450 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-08-05 14:07:50.700'
           ,'2019-08-05 14:07:50.700'
           ,1 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (6 -- Categoria
           ,'0,1,2,3,4' -- Disponibilidad
           ,'CABA' -- Ubicacion
           ,'Efectos FX' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_391635b4-1a9a-49ba-a7f0-d24105695a36_efectos_fx.jpg' -- Foto
           ,6600 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-03-01 14:07:50.700'
           ,'2019-03-01 14:07:50.700'
           ,0 -- Promocionada
           ,0 -- Visible
           ,'Pendiente' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (7 -- Categoria
           ,'1,2,3,4,5' -- Disponibilidad
           ,'Almagro' -- Ubicacion
           ,'Equipos de Audio' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_d6ffce95-d535-4b17-b70d-8d2f0ce4bae4_equipos_audio.jpg' -- Foto
           ,9150 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-12-09 14:07:50.700'
           ,'2019-12-09 14:07:50.700'
           ,1 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (3 -- Categoria
           ,'1,2,3,4,5' -- Disponibilidad
           ,'Colegiales' -- Ubicacion
           ,'Castineras' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_2c16e4e8-3535-4a71-a6e3-5ea30ae8fe35_descarga.png' -- Foto
           ,8700 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-07-04 14:07:50.700'
           ,'2019-07-04 14:07:50.700'
           ,0 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO

INSERT INTO [dbo].[Publicaciones]
           ([Categoria_Id], [Disponibilidad], [Ubicacion], [Titulo],
		    [Descripcion], [Foto], [Precio], [Referencias],
			[CV], [Reel], [FechaDePublicacion], [FechaDeModificacion],
			[Promocionada], [Visible], [Estado], [Usuario_Id])
     VALUES
           (14 -- Categoria
           ,'1,2,3,4,5,6' -- Disponibilidad
           ,'Nuñez' -- Ubicacion
           ,'Maquillaje' -- Titulo
           ,'Soy una descripción del servicio publicado.'
           ,'/UploadedFiles/2_7b802bb3-fa91-4b8a-9981-02ee7b34e03e_maquillaje.jpg' -- Foto
           ,8745 -- Precio
           ,NULL -- Referencias
           ,'/UploadedFiles/2_c4c7f22d-5230-4eca-b141-69af8778dabc_curriculum_vitae_test.pdf' -- CV
           ,NULL -- Reel
           ,'2019-06-07 14:07:50.700'
           ,'2019-06-07 14:07:50.700'
           ,1 -- Promocionada
           ,1 -- Visible
           ,'Aprobada' -- Estados: Pendiente / Aprobada
           ,2)
GO
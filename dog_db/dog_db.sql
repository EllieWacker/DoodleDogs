/* drop the database if it exists */

print '' print '*** dropping database dog_db'
GO
IF EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = 'dog_db')
BEGIN 
	DROP DATABASE [dog_db]
END
GO

print '' print '*** creating database dog_db'
GO 
CREATE DATABASE [dog_db]
GO

print '' print '*** using database dog_db'
GO 
USE [dog_db]
GO 

print '' print '*** creating user table'
GO
CREATE TABLE [dbo].[User] (
	[UserID]				[int] IDENTITY(1000000, 1)	NOT NULL,
	[GivenName]				[nvarchar](50)				NOT NULL,
	[FamilyName]			[nvarchar](100)				NOT NULL,
	[PhoneNumber]			[nvarchar](11)				NOT NULL,
	[Email]					[nvarchar](250)				NOT NULL,
	[PasswordHash]			[nvarchar](100)				NOT NULL DEFAULT
		'9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e',
	[Active]				[bit]						NOT NULL DEFAULT 1,
	
	CONSTRAINT	[pk_userid]	PRIMARY KEY([UserID] ASC),
	CONSTRAINT [ak_email] UNIQUE([Email] ASC)
)
GO

print '' print '*** inserting user test records'
GO
INSERT INTO [dbo].[User]
		([GivenName], [FamilyName], [PhoneNumber], [Email])
	VALUES	
		('System', 'Admin', '000000000', 'admin@company.com'),
		('Livie', 'Wacker', '1319111111', 'livie@company.com'), 
		('Avena', 'Williams', '13195555', 'avena@company.com'),
		('Sprig', 'Plantar', '131955', 'sprig@company.com'),
		('King', 'Claw', '13195555', 'king@company.com'),
		('Fonzi', 'Dawg', '131955551', 'fonzi@company.com'),
		('Sarah', 'Jeanne', '131955551', 'sarah@company.com')
GO

print '' print '*** give admin password'
GO
UPDATE  	[dbo].[User]
	SET 	[PasswordHash] = 'b03ddf3ca2e714a6548e7495e2a03f5e824eaac9837cd7f159c67b90fb4b7342'
	WHERE	[Email] = 'admin@company.com'
GO



print '' print '*** create an inactive user'
GO
UPDATE  	[dbo].[User]
	SET 	[Active] = 0
	WHERE	[Email] = 'fonzi@company.com'
GO

print '' print '*** create userrole table'
GO
CREATE TABLE [dbo].[UserRole] (
	[UserRoleID]	[nvarchar](50)		NOT NULL,
	[UserID]		[int]				NOT NULL,
	[Description]	[nvarchar](250)		NOT NULL,
	
	CONSTRAINT [pk_user_roleid] PRIMARY KEY([UserRoleID],[UserID]),
	CONSTRAINT [fk_user_userid] FOREIGN KEY ([UserID]) 
        REFERENCES [dbo].[User]([UserID])
)
GO

print '' print '*** inserting user role test records'
GO
INSERT INTO [dbo].[UserRole] 
		([UserRoleID],	[UserID], [Description])
	VALUES
	('Admin', 1000000, 'Manages users and user accounts.'),
	('User',   1000001, 'Basic user with no special access.'),
	('User',   1000002, 'Basic user with no special access.'),
	('User',   1000006, 'Basic user with no special access.'),
	('User',   1000004, 'Basic user with no special access.'),
	('Adopter',1000005, 'User who has been approved to adopt.'),
	('No Access',1000003, 'Banned from everything.')
GO

print '' print '*** creating sp_insert_roles'
GO
CREATE PROCEDURE [dbo].[sp_insert_roles]
	(
		@UserRoleID			[nvarchar](50),
		@UserID				[int],
		@Description		[nvarchar](250)
	)
AS
	BEGIN
		INSERT INTO [dbo].[UserRole]
			([UserRoleID], [UserID], [Description])
		VALUES	
			(@UserRoleID, @UserID, @Description)
			
		 SELECT @UserRoleID AS InsertedUserRoleID;
	END
GO    



print '' print '*** creating breed table'
GO
CREATE TABLE [dbo].[Breed] (
	[BreedID]			[nvarchar](50)				NOT NULL,
	[Size]				[nvarchar](50)				NULL,
	[Image]				[nvarchar](30)				NULL ,
	[Hypoallergenic]	[bit]						NULL DEFAULT 1,
	[LifeExpectancy]	[nvarchar](50)				NOT NULL,
	[GoodDogs]			[bit]						NULL DEFAULT 1,
	[GoodKids]			[bit]						NULL DEFAULT 1,
	[Description]		[nvarchar](1000)			NOT NULL,
	
	CONSTRAINT	[pk_breedid]	PRIMARY KEY([BreedID] ASC)
)
GO

print '' print '*** inserting breed test records'
GO
INSERT INTO [dbo].[Breed] 
		([BreedID], [Size], [Image], [Hypoallergenic], [LifeExpectancy], [GoodDogs], [GoodKids], [Description])
	VALUES
	('Miniature Poodle', '10-15 inches', 'poodle.jpg', 1, '12-17 years', 1, 1, 'Miniature poodles are intelligent, athletic dogs who love companionship and are easy to get along with. Although they are occasionally characterized as a finicky breed, miniature poodles are wonderful family dogs thanks to their smarts, their eager-to-please attitude, and their gentle demeanor with kids and other pets. They are an easy breed to train, low-allergen, and low-odor, so they fit in well with most homes and families. Though their low-shedding curly coats have the benefit of being nearly hypoallergenic, they do require lots of care and grooming. If you have the time and resources to dedicate to these peaceful pups, they make loving and loyal companions.'),
	('Australian Shepard', '18-21 inches', 'australian.jpg', 0, '13-15 years', 1, 1, 'Aussies are active yet easy-going dogs that love to romp with children. They tend to get along well with other pets. The breed is considered highly intelligent and easy to train. Aussies are known for being especially eager to please their owners. True to their herding instincts, Aussies are very protective of their families and territory and will let you know if strangers approach, but they are not considered aggressive.'),
	('American Cocker Spaniel', '13.3-15.5 inches', 'cockerSpaniel.jpg', 0, '10-12 years', 1, 1, 'The American Cocker Spaniel started out as the English Cocker Spaniel but was bred with other breeds because the Americans liked their hunting dogs smaller in order to hunt game birds. They are medium-sized dogs, with a medium-sized energy level and a generally happy personality. They are a smart breed, easy to train for hunting, and loyal to their family.'),
	('Golden Retriever', '12-18 inches', 'goldenRetriever.jpg', 0, '10-12 years', 1, 1, 'The golden retriever is even-tempered, intelligent and affectionate. Golden retrievers are playful, yet gentle with children, and they tend to get along well with other pets and strangers.These dogs are eager to please, which probably explains why they respond so well to obedience training and are such popular service dogs. They also like to work, whether it involves hunting birds or fetching their guardians slippers. Golden retrievers are not often barkers, and they lack guard instincts, so do not count on them to make good watchdogs. However, some golden retrievers will let you know when strangers are approaching.'),
	('Mini Aussiedoodle', '10-15 inches', 'aussiedoodle.jpg', 1, 'Up to 15 years', 1, 1, 'The Mini Aussiedoodle is a popular and lovable breed that has captured the hearts of dog lovers everywhere. This cute and cuddly crossbreed is a delightful mix of the Australian Shepherd and the Miniature Poodle, resulting in a smart and friendly pup that makes a fantastic family companion. With their small size and spirited personality, the Mini Aussiedoodle puppy is perfect for those who love a dog that is both charming and easy to care for. '),
	('Cockapoo', '12-15 inches', 'cockapoo.jpg', 1, '12-15 years', 1, 1, 'Known for their playful and sociable demeanor, Cockapoos make excellent family pets and get along well with children and other animals. They are generally adaptable to various living situations, whether in apartments or houses with yards. They require regular exercise to stay happy and healthy. Due to their low-shedding coats, Cockapoos are often considered a good choice for individuals with allergies. Additionally, their friendly disposition and eagerness to please make them easily trainable, even for first-time dog owners.'),
	('Mini Goldendoodle', '16-20 inches', 'goldendoodle.jpg', 1, '10-15 years', 1, 1, 'The Miniature Goldendoodle is a friendly, affectionate and attentive breed. This soft and cuddly dog craves your attention, enjoys playing games, and also loves to snooze on your lap or feet. They can also be quite goofy and will enjoy entertaining you with their tricks and high energy antics. Mini Goldendoodles embody the perfect blend of attractive appearance, loyalty, and bright, sociable personalities. These affectionate and eager-to-please dogs are low-shedding, making them fantastic family pets. ')
GO


print '' print '*** creating father dog table'
GO
CREATE TABLE [dbo].[FatherDog] (
	[FatherDogID]		[nvarchar](50)				NOT NULL,
	[BreedID]			[nvarchar](50)				NOT NULL,
	[Personality]		[nvarchar](50)				NOT NULL,
	[EnergyLevel]		[nvarchar](50)				NOT NULL,
	[BarkingLevel]		[nvarchar](50)				NOT NULL,
	[Trainability]		[nvarchar](100)				NOT NULL,
	[Image]				[nvarchar](30)				NULL,
	[Description]		[nvarchar](1000)			NOT NULL DEFAULT '',
	
	CONSTRAINT	[pk_fatherdogid]	PRIMARY KEY([FatherDogID] ASC),
	CONSTRAINT 	[fk_breedid] 		FOREIGN KEY([BreedID])
		REFERENCES [Breed]([BreedID])
)
GO

print '' print '*** inserting father dog test records'
GO
INSERT INTO [dbo].[FatherDog] 
		([FatherDogID],	[BreedID], [Personality], [EnergyLevel], [BarkingLevel], [Trainability], [Image], [Description])
	VALUES
	('Harold', 'Miniature Poodle', 'Playful', 'Full of Energy', 'Medium', 'Quick to learn', 'fatherHarold.jpg', 'Harold was born August 9, 2022, in Des Moines, IA.  He arrived at his new home in Cedar Rapids, IA on November 22, 2022. What a beautiful miniature black male poodle. When we first laid eyes on him, he melted our hearts. As time has gone by, it has been a joy to raise such a smart dog. He loves to do tricks, and when engaging with other puppies he can show off his quickness by stealing your glove and keeping it away from other puppies. He also knows how to stand his ground when needed. We are looking forward to the new puppies Harold will father in the near future.'),
	('Finn', 'Miniature Poodle', 'Cuddly', 'Full of Energy', 'Low', 'Moderate', 'fatherFinn.jpg', 'Finn is one of the sweetest cuddliest puppies you will ever meet! With an easygoing friendly personality, Finn is always making new friends. His soft fluffy brindle furr and beautiful blue/green eyes set Finn apart from other dogs. Everyone adores Finn and we hope you will adore his puppies too!')
GO


print '' print '*** creating mother dog table'
GO
CREATE TABLE [dbo].[MotherDog] (
	[MotherDogID]		[nvarchar](50)				NOT NULL,
	[BreedID]			[nvarchar](50)				NOT NULL,
	[Personality]		[nvarchar](100)				NOT NULL,
	[EnergyLevel]		[nvarchar](50)				NOT NULL,
	[BarkingLevel]		[nvarchar](50)				NOT NULL,
	[Trainability]		[nvarchar](100)				NOT NULL,
	[Image]				[nvarchar](30)				NULL,
	[Description]		[nvarchar](1000)			NOT NULL DEFAULT '',
	
	CONSTRAINT	[pk_motherdogid]	PRIMARY KEY([MotherDogID] ASC),
	CONSTRAINT 	[fk_breedid2] 		FOREIGN KEY([BreedID])
		REFERENCES [Breed]([BreedID])
)
GO

print '' print '*** inserting mother dog test records'
GO
INSERT INTO [dbo].[MotherDog] 
		([MotherDogID],	[BreedID], [Personality], [EnergyLevel], [BarkingLevel], [Trainability], [Image], [Description])
	VALUES
	('Mya', 'Golden Retriever', 'Relaxed', 'Low to Medium', 'Low', 'Impressively quick', 'motherMya.jpg', 'Mya was born June 26, 2020, in Antler, OK. She arrived at her new home in Cedar Rapids, IA on September 3, 2020. Mya is smart, a quick learner, and just loves the many hugs we give her. It is not often you see such loyalty in a dog, but it is so evident in Mya. Famous for her love of stuffed animals, Mya is known to wander the property with one in her mouth. She also enjoys sharing her treasures with her new puppies! We are anxious to see the many adorable puppies Mya will raise over time.
'),
	('Clemmy', 'Australian Shepard', 'Playful', 'High', 'Moderate', 'High', 'motherClemmy.png', 'Clemmy (Clementine) was born November 28, 2020, in Ellsworth, MN. She arrived at her new home in Cedar Rapids, IA on January 18, 2021. What a lovely looking blue merle coat color she has, coming from the finest of stock in the Australian Shepherd breed. This unique look just sets Clemmy apart, but it is also evident there is so much more that Clemmy possesses. The poise and intelligence she displays is amazing. We are looking forward to seeing Clemmy mother her cute little Aussiedoodle puppies through the years.
'),
	('Rosie', 'American Cocker Spaniel', 'Affectionate', 'Moderate', 'Low to Moderate', 'Medium', 'motherRosie.jpg', 'Rosie was born June 26, 2020, in Antler, OK. She arrived at her new home in Cedar Rapids, IA on September 3, 2020. Upon arrival it was immediately clear we had purchased one of the most lovable Cocker Spaniels there are. Over time we have found Rosie to be a loyal and lovable dog, who is always happy to cuddle. Rosie has grown up quickly, because of her many interactions with other dogs and children, but mostly from the loving home she has been given. We are looking forward to seeing the many adorable puppies Rosie will raise.
')
GO


print '' print '*** creating litter table'
GO
CREATE TABLE [dbo].[Litter] (
	[LitterID]			[nvarchar](50)				NOT NULL,
	[FatherDogID]		[nvarchar](50)				NOT NULL,
	[MotherDogID]		[nvarchar](50)				NOT NULL,
	[Image]				[nvarchar](30)				NULL,
	[DateOfBirth]		[DATE]						NOT NULL,
	[GoHomeDate]		[DATE]						NOT NULL,
	[NumberPuppies]		[int]						NOT NULL,
	
	CONSTRAINT	[pk_litterid]		PRIMARY KEY([LitterID] ASC),
	CONSTRAINT 	[fk_fatherdogid] 	FOREIGN KEY([FatherDogID])
		REFERENCES [FatherDog]([FatherDogID]),
	CONSTRAINT 	[fk_motherdogid] 	FOREIGN KEY([MotherDogID])
		REFERENCES [MotherDog]([MotherDogID])
)
GO
print '' print '*** inserting litter test records'
GO
INSERT INTO [dbo].[Litter] 
		([LitterID], [FatherDogID], [MotherDogID], [Image], [DateOfBirth], [GoHomeDate], [NumberPuppies])
	VALUES
	('AussieLit1', 'Harold', 'Clemmy', 'aussieLit1.png', '2023-12-10', '2024-01-02', 5),
	('CockerLit1', 'Harold', 'Rosie', 'cockerLit1.png', '2024-01-01', '2024-05-05', 3),
	('GoldenLit1', 'Harold', 'Mya', 'goldenLit1.png', '2024-04-07', '2024-07-10', 6),
	
	('AussieLit2', 'Harold', 'Clemmy', 'aussieLit2.jpg', '2025-02-02', '2025-04-10', 4),
	('CockerLit2', 'Harold', 'Rosie', 'cockerLit2.jpg', '2025-02-20', '2025-04-19', 5),
	('GoldenLit2', 'Harold', 'Mya', 'goldenLit2.jpg', '2025-03-21', '2025-05-20', 5),
	
	('AussieLit3', 'Finn', 'Clemmy', 'aussieLit3.png', '2025-02-20', '2025-04-20', 6),
	('CockerLit3', 'Finn', 'Rosie', 'cockerLit3.png', '2025-03-01', '2025-05-04', 4),
	('GoldenLit3', 'Finn', 'Mya', 'goldenLit3.png', '2025-04-01', '2025-06-02', 4)
GO

print '' print '*** creating medical records table'
GO
CREATE TABLE [dbo].[MedicalRecord] (
	[MedicalRecordID]	[nvarchar](50)			NOT NULL,
	[Treatments]		[nvarchar](100)			NULL,
	[Weight]			[int]					NOT NULL,
	[Issues]			[nvarchar](200)			NULL,
	
	CONSTRAINT	[pk_medicalrecordid]	PRIMARY KEY([MedicalRecordID] ASC)
)
GO
print '' print '*** inserting med record test records'
GO
INSERT INTO [dbo].[MedicalRecord] 
		([MedicalRecordID],[Treatments],[Weight], [Issues])
	VALUES
	('LukeWacker1', 'Deworming', 2.5, 'None'),
	('LukeWacker2', 'Deworming', 3, 'None'),
	('LukeWacker3', 'Deworming', 3.5, 'None'),
	
	('AmyMeyer1', 'Deworming', 2.5, 'None'),
	('AmyMeyer2', 'Deworming', 3, 'None'),
	('AmyMeyer3', 'Deworming', 3.5, 'None'),
	('AmyMeyer4', 'Deworming', 3.5, 'None'),
	
	('SusanFaley1', 'Deworming', 2.5, 'None'),
	('SusanFaley2', 'Deworming', 3, 'None'),
	('SusanFaley3', 'Deworming', 4, 'None'),
	('SusanFaley4', 'Deworming', 3, 'None'),
	('SusanFaley5', 'Deworming', 3.5, 'None'),
	
	('CarterSmith1', 'Deworming', 2.5, 'None'),
	('CarterSmith2', 'Deworming', 3, 'None'),
	('CarterSmith3', 'Deworming', 4, 'None'),
	('CarterSmith4', 'Deworming', 3, 'None'),
	('CarterSmith5', 'Deworming', 3.5, 'None')
GO



print '' print '*** creating puppy table'
GO
CREATE TABLE [dbo].[Puppy] (
	[PuppyID]			[nvarchar](50)		NOT NULL,
	[BreedID]			[nvarchar](50)		NOT NULL,
	[LitterID]			[nvarchar](50)		NOT NULL,
	[MedicalRecordID]	[nvarchar](50)		NULL,
	[Image]				[nvarchar](30)		NOT NULL,
	[Gender]			[nvarchar](30)		NOT NULL,
	[Adopted]			[bit]				NOT NULL,
	[Microchip]			[bit]				NOT NULL DEFAULT 0,
	[Price]				[DECIMAL](9,2)		NOT NULL DEFAULT 800.00,
	
	CONSTRAINT	[pk_puppyid]	PRIMARY KEY([PuppyID] ASC),
	CONSTRAINT 	[fk_breedid3] 	FOREIGN KEY([BreedID])
		REFERENCES [Breed]([BreedID]),
	CONSTRAINT 	[fk_litterid] 	FOREIGN KEY([LitterID])
		REFERENCES [Litter]([LitterID]),
	CONSTRAINT 	[fk_medicalrecordid] 	FOREIGN KEY([MedicalRecordID])
		REFERENCES [MedicalRecord]([MedicalRecordID])
)
GO
print '' print '*** inserting puppy test records'
GO
INSERT INTO [dbo].[Puppy] 
		([PuppyID], [BreedID], [LitterID], [MedicalRecordID], [Image], [Gender], [Adopted], [Microchip], [Price])
	VALUES
	('ALit1One', 'Mini Aussiedoodle', 'AussieLit1', 'LukeWacker1', 'testTwizzler.jpg', 'Female', 1, 1, 800.00),
	('CLit1One', 'Cockapoo', 'CockerLit1', 'LukeWacker2', 'testCassie.jpg', 'Female', 1, 1, 900),
	('GLit1One', 'Mini Goldendoodle', 'GoldenLit1', 'LukeWacker3', 'testLuna.jpg', 'Female', 1, 1, 100),
	
	-- aussie litter 2--
	('ALit2One', 'Mini Aussiedoodle', 'AussieLit2', 'AmyMeyer1', 'aLit2One.jpg', 'Male', 0, 1, 800.00),
	('ALit2Two', 'Mini Aussiedoodle', 'AussieLit2', 'AmyMeyer2', 'aLit2Two.jpg', 'Female', 0, 1, 820.00),
	('ALit2Three', 'Mini Aussiedoodle', 'AussieLit2', 'AmyMeyer3', 'aLit2Three.jpg', 'Male', 0, 1, 850.00),
	('ALit2Four', 'Mini Aussiedoodle', 'AussieLit2', 'AmyMeyer4', 'aLit2Four.jpg', 'Female', 0, 1, 800.00),
	
		-- aussie litter 3--
	('ALit3One', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer1', 'aLit3One.png', 'Female', 0, 1, 950.00),
	('ALit3Two', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer2', 'aLit3Two.png', 'Female', 0, 1, 920.00),
	('ALit3Three', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer3', 'aLit3Three.png', 'Male', 0, 1, 850.00),
	('ALit3Four', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer4', 'aLit3Four.png', 'Female', 0, 1, 900.00),
	('ALit3Five', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer4', 'aLit3Five.png', 'Male', 0, 1, 840.00),
	('ALit3Six', 'Mini Aussiedoodle', 'AussieLit3', 'AmyMeyer4', 'aLit3Six.png', 'Male', 0, 1, 880.00),
	
	-- Cocker litter 2--
	('CLit2One', 'Cockapoo', 'CockerLit2', 'SusanFaley1','cLit2One.jpg', 'Female', 0, 1, 900.00),
	('CLit2Two', 'Cockapoo', 'CockerLit2', 'SusanFaley2','cLit2Two.jpg', 'Male', 0, 1, 800.00),
	('CLit2Three', 'Cockapoo', 'CockerLit2', 'SusanFaley3', 'cLit2Three.jpg', 'Female', 0, 1, 850.00),
	('CLit2Four', 'Cockapoo', 'CockerLit2','SusanFaley4', 'cLit2Four.jpg',  'Male', 0, 1, 820.00),
	('CLit2Five', 'Cockapoo', 'CockerLit2','SusanFaley5', 'cLit2Five.jpg', 'Female', 0, 1, 810.00),
	
	-- Cocker litter 3--
	('CLit3One', 'Cockapoo', 'CockerLit3', 'SusanFaley1','cLit3One.png', 'Male', 0, 1, 800.00),
	('CLit3Two', 'Cockapoo', 'CockerLit3', 'SusanFaley2','cLit3Two.png', 'Female', 0, 1, 900.00),
	('CLit3Three', 'Cockapoo', 'CockerLit3', 'SusanFaley3', 'cLit3Three.png', 'Female', 0, 1, 850.00),
	('CLit3Four', 'Cockapoo', 'CockerLit3','SusanFaley4', 'cLit3Four.png',  'Male', 0, 1, 820.00),	
	
	-- golden litter 2--
	('GLit2One', 'Mini Goldendoodle', 'GoldenLit2', 'CarterSmith1', 'gLit2One.jpg', 'Female', 0, 1, 850.00),
	('GLit2Two', 'Mini Goldendoodle', 'GoldenLit2', 'CarterSmith2', 'gLit2Two.jpg', 'Male', 0, 1, 800.00),
	('GLit2Three', 'Mini Goldendoodle', 'GoldenLit2', 'CarterSmith3','gLit2Three.jpg', 'Female', 0, 1, 820.00),
	('GLit2Four', 'Mini Goldendoodle', 'GoldenLit2', 'CarterSmith4','gLit2Four.jpg', 'Male', 0, 1, 800.00),
	('GLit2Five', 'Mini Goldendoodle', 'GoldenLit2', 'CarterSmith5', 'gLit2Five.jpg', 'Female', 0, 1, 800.00),
	
	-- golden litter 3--
	('GLit3One', 'Mini Goldendoodle', 'GoldenLit3', 'CarterSmith1', 'gLit3One.png', 'Male', 0, 1, 850.00),
	('GLit3Two', 'Mini Goldendoodle', 'GoldenLit3', 'CarterSmith2', 'gLit3Two.png', 'Female', 0, 1, 900.00),
	('GLit3Three', 'Mini Goldendoodle', 'GoldenLit3', 'CarterSmith3','gLit3Three.png', 'Male', 0, 1, 820.00),
	('GLit3Four', 'Mini Goldendoodle', 'GoldenLit3', 'CarterSmith4','gLit3Four.png', 'Female', 0, 1, 890.00)
GO


print '' print '*** creating puppy application table'
GO
CREATE TABLE [dbo].[Application] (
	[ApplicationID]		[int]IDENTITY(1000000, 1)				NOT NULL,
	[UserID]			[int]				NOT NULL,
	[FullName]			[nvarchar](50)		NOT NULL,
	[Age]				[int]				NOT NULL,
	[Renting]			[bit]				NOT NULL DEFAULT 0,
	[Yard]				[bit]				NOT NULL DEFAULT 0,
	[DesiredBreed]		[nvarchar](50)		NOT NULL,
	[DesiredGender]		[nvarchar](50)		NOT NULL,
	[PreferredContact]	[nvarchar](50)		NOT NULL,
	[Status]			[bit]				NOT NULL DEFAULT 0,
	[Comment]			[nvarchar](400)		NULL,
	
	CONSTRAINT	[pk_applicationid]	PRIMARY KEY([ApplicationID] ASC),
	CONSTRAINT [fk_userid] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID])
)
GO
print '' print '*** inserting application test records'
GO
INSERT INTO [dbo].[Application] 
		([UserID],[FullName], [Age], [Renting], [Yard], [DesiredBreed], [DesiredGender], [PreferredContact], [Comment])
	VALUES
	(1000001, 'Livie Wacker', 20, 1, 0, 'Aussiedoodle', 'Female', 'Email', ''),
	(1000002, 'Avena Williams', 20, 1, 1, 'Cockapoo', 'Male', 'Text', ''),
	(1000003, 'Sprig Plantar', 20, 0, 1, 'Goldendoodle', 'Female', 'Phone Call', '')
GO


print '' print '*** creating adoption table'
GO
CREATE TABLE [dbo].[Adoption] (
	[AdoptionID]	[int]IDENTITY(1000000, 1) NOT NULL,
	[ApplicationID]	[int]				NOT NULL,
	[PuppyID]		[nvarchar](50)		NOT NULL,
	[UserID]		[int]				NOT NULL,
	[Status]		[nvarchar](50)		NOT NULL,
	
	CONSTRAINT	[pk_adoptionid]	PRIMARY KEY([AdoptionID] ASC),
	CONSTRAINT [fk_puppyid] FOREIGN KEY([PuppyID])
		REFERENCES [Puppy]([PuppyID]),
	CONSTRAINT 	[fk_userid2] FOREIGN KEY([UserID])
		REFERENCES [User]([UserID]),
	CONSTRAINT 	[fk_applicationid] FOREIGN KEY([ApplicationID])
		REFERENCES [Application]([ApplicationID])
)
GO
print '' print '*** inserting adoption test records'
GO
INSERT INTO [dbo].[Adoption] 
		([ApplicationID],[PuppyID], [UserID], [Status])
	VALUES
	(1000000, 'ALit1One', 1000003, 'In Progress'),
	(1000001, 'CLit1One', 1000004, 'Adopted'),
	(1000002, 'GLit1One', 1000002, 'Adopted')
GO

print '' print '*** creating testimonial table'
GO
CREATE TABLE [dbo].[Testimonial] (
	[TestimonialID]		[nvarchar](50)		NOT NULL,
	[AdoptionId]		[int]				NOT NULL,
	[Image]				[nvarchar](30)		NULL,
	[Details]			[nvarchar](250)		NULL,
	[Rating]			[int]				NULL,
	
	CONSTRAINT	[pk_testimonialid]	PRIMARY KEY([TestimonialID] ASC),
	CONSTRAINT 	[fk_adoptionid] 	FOREIGN KEY([AdoptionID])
		REFERENCES [Adoption]([AdoptionID])
)
GO
print '' print '*** inserting testimonial test records'
GO
INSERT INTO [dbo].[Testimonial] 
		([TestimonialID],[AdoptionID], [Image], [Details], [Rating])
	VALUES
	('Wacker Family Testimonial', 1000000, 'testTwizzler.jpg', 'Twizzler is an amazing Aussiedoodle puppy! She is so smart and loves to learn new tricks and she is very sweet with our young children!', 5),
	('Sanner Family Testimonial', 1000001, 'testCassie.jpg', 'We love our cockapoo puppy Cassie! She is so sweet and always loves to cuddle! We would be happy to adopt from Doodle Dogs again!', 5),
	('Meyer Family Testimonial', 1000002, 'testLuna.jpg', 'Luna is the sweetest dog ever! Our kids love playing fetch with her every day! She has boundless energy but also loves to cuddle. If you are thinking about getting a goldendoodle, you definately should!', 5)
GO

/*stored procedures*/

/* User and UserRole related SP*/

print '' print '*** creating procedure sp_authenticate_user'
GO
CREATE PROCEDURE [dbo].[sp_authenticate_user]
	(
		@Email			[nvarchar](250),
		@PasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		SELECT COUNT([UserID])
		FROM	[User]
		WHERE 	[Email] = @Email
			AND	[PasswordHash] = @PasswordHash
			AND [Active] = 1
	
	END
GO


print '' print '*** creating procedure sp_select_user_by_email'
GO
CREATE PROCEDURE [dbo].[sp_select_user_by_email]
	(
		@Email			[nvarchar](250)
	)
AS
	BEGIN
		SELECT [UserID], [GivenName], [FamilyName], [PhoneNumber],
		[Email],[Active]		
		FROM	[User]
		WHERE	[Email] = @Email
		       
	END
GO



print '' print '*** creating procedure sp_select_roles_by_UserID'
GO
CREATE PROCEDURE [dbo].[sp_select_roles_by_UserID]
	(
		@UserID			[int]
	)
AS
	BEGIN
		SELECT [UserID], [UserRoleID]
		FROM	[UserRole]
		WHERE	[UserID] = @UserID
	END
GO



print '' print '*** creating procedure sp_update_user_role_by_userid'
GO
CREATE PROCEDURE [dbo].[sp_update_user_role_by_userid]
	(
		@UserID				[int],
		@NewUserRoleID		[nvarchar](50),
		@OldUserRoleID		[nvarchar](50)
	)
AS
	BEGIN
		UPDATE  [UserRole]
		SET		[UserRoleID] = @NewUserRoleID
		WHERE	[UserRoleID] = @OldUserRoleID
			AND	[UserID] = @UserID
			
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating procedure sp_select_all_user_roles'
GO
CREATE PROCEDURE [dbo].[sp_select_all_user_roles]
AS
	BEGIN
		SELECT [UserID], [UserRoleID], [Description]
		FROM		[UserRole] 
	END
GO




print '' print '*** creating procedure sp_update_passwordhash_by_email'
GO
CREATE PROCEDURE [dbo].[sp_update_passwordhash_by_email]
	(
		@Email				[nvarchar](250),
		@NewPasswordHash	[nvarchar](100),
		@OldPasswordHash	[nvarchar](100)
	)
AS
	BEGIN
		UPDATE  [User]
		SET		[PasswordHash] = @NewPasswordHash
		WHERE	[PasswordHash] = @OldPasswordHash
			AND	[Email] = @Email
			
		RETURN @@ROWCOUNT
	END
GO


print '' print '*** creating procedure sp_select_all_users'
GO
CREATE PROCEDURE [dbo].[sp_select_all_users]
AS
	BEGIN
		SELECT [UserID], [GivenName], [FamilyName], [PhoneNumber],
		[Email],[Active]
		FROM		[User] 
		ORDER BY	[FamilyName]
	END
GO

print '' print '*** creating procedure sp_update_user_email'
GO 
CREATE PROCEDURE [dbo].[sp_update_user_email]
	(
		@NewEmail			[nvarchar](250),
		@OldEmail			[nvarchar](250),
		@PasswordHash		[nvarchar](100)
	)
AS
	BEGIN
		UPDATE 			[User]
		SET				[Email] = @NewEmail
		WHERE			[Email] = @OldEmail
			AND 		[PasswordHash] = @PasswordHash
		RETURN @@ROWCOUNT
	END
GO



print '' print '*** creating procedure sp_update_user_by_userid'
GO
CREATE PROCEDURE [dbo].[sp_update_user_by_userid]
	(
		@UserID				[int],
		@GivenName			[nvarchar](50),
		@FamilyName			[nvarchar](100),
		@PhoneNumber		[nvarchar](11)
	)
AS
	BEGIN
		UPDATE 			[User]
		SET				[GivenName] = @GivenName,
						[FamilyName] = @FamilyName,
						[PhoneNumber] = @PhoneNumber
						
		WHERE			[UserID] = @UserID
		
		RETURN @@ROWCOUNT
	END
GO

print '' print '*** creating sp_insert_user'
GO
CREATE PROCEDURE [dbo].[sp_insert_user]
	(
		@GivenName			[nvarchar](50),
		@FamilyName			[nvarchar](100),
		@PhoneNumber		[nvarchar](11),
		@Email				[nvarchar](250)
	)
AS
	BEGIN
		INSERT INTO [dbo].[User]
			([GivenName], [FamilyName], [PhoneNumber], [Email])
		VALUES	
			(@GivenName, @FamilyName, @PhoneNumber, @Email)
			
		SELECT SCOPE_IDENTITY() AS UserID;
	END
GO       

		
print '' print '*** creating procedure sp_select_all_users_by_active'
GO
CREATE PROCEDURE [dbo].[sp_select_all_users_by_active]
	(
		@Active		[bit]
	)
AS
	BEGIN
		SELECT [UserID], [GivenName], [FamilyName], [PhoneNumber],
				[Email],[Active]
		FROM	[User]  
		WHERE	[Active] = @Active
	END
GO


print '' print '*** creating procedure sp_update_application_status'
GO 
CREATE PROCEDURE [dbo].[sp_update_application_status]
	(
		@ApplicationID					[int],
		@OldStatus 	[bit],
		@NewStatus	[bit]
	)
AS
	BEGIN
		UPDATE 			[Application]
		SET				[Status] = @NewStatus
		WHERE			[Status] = @OldStatus
			AND 		[ApplicationID] = @ApplicationID
		RETURN @@ROWCOUNT
	END
GO    

/* Mother and father dog related SP*/


print '' print '*** creating procedure sp_select_mother_dog_by_motherDogID'
GO
CREATE PROCEDURE [dbo].[sp_select_mother_dog_by_motherDogID]
	(
		@MotherDogID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [MotherDogID],[BreedID], [Personality], [EnergyLevel], 
		[BarkingLevel], [Trainability], [Image], [Description]		
		FROM	[MotherDog]  
		WHERE	[MotherDogID] = @MotherDogID
	END
GO

print '' print '*** creating procedure sp_select_all_mother_dogs'
GO
CREATE PROCEDURE [dbo].[sp_select_all_mother_dogs]
AS
	BEGIN
		SELECT [MotherDogID],[BreedID], [Personality], [EnergyLevel], 
		[BarkingLevel], [Trainability], [Image], [Description]		
		FROM	[MotherDog]  
	END
GO



print '' print '*** creating procedure sp_select_father_dog_by_fatherDogID'
GO
CREATE PROCEDURE [dbo].[sp_select_father_dog_by_fatherDogID]
	(
		@FatherDogID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [FatherDogID],[BreedID], [Personality], [EnergyLevel], 
		[BarkingLevel], [Trainability], [Image], [Description]		
		FROM	[FatherDog]  
		WHERE	[FatherDogID] = @FatherDogID
	END
GO


print '' print '*** creating procedure sp_select_all_father_dogs'
GO
CREATE PROCEDURE [dbo].[sp_select_all_father_dogs]
AS
	BEGIN
		SELECT [FatherDogID],[BreedID], [Personality], [EnergyLevel], 
		[BarkingLevel], [Trainability], [Image], [Description]		
		FROM	[FatherDog]  
	END
GO


/* Breed related SP*/

print '' print '*** creating procedure sp_select_breed_by_BreedID'
GO
CREATE PROCEDURE [dbo].[sp_select_breed_by_breedID]
	(
		@BreedID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [BreedID], [Size], [Image], [Hypoallergenic], [LifeExpectancy], 
		[GoodDogs], [GoodKids], [Description]		
		FROM	[Breed]  
		WHERE	[BreedID] = @BreedID
	END
GO

print '' print '*** creating procedure sp_select_all_breeds'
GO
CREATE PROCEDURE [dbo].[sp_select_all_breeds]
AS
	BEGIN
		SELECT [BreedID], [Size], [Image], [Hypoallergenic], [LifeExpectancy], 
		[GoodDogs], [GoodKids], [Description]
		FROM [Breed]  
	END
GO 

/* Testimonial related SP*/

print '' print '*** creating procedure sp_select_testimonial_by_testimonialID'
GO
CREATE PROCEDURE [dbo].[sp_select_testimonial_by_testimonialID]
	(
		@TestimonialID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [TestimonialID],	[AdoptionID], [Image], [Details], [Rating]
		FROM	[Testimonial]  
		WHERE	[TestimonialID] = @TestimonialID
	END
GO

print '' print '*** creating procedure sp_select_all_testimonials'
GO
CREATE PROCEDURE [dbo].[sp_select_all_testimonials]
AS
	BEGIN
		SELECT [TestimonialID],	[AdoptionID], [Image], [Details], [Rating]
		FROM [Testimonial]
	END
GO 

/* Litter related SP*/

print '' print '*** creating procedure sp_select_litter_by_litterID'
GO
CREATE PROCEDURE [dbo].[sp_select_litter_by_litterID]
	(
		@LitterID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [LitterID],	[FatherDogID], [MotherDogID], [Image], [DateOfBirth], [GoHomeDate], [NumberPuppies]
		FROM	[Litter]  
		WHERE	[LitterID] = @LitterID
	END
GO


print '' print '*** creating procedure sp_select_all_litters'
GO
CREATE PROCEDURE [dbo].[sp_select_all_litters]
AS
	BEGIN
		SELECT [LitterID],	[FatherDogID], [MotherDogID], [Image], [DateOfBirth], [GoHomeDate], [NumberPuppies]
		FROM	[Litter]   
	END
GO

PRINT ''
PRINT '*** creating procedure sp_update_litter_by_litterID'
GO
CREATE PROCEDURE [dbo].[sp_update_litter_by_litterID]
(
		@LitterID            [nvarchar](50) ,
        @FatherDogID         [nvarchar](50),
        @MotherDogID         [nvarchar](50),
        @Image               [nvarchar](30),
        @DateOfBirth         DATE,
        @GoHomeDate          DATE,
        @NumberPuppies       int
)
AS
BEGIN
    UPDATE [Litter]
    SET 
        	[LitterID]  =  @LitterID      ,  
			[FatherDogID] =  @FatherDogID   , 
			[MotherDogID] = @MotherDogID   , 
			[Image]       = @Image         , 
			[DateOfBirth] = @DateOfBirth   , 
			[GoHomeDate] = @GoHomeDate    , 
			[NumberPuppies] = @NumberPuppies  	
    WHERE 
        [LitterID] = @LitterID;
    RETURN @@ROWCOUNT;
END
GO


PRINT '*** Create Stored Procedure sp_insert_litter ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_litter]
    (
        @LitterID            [nvarchar](50) ,
        @FatherDogID         [nvarchar](50),
        @MotherDogID         [nvarchar](50),
        @Image               [nvarchar](30),
        @DateOfBirth         DATE,
        @GoHomeDate          DATE,
        @NumberPuppies       int
    )
AS
BEGIN
    INSERT INTO Litter (
	[LitterID]    ,  
	[FatherDogID]  , 
	[MotherDogID]  , 
	[Image]        , 
	[DateOfBirth]  , 
	[GoHomeDate]   , 
	[NumberPuppies] 	
)
    VALUES (    
	@LitterID         ,
	@FatherDogID      ,
	@MotherDogID      ,
	@Image            ,
	@DateOfBirth      ,
	@GoHomeDate       ,
	@NumberPuppies  
)
    RETURN @@ROWCOUNT
END
GO

print '' print '*** Create Stored Procedure sp_delete_litter_by_litterID ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_litter_by_litterID]
	(
		@LitterID		[nvarchar](50)
	)
AS
	BEGIN
		DELETE FROM litter
		WHERE LitterID = @LitterID;
		RETURN @@ROWCOUNT
	END
GO


/* Puppy related SP*/

print '' print '*** creating procedure sp_select_puppy_by_litterID'
GO
CREATE PROCEDURE [dbo].[sp_select_puppies_by_litterID]
	(
		@LitterID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [PuppyID], [BreedID], [LitterID], [MedicalRecordID], [Image], [Gender], [Adopted], [Microchip], [Price]
		FROM	[Puppy]  
		WHERE	[LitterID] = @LitterID
	END
GO

print '' print '*** creating procedure sp_select_puppy_by_breedID'
GO
CREATE PROCEDURE [dbo].[sp_select_puppies_by_breedID]
	(
		@BreedID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [PuppyID], [BreedID], [LitterID], [MedicalRecordID], [Image], [Gender], [Adopted], [Microchip], [Price]
		FROM	[Puppy]  
		WHERE	[BreedID] = @BreedID
	END
GO

print '' print '*** creating procedure sp_update_puppy'
GO 
CREATE PROCEDURE [dbo].[sp_update_puppy]
	(
		@PuppyID				[nvarchar](50),
		@OldAdopted 			[bit],
		@NewAdopted				[bit]
	)
AS
	BEGIN
		UPDATE 			[Puppy]
		SET				[Adopted] = @NewAdopted
		WHERE			[PuppyID] = @PuppyID
		RETURN @@ROWCOUNT
	END
GO  

print '' print '*** Create Stored Procedure sp_delete_puppy_by_puppyID ***'
GO
CREATE PROCEDURE [dbo].[sp_delete_puppy_by_puppyID]
	(
		@PuppyID		[nvarchar](50)
	)
AS
	BEGIN
		DELETE FROM puppy
		WHERE PuppyID = @PuppyID;
		RETURN @@ROWCOUNT
	END
GO

PRINT '' PRINT '*** creating procedure sp_update_puppy_by_puppyid'
GO

CREATE PROCEDURE [dbo].[sp_update_puppy_by_puppyid]
(
    @PuppyID            [nvarchar](50) ,
    @BreedID             [nvarchar](50),
    @LitterID            [nvarchar](50),
    @MedicalRecordID     [nvarchar](50),
    @Image               [nvarchar](30),
    @Gender              [nvarchar](30),
    @Adopted            [bit],
    @Microchip          [bit],
    @Price              [DECIMAL](9,2)
)
AS
BEGIN
    UPDATE [Puppy]
    SET 
        [BreedID] = @BreedID,
        [LitterID] = @LitterID,
        [MedicalRecordID] = @MedicalRecordID,
        [Image] = @Image,
        [Gender] = @Gender,
        [Adopted] = @Adopted,
        [Microchip] = @Microchip,
        [Price] = @Price
    WHERE 
        [PuppyID] = @PuppyID;
    RETURN @@ROWCOUNT;
END
GO

PRINT '*** Create Stored Procedure sp_insert_puppy ***'
GO
CREATE PROCEDURE [dbo].[sp_insert_puppy]
    (
        @PuppyID            [nvarchar](50) ,
        @BreedID             [nvarchar](50),
        @LitterID            [nvarchar](50),
        @MedicalRecordID     [nvarchar](50),
        @Image               [nvarchar](30),
        @Gender              [nvarchar](30),
        @Adopted            [bit],
        @Microchip          [bit],
        @Price              [DECIMAL](9,2)
    )
AS
BEGIN
    INSERT INTO Puppy (
	PuppyID  ,       
	BreedID  ,       
	LitterID  ,      
	MedicalRecordID ,
	Image          , 
	Gender          ,
	Adopted         ,
	Microchip       ,
	Price           
)
    VALUES (@PuppyID  ,       
	@BreedID         ,
	@LitterID   ,     
	@MedicalRecordID ,
	@Image      ,     
	@Gender      ,    
	@Adopted    ,     
	@Microchip  ,     
	@Price)

    RETURN @@ROWCOUNT
END
GO


print '' print '*** creating procedure sp_select_puppy_by_puppyID'
GO
CREATE PROCEDURE [dbo].[sp_select_puppy_by_puppyID]
	(
		@PuppyID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [PuppyID], [BreedID], [LitterID], [MedicalRecordID], [Image], [Gender], [Adopted], [Microchip], [Price]
		FROM	[Puppy]  
		WHERE	[PuppyID] = @PuppyID
	END
GO  

print '' print '*** creating procedure sp_select_all_puppies'
GO
CREATE PROCEDURE [dbo].[sp_select_all_puppies]
AS
	BEGIN
		SELECT [PuppyID], [BreedID], [LitterID], [MedicalRecordID], [Image], [Gender], [Adopted], [Microchip], [Price]
		FROM	[Puppy]  
	END
GO

/*Medical Record related SP*/

print '' print '*** creating procedure sp_select_medical_record_by_medicalrecordID'
GO
CREATE PROCEDURE [dbo].[sp_select_medical_record_by_medicalrecordID]
	(
		@MedicalRecordID		[nvarchar](50)
	)
AS
	BEGIN
		SELECT [MedicalRecordID], [Treatments], [Weight], [Issues] 
		FROM	[MedicalRecord]  
		WHERE	[MedicalRecordID] = @MedicalRecordID
	END
GO

/* Applciation related SP*/

print '' print '*** creating procedure sp_select_applicaton_by_userID'
GO
CREATE PROCEDURE [dbo].[sp_select_applications_by_userID]
	(
		@UserID		[int]
	)
AS
	BEGIN
		SELECT [ApplicationID], [UserID], [FullName], [Age], [Renting], [Yard], [DesiredBreed], [DesiredGender], [PreferredContact],[Status],[Comment]
		FROM	[Application]  
		WHERE	[UserID] = @UserID
	END
GO

print '' print '*** creating procedure sp_select_all_applications'
GO
CREATE PROCEDURE [dbo].[sp_select_all_applications]
AS
	BEGIN
		SELECT [ApplicationID],[UserID],[FullName],[Age],[Renting],[Yard],[DesiredBreed],[DesiredGender],[PreferredContact],[Status],[Comment]			
		FROM	[Application]  
	END
GO

print '' print '*** creating sp_insert_application'
GO
CREATE PROCEDURE [dbo].[sp_insert_application]
	(
		@UserID				[int]			,
		@FullName			[nvarchar](50)	,
		@Age				[int]			,
		@Renting			[bit]			,
		@Yard				[bit]			,
		@DesiredBreed		[nvarchar](50)	,
		@DesiredGender		[nvarchar](50)	,
		@PreferredContact	[nvarchar](50)	,
		@Status				[bit] = 0,
		@Comment			[nvarchar](400)	= null
	)
AS
	BEGIN
		INSERT INTO [dbo].[Application]
			([UserID], [FullName], [Age], [Renting], [Yard], [DesiredBreed], [DesiredGender], [PreferredContact],[Status],[Comment])
		VALUES	
			(@UserID, @FullName, @Age, @Renting, @Yard, @DesiredBreed, @DesiredGender, @PreferredContact, @Status, @Comment)
			
		SELECT SCOPE_IDENTITY()
	END
GO  

/* Adoption related SP*/

print '' print '*** creating sp_insert_adoption'
GO
CREATE PROCEDURE [dbo].[sp_insert_adoption]
	(		
		@ApplicationID	[int],			
		@PuppyID		[nvarchar](50)	,
		@UserID			[int]	,		
		@Status			[nvarchar](50)	
	)
AS
	BEGIN
		INSERT INTO [dbo].[Adoption]
			([ApplicationID], [PuppyID], [UserID], [Status])
		VALUES	
			(@ApplicationID, @PuppyID, @UserID, @Status)
			
		SELECT SCOPE_IDENTITY() AS AdoptionID;
	END
GO       

print '' print '*** creating sp_select_all_adoptions'
GO
CREATE PROCEDURE [dbo].[sp_select_all_adoptions]
AS
	BEGIN
		SELECT [AdoptionID], [ApplicationID], [PuppyID], [UserID], [Status]
		FROM [Adoption]
	END
GO 

print '' print '*** creating procedure sp_update_adoption_status_by_id'
GO 
CREATE PROCEDURE [dbo].[sp_update_adoption_status_by_id]
	(
		@AdoptionID				[int],
		@OldStatus	 			[nvarchar](50)	,
		@NewStatus				[nvarchar](50)	
	)
AS
	BEGIN
		UPDATE 			[Adoption]
		SET				[Status] = @NewStatus
		WHERE			[AdoptionID] = @AdoptionID
		RETURN @@ROWCOUNT
	END
GO    
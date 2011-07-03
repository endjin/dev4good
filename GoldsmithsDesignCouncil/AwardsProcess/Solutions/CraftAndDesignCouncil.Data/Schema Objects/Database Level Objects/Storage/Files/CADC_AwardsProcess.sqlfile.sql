ALTER DATABASE [$(DatabaseName)]
    ADD FILE (NAME = [CADC_AwardsProcess], FILENAME = '$(DefaultDataPath)CADC_AwardsProcess.mdf', FILEGROWTH = 1024 KB) TO FILEGROUP [PRIMARY];


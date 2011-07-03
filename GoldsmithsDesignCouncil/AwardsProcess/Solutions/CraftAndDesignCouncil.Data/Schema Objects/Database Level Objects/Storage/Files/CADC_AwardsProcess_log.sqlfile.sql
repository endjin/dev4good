ALTER DATABASE [$(DatabaseName)]
    ADD LOG FILE (NAME = [CADC_AwardsProcess_log], FILENAME = '$(DefaultLogPath)CADC_AwardsProcess_log.ldf', MAXSIZE = 2097152 MB, FILEGROWTH = 10 %);


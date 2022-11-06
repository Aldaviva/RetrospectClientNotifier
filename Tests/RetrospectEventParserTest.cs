using System;
using DataSizeUnits;
using FluentAssertions;
using RetrospectClientNotifier;
using RetrospectClientNotifier.Events;
using Xunit;

namespace Tests;

public class RetrospectEventParserTest {

    [Fact]
    public void startSource() {
        const string    ARGS   = @"retroeventhandler.exe StartSource ""Test"" ""Backup Clients/Aegir/Games (G:)"" ""G:\"" ""Aegir"" ""C:\Programs\Security\Retrospect\intv0006.reh""";
        RetrospectEvent actual = RetrospectEventParser.parseEvent(Notifier.getCommandLineArguments(ARGS));
        actual.Should().BeOfType<StartSourceEvent>();
        StartSourceEvent actualStart = (StartSourceEvent) actual;
        actualStart.scriptName.Should().Be("Test");
        actualStart.clientName.Should().Be("Aegir");
        actualStart.sourceName.Should().Be("Backup Clients/Aegir/Games (G:)");
        actualStart.sourcePath.Should().Be(@"G:\");
        actualStart.interventionFile.Should().Be(@"C:\Programs\Security\Retrospect\intv0006.reh");
    }

    [Fact]
    public void endSource() {
        const string ARGS =
            @"retroeventhandler.exe EndSource ""Test"" ""Backup Clients/Aegir/Games (G:)"" ""G:\"" ""Aegir"" ""624640"" ""1112"" ""0"" ""7/11/2021 12:32"" ""7/11/2021 12:32"" ""7/11/2021 12:32"" ""Test"" ""Normal"" ""Aegir"" ""0"" ""0"" ""successful"" ""true""";
        RetrospectEvent actual = RetrospectEventParser.parseEvent(Notifier.getCommandLineArguments(ARGS));
        actual.Should().BeOfType<EndSourceEvent>();
        EndSourceEvent actualEnd = (EndSourceEvent) actual;
        actualEnd.scriptName.Should().Be("Test");
        actualEnd.clientName.Should().Be("Aegir");
        actualEnd.sourceName.Should().Be("Backup Clients/Aegir/Games (G:)");
        actualEnd.sourcePath.Should().Be(@"G:\");
        actualEnd.backupAction.Should().Be("Normal");
        actualEnd.backupSet.Should().Be("Test");
        actualEnd.duration.Should().Be(TimeSpan.Zero);
        actualEnd.errorCount.Should().Be(0);
        actualEnd.fatalErrorCode.Should().Be(0);
        actualEnd.filesBackedUp.Should().Be(1112);
        actualEnd.parentVolume.Should().Be("Aegir");
        actualEnd.scriptStart.Should().Be(new DateTime(2021, 7, 11, 12, 32, 0));
        actualEnd.sourceStart.Should().Be(new DateTime(2021, 7, 11, 12, 32, 0));
        actualEnd.sourceEnd.Should().Be(new DateTime(2021, 7, 11, 12, 32, 0));
        actualEnd.sizeBackedUp.Should().Be(new DataSize(624640, Unit.Kilobyte));
    }

}
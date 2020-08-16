using System;
using System.Linq;
using CommandApi.Controllers;
using CommandApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CommandApi.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        DbContextOptionsBuilder<CommandContext> optionsBuilder;
        CommandContext dbContext;
        CommandsController commandsController;

        public CommandsControllerTests()
        {
            optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("UnitTestInMemoryDB");
            dbContext = new CommandContext(optionsBuilder.Options);

            commandsController = new CommandsController(dbContext);
        }

        public void Dispose()
        {
            optionsBuilder = null;
            foreach (var cmd in dbContext.CommandItems)
            {
                dbContext.CommandItems.Remove(cmd);
            }
            dbContext.SaveChanges();
            dbContext.Dispose();
            commandsController = null;
        }

        [Fact]
        public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //Arrange
            //Act
            var result = commandsController.GetCommandItems();

            //Assert
            Assert.Empty(result.Value);
        }

        [Fact]
        public void GetCommandItems_ReturnsOneItem_WhenDBHasOneObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            //Act
            var result = commandsController.GetCommandItems();

            //Assert
            Assert.Single(result.Value);
        }

        [Fact]
        public void GetCommandItems_ReturnNItems_WhenDBHasNObjects()
        {
            //Arrange
            var command1 = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            var command2 = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            dbContext.CommandItems.Add(command1);
            dbContext.CommandItems.Add(command2);
            dbContext.SaveChanges();

            //Act
            var result = commandsController.GetCommandItems();

            //Assert
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public void GetCommandItems_ReturnsTheCorrectType()
        {
            //Arrange

            //Act
            var result = commandsController.GetCommandItems();

            //Assert
            Assert.IsType<ActionResult<IQueryable<Command>>>(result);
        }

        [Fact]
        public void GetCommadnItem_ReturnsNullResult_WhenInvalidId()
        {
            //Arrange
            //DB should be empty. any Id will be invalid

            //Act
            var result = commandsController.GetCommandItem(0);

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public void GetCommandItem_Returns404NotFound_WhenInvalidId()
        {
            //Arrange
            //DB should be empty. any Id will be invalid

            //Act
            var result = commandsController.GetCommandItem(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommandItem_ReturnsTheCorrectType()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;

            //Act
            var result = commandsController.GetCommandItem(cmdId);

            //Assert
            Assert.IsType<ActionResult<Command>>(result);
        }

        [Fact]
        public void GetCommandItem_ReturnsTheCorrectResource()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;

            //Act
            var result = commandsController.GetCommandItem(cmdId);

            //Assert
            Assert.Equal(cmdId, result.Value.Id);
        }

        [Fact]
        public void PostCommandItem_ObjectCountIncrement_WhenValidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            var oldCount = dbContext.CommandItems.Count();

            //Act
            var result = commandsController.PostCommandItem(command);

            //Assert
            Assert.Equal(oldCount + 1, dbContext.CommandItems.Count());
        }

        [Fact]
        public void PostCommandItem_Returns201Created_WhenValidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };

            //Act
            var result = commandsController.PostCommandItem(command);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public void PutCommandItem_AttributeUpdated_WhenValidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            command.HowTo = "UPDATED";

            //Act
            commandsController.PutCommandItem(cmdId, command);
            var result = dbContext.CommandItems.Find(cmdId);

            //Assert
            Assert.Equal(command.HowTo, result.HowTo);
        }

        [Fact]
        public void PutCommandItem_Returns204_WhenValidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            command.HowTo = "UPDATED";

            //Act
            var result = commandsController.PutCommandItem(cmdId, command);

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void PutCommandItem_Returns400_WhenInvalidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id + 1;
            command.HowTo = "UPDATED";

            //Act
            var result = commandsController.PutCommandItem(cmdId, command);
            //Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void PutCommandItem_AttributeUnchanged_WhenInvalidObject()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var command2 = new Command
            {
                Id = command.Id,
                HowTo = "UPDATED",
                Platform = "UPDATED",
                CommandLine = "UPDATED"
            };

            //Act
            commandsController.PutCommandItem(command.Id + 1, command2);
            var result = dbContext.CommandItems.Find(command.Id);

            //Assert
            Assert.Equal(command.HowTo, result.HowTo);
        }

        [Fact]
        public void DeleteCommandItem_ObjectsDecrement_WhenValidObjectId()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();
            var cmdId = command.Id;
            var objCount = dbContext.CommandItems.Count();

            //Act
            commandsController.DeleteCommandItem(cmdId);

            //Assert
            Assert.Equal(objCount - 1, dbContext.CommandItems.Count());
        }

        [Fact]
        public void DeleteCommandItem_Returns200OK_WhenValidObjectId()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;

            //Act
            var result = commandsController.DeleteCommandItem(cmdId);

            //Assert
            Assert.Null(result.Result);
        }

        [Fact]
        public void DeleteCommandItem_Returns404NotFound_WhenInvalidObjectId()
        {
            //Arrange
            //Act
            var result = commandsController.DeleteCommandItem(-1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void DeleteCommandItem_ObjectCountNotDecremented_WhenInvalidObjectId()
        {
            //Arrange
            var command = new Command
            {
                HowTo = "Do Something",
                Platform = "Some Platform",
                CommandLine = "Some CommandLine"
            };
            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            var cmdId = command.Id;
            var objCount = dbContext.CommandItems.Count();

            //Act
            var result = commandsController.DeleteCommandItem(cmdId + 1);

            //Assert
            Assert.Equal(objCount, dbContext.CommandItems.Count());
        }
    }
}

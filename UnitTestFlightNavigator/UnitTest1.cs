using Castle.Core.Logging;
using FlightNavigatorApi.BusinessLogic;
using FlightNavigatorApi.DAL;
using FlightNavigatorApi.Hubs;
using FlightNavigatorApi.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;
using System.Data;

namespace UnitTestFlightNavigator
{
    [TestClass]
    public class FlightsLogicTests
    {
        private FlightsLogic? _FlightsLogic;
        private Mock<ILogger<FlightsLogic>> _loggerMock;


        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<FlightsLogic>>();
            // Create an instance of the class under test (FlightManager)
            _FlightsLogic = new FlightsLogic(_loggerMock.Object);

            //_loggerMock.Setup(logger => logger.Log(LogLevel.Debug, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));

        }

        [TestMethod]
        public void HandleLegOneTwo_NoActiveFlights()
        {
            //arrange
            var flight1 = new Flight { FlightNumber = "ABC123", Leg = Leg.LegFinshed, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal"};
            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.LegFinshed, new List<Flight>() }
            };
            allActiveFlights[Leg.LegFinshed].Add(flight1);


            // Act
            _FlightsLogic!.HandleLegOneTwo(allActiveFlights,Leg.Leg1,Leg.Leg2 );

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 1);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.LegFinshed) == true);
            Assert.IsTrue(allActiveFlights[Leg.LegFinshed].Count == 1);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg1) == false);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg2) == false);
        }
        [TestMethod]
        public void HandleLegOneTwo_WithOneFlightInLeg1()
        {
            //arrange
            var flight1 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg1, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg1, new List<Flight>() }
            };
            allActiveFlights[Leg.Leg1].Add(flight1);


            // Act
            _FlightsLogic!.HandleLegOneTwo(allActiveFlights, Leg.Leg1, Leg.Leg2);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count ==2);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.LegFinshed) == false);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg1) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg1].Count == 0);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg2) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg2].Count == 1);

        }
        [TestMethod]
        public void HandleLegOneTwo_WithOneFlightInLeg2AndNoFlightInLeg3()
        {
            //arrange
            var flight1 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg2, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg2, new List<Flight>() }
            };
            allActiveFlights[Leg.Leg2].Add(flight1);


            // Act
            _FlightsLogic!.HandleLegOneTwo(allActiveFlights, Leg.Leg2, Leg.Leg3);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 2);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg2) == true);          
            Assert.IsTrue(allActiveFlights[Leg.Leg2].Count == 0);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg3) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg3].Count == 1);
        }

        [TestMethod]
        public void HandleLegSixSeven_LegEightOccupied()
        {
            //arrange
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight8 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg8, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };

            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg6, new List<Flight>() },
                { Leg.Leg8, new List<Flight>() }

            };
            allActiveFlights[Leg.Leg6].Add(flight6);
            allActiveFlights[Leg.Leg8].Add(flight8);

            // Act
            _FlightsLogic!.HandleLegSixSeven(allActiveFlights, Leg.Leg6);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 2);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg8) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg8].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 1);
        }



        [TestMethod]
        public void HandleLegSixSeven_LegEightFree()
        {
            //arrange
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
           

            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg6, new List<Flight>() },
            };

            allActiveFlights[Leg.Leg6].Add(flight6);
           // allActiveFlights[Leg.Leg8].Add(flight6);


            // Act
            _FlightsLogic!.HandleLegSixSeven(allActiveFlights,Leg.Leg6);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 2);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg8) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg8].Count == 1);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 0);

        }


        [TestMethod]
        public void HandleLegfive_LegCheckingIfTestingFiveMovesToSixOrSeven()
        {
            //arrange
            var flight5 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg5, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight7 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg7, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg6, new List<Flight>() },
                { Leg.Leg5, new List<Flight>()},
                { Leg.Leg7, new List<Flight>()}
            };

            allActiveFlights[Leg.Leg5].Add(flight5);
             allActiveFlights[Leg.Leg6].Add(flight6);
            allActiveFlights[Leg.Leg7].Add(flight7);


            // Act
            _FlightsLogic!.HandleLegFive(allActiveFlights);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg5) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg7) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg5].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg7].Count == 1);
        }


        [TestMethod]
        public void HandleLegfour_CheckingIfLeg3OrLeg8PassToLeg4()
        {
            //arrange
            var flight3 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg3, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight8 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg8, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight4 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg4, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg3, new List<Flight>() },
                { Leg.Leg8, new List<Flight>()},
                { Leg.Leg4, new List<Flight>()}
            };

            allActiveFlights[Leg.Leg3].Add(flight3);
            allActiveFlights[Leg.Leg8].Add(flight8);
            allActiveFlights[Leg.Leg4].Add(flight4);


            // Act
            _FlightsLogic!.HandleLegFour(allActiveFlights);

            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 4);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg4) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg8) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg3) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg8].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg9].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg4].Count == 0);
        }

        [TestMethod]
        public void HandleLegfour_Leg3OrLeg8PassToLeg4()
        {
            //arrange
            var flight3 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg3, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight8 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg8, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg3, new List<Flight>() },
                { Leg.Leg8, new List<Flight>()},
                { Leg.Leg4, new List<Flight>()}
            };

            allActiveFlights[Leg.Leg3].Add(flight3);
            allActiveFlights[Leg.Leg8].Add(flight8);


            // Act  HandleLegThree
            _FlightsLogic!.HandleLegFour(allActiveFlights);
            _FlightsLogic!.HandleLegThree(allActiveFlights);


            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsFalse(allActiveFlights.ContainsKey(Leg.Leg4) == false);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg8) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg3) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg8].Count == 1);
            //Assert.IsTrue(allActiveFlights[Leg.Leg9].Count == 0);
            Assert.IsTrue(allActiveFlights[Leg.Leg4].Count == 1);
        }


        [TestMethod]
        public void HandleLegfour_IfLeg8PassToLeg4()
        {
            //arrange
            var flight3 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg3, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight8 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg8, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg3, new List<Flight>() },
                { Leg.Leg8, new List<Flight>()},
            };

            allActiveFlights[Leg.Leg3].Add(flight3);
            allActiveFlights[Leg.Leg8].Add(flight8);


            // Act  HandleLegThree
            //_FlightsLogic!.HandleLegFour(allActiveFlights);
            _FlightsLogic!.HandleLegEight(allActiveFlights);


            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsFalse(allActiveFlights.ContainsKey(Leg.Leg4) == false);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg8) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg3) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg8].Count == 0);
            Assert.IsTrue(allActiveFlights[Leg.Leg4].Count == 1);
        }


         [TestMethod]
        public void IfLegFiveTransfersTheFlightToLeg6OrSevenIfItFree()
        {
            //arrange
            var flight5 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg5, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight7 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg7, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg5, new List<Flight>()},
                { Leg.Leg6, new List<Flight>()},
                { Leg.Leg7, new List<Flight>()}
            };

            allActiveFlights[Leg.Leg5].Add(flight5);
            allActiveFlights[Leg.Leg6].Add(flight6);
            allActiveFlights[Leg.Leg7].Add(flight7);

            // Act  HandleLegThree
            //_FlightsLogic!.HandleLegFour(allActiveFlights);
            _FlightsLogic!.HandleLegFive(allActiveFlights);


            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg5) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg7) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg7].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg5].Count == 1);
        }

        [TestMethod]
        public void IfLegFiveTransfersTheFlight5ToLeg6OrLeg7()
        {
            //arrange
            var flight5 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg5, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg5, new List<Flight>()},
                { Leg.Leg6, new List<Flight>()},
            };

            allActiveFlights[Leg.Leg5].Add(flight5);
            allActiveFlights[Leg.Leg6].Add(flight6);

            // Act  HandleLegThree
            //_FlightsLogic!.HandleLegFour(allActiveFlights);
            _FlightsLogic!.HandleLegFive(allActiveFlights);


            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg5) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg5].Count == 0);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg7].Count == 1);
            
        }


        [TestMethod]
        public void IfHandleLegQueueHandleLeg6()
        {
            //arrange
            //var LegQueue = new Flight { FlightNumber = "ABC123", Leg = Leg.LegQueue, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };

            var LegQueue = new Flight { FlightNumber = "ABC123", Leg = Leg.LegQueue, IsArrival = false, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight6 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg6, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };
            var flight7 = new Flight { FlightNumber = "ABC123", Leg = Leg.Leg7, IsArrival = true, CreatedAt = DateTime.Today, Airline = "elal" };


            var allActiveFlights = new Dictionary<Leg, List<Flight>>
            {
                { Leg.Leg7, new List<Flight>()},
                { Leg.Leg6, new List<Flight>()},
                { Leg.LegQueue, new List<Flight>()},
            };

            allActiveFlights[Leg.Leg6].Add(flight6);
            allActiveFlights[Leg.Leg7].Add(flight7);
            allActiveFlights[Leg.LegQueue].Add(LegQueue);
            //allActiveFlights[Leg.LegQueue].Add(LegQueue);

            // Act  HandleLegThree
            //_FlightsLogic!.HandleLegFour(allActiveFlights);
            _FlightsLogic!.HandleLegQueue(allActiveFlights);


            // Assert
            // Ensure that the method returns without making any changes
            Assert.IsTrue(allActiveFlights.Count == 3);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg7) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.Leg6) == true);
            Assert.IsTrue(allActiveFlights.ContainsKey(Leg.LegQueue) == true);
            Assert.IsTrue(allActiveFlights[Leg.Leg7].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.Leg6].Count == 1);
            Assert.IsTrue(allActiveFlights[Leg.LegQueue].Count == 1);

        }
    }

}
        

    
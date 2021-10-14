using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsServices.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scropeFactory;
        private IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory,IMapper mapper)
        {
            _scropeFactory = scopeFactory;
            _mapper = mapper;
        }

        private EventType DetermineEvent(string notifcationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var evnetType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

            switch(evnetType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }

        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    // TO DO
                    break;
                default:
                    break;
            }
        }


        private void addPlatform(string platformPublishecMessage)
        {
            using( var scope = _scropeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var platformPlubishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishecMessage);

                try
                {
                    var plat = _mapper.Map<Platform>(platformPlubishedDto);
                    if (!repo.ExternalPlatformExists(plat.ExternalID))
                    {
                        repo.CreatePlatform(plat);
                        repo.SaveChange();
                    }
                    else
                    {
                        Console.WriteLine($"--> Platform already exisits...");
                    }
                }
                catch (Exception ex)
                {
                     Console.WriteLine($"--> Coulde not add Platform to DB {ex.Message}");
                }
            }
        }
    }

    public enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
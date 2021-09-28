using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[Controller]")]
    [ApiController]
    public class CommandsController:ControllerBase
    {
        private readonly ICommandRepo _respository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _respository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsFormPlatform: {platformId}");

            if (!_respository.PlaformExits(platformId))
            {
                return NotFound();
            }

            var commands = _respository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GEtCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandReadDto(int platformId ,int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} / {commandId}");

            if (!_respository.PlaformExits(platformId))
            {
                return NotFound();
            }

            var command = _respository.GetCommand(platformId,commandId);
            if (command == null)
            {
                return NotFound();
            }


            return Ok(_mapper.Map<CommandReadDto>(command));
        }


        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandReadDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");

            if (!_respository.PlaformExits(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _respository.CreateCommand(platformId,command);
            _respository.SaveChange();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandsForPlatform),new {poatformId = platformId, commandId = commandReadDto.Id},commandReadDto);
        }

    }
}
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEF.Config;
using WebApiEF.Models.Data;
using WebApiEF.Models.DTOs;
using WebApiEF.Repository;

namespace WebApiEF.Controllers
{
    [ApiController, Route("[controller]")]
    public class ArtikalController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ArtikalController> _logger;
        private readonly IMapper _mapper;

        public ArtikalController(IUnitOfWork unitOfWork, ILogger<ArtikalController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetArtikli()
        {
            try
            {
                var artikli = await Task.Run(() => _unitOfWork.Artikli.GetAll());
                var artikliDTO = _mapper.Map<IEnumerable<ArtikalDTO>>(artikli);

                return Ok(artikliDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetArtikli)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}", Name = "GetArtikal")]
        public async Task<IActionResult> GetArtikal(int id)
        {
            try
            {
                var artikal = await Task.Run(() => _unitOfWork.Artikli.GetSingleAsync(entity => entity.ArtikalId == id));
                var artikalDTO = _mapper.Map<ArtikalDTO>(artikal);

                return Ok(artikalDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetArtikal)}/{id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateArtikal([FromBody] CreateUpdateArtikalDTO artikalDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid CREATE attempt in {nameof(CreateArtikal)}");
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var artikal = _mapper.Map<Artikal>(artikalDTO);
                    await Task.Run(() => _unitOfWork.Artikli.Add(artikal));
                    _ = await Task.Run(() => _unitOfWork.Save());
                    return CreatedAtRoute("GetArtikal", new { id = artikal.ArtikalId }, artikal);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(CreateArtikal)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateArtikal(int id, [FromBody] CreateUpdateArtikalDTO artikalDTO)
        {
            if (id < 1 || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateArtikal)}");
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var artikal = await Task.Run(() => _unitOfWork.Artikli.GetSingleAsync(entity => entity.ArtikalId == id));

                    if (artikal == null)
                    {
                        _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateArtikal)}");
                        return BadRequest("Match not found");
                    }
                    else
                    {
                        _ = _mapper.Map(artikalDTO, artikal);
                        await Task.Run(() => _unitOfWork.Artikli.Update(artikal));
                        _ = await Task.Run(() => _unitOfWork.Save());

                        return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateArtikal)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteArtikal(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteArtikal)}");
                return BadRequest("Invalid ID");
            }
            else
            {
                try
                {
                    var artikal = await Task.Run(() => _unitOfWork.Artikli.GetSingleAsync(entity => entity.ArtikalId == id));
                    var relation = await Task.Run(() => _unitOfWork.RacunStavke.Exists(entity => entity.ArtikalId == id));

                    if (artikal == null)
                    {
                        _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteArtikal)}");
                        return BadRequest("Match not found");
                    }
                    else if (relation)
                    {
                        _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteArtikal)}");
                        return BadRequest("Relations detected");
                    }
                    else
                    {
                        await Task.Run(() => _unitOfWork.Artikli.Delete(artikal));
                        _ = await Task.Run(() => _unitOfWork.Save());

                        return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteArtikal)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}

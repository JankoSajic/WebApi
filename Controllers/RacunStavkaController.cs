using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEF.Models.Data;
using WebApiEF.Models.DTOs;
using WebApiEF.Repository;

namespace WebApiEF.Controllers
{
    [ApiController, Route("[controller]")]
    public class RacunStavkaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RacunStavkaController> _logger;
        private readonly IMapper _mapper;

        public RacunStavkaController(IUnitOfWork unitOfWork, ILogger<RacunStavkaController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRacunStavke()
        {
            try
            {
                var racunStavke = await Task.Run(() => _unitOfWork.RacunStavke.GetAll());
                var racunStavkeDTO = _mapper.Map<IEnumerable<RacunStavkaDTO>>(racunStavke);

                return Ok(racunStavkeDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRacunStavke)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRacunStavka(int id)
        {
            try
            {
                var racunStavka = await Task.Run(() => _unitOfWork.RacunStavke.GetSingleAsync(entity => entity.RacunStavkaId == id));
                var racunStavkaDTO = _mapper.Map<RacunStavkaDTO>(racunStavka);

                return Ok(racunStavkaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRacunStavka)}/{id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]

        public async Task<IActionResult> CreateRacunStavka(int idRacuna, PredRacunStavka predRacunStavka)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid CREATE attempt in {nameof(CreateRacunStavka)}");
                return BadRequest(ModelState);
            }

            try
            {
                var racunStavka = _mapper.Map<RacunStavka>(predRacunStavka);
                //var racun = _mapper.Map<Racun>(idRacuna);
                var racunAsync = _unitOfWork.Racuni.GetSingleAsync(x => x.RacunId == idRacuna);//Proveriti da li treba task da se postavi za GetSingle
                if (racunAsync == null)
                {
                    return BadRequest();
                }

                var racun = await racunAsync;

                if(racun == null)
                    return NotFound();

                racunStavka.RacunId = idRacuna;
                racun.Total += racunStavka.Kolicina * racunStavka.Cena;
                await Task.Run(() => _unitOfWork.RacunStavke.Add(racunStavka));
                await Task.Run(() => _unitOfWork.Racuni.Update(racun));
                _ = await Task.Run(() => _unitOfWork.Save());
                return Ok(predRacunStavka);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateRacunStavka)} in added Racun");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]

        public async Task<IActionResult> DeleteRacunStavka(int idRacunStavka)
        {
            if (idRacunStavka < 1 || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(DeleteRacunStavka)}");
                return BadRequest(ModelState);
            }
            try
            {
                var racunStavka = await Task.Run(() => _unitOfWork.RacunStavke.GetSingleAsync(x => x.RacunStavkaId == idRacunStavka));
                if (racunStavka == null)
                {
                    return BadRequest();
                }
                var racun = await Task.Run(() => _unitOfWork.Racuni.GetSingleAsync(x => x.RacunId == racunStavka.RacunId));
                if(racun == null)
                {
                    return BadRequest();
                }

                racun.Total -= racunStavka.Kolicina * racunStavka.Cena;
                await Task.Run(() => _unitOfWork.RacunStavke.Delete(racunStavka));
                if(racun.RacunStavkas.Count == 0)
                    await Task.Run(() => _unitOfWork.Racuni.Delete(racun));
                else
                    await Task.Run(() => _unitOfWork.Racuni.Update(racun));
                _ = await Task.Run(() => _unitOfWork.Save());
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteRacunStavka)}");
                return StatusCode(500, ex.Message);
            }
        }

        //Nije gotov

        [HttpPut]

        public async Task<IActionResult> UpdateRacunStavka(int idRacunStavka, [FromBody] PredRacunStavka predRacunStavka)
        {
            if (idRacunStavka < 1 || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRacunStavka)}");
                return BadRequest(ModelState);
            }
            try
            {
                var racunStavka = await Task.Run(() => _unitOfWork.RacunStavke.GetSingleAsync(x => x.RacunStavkaId == idRacunStavka));
                if (racunStavka == null)
                {
                    return BadRequest();
                }
                var racun = await Task.Run(() => _unitOfWork.Racuni.GetSingleAsync(x => x.RacunId == racunStavka.RacunId));
                if (racun == null)
                {
                    return BadRequest();
                }
                racun.Total -= racunStavka.Cena * racunStavka.Kolicina;
                _mapper.Map(predRacunStavka, racunStavka);
                await Task.Run(() => _unitOfWork.RacunStavke.Update(racunStavka));
                racun.Total += racunStavka.Cena * racunStavka.Kolicina;

                await Task.Run(() => _unitOfWork.Racuni.Update(racun));

                await Task.Run(() => _unitOfWork.Save());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateRacunStavka)}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}

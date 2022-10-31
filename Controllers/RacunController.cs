using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiEF.Models.Data;
using WebApiEF.Models.DTOs;
using WebApiEF.Repository;

namespace WebApiEF.Controllers
{
    [ApiController, Route("[controller]")]
    public class RacunController : ControllerBase
    {
        private readonly DbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RacunController> _logger;
        private readonly IMapper _mapper;
        private static int rBrDokumenta = 0;

        public RacunController(TestDbContext _testDbContext, IUnitOfWork unitOfWork, ILogger<RacunController> logger, IMapper mapper)
        {
            _dbContext = _testDbContext;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetRacuni()
        {
            try
            {
                var racuni = await _unitOfWork.Racuni.GetAsync(includes: new List<string> { "RacunStavkas" });
                var racuniDTO = _mapper.Map<IEnumerable<PredRacun>>(racuni);

                return Ok(racuniDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRacuni)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}", Name ="GetRacun")]
        public async Task<IActionResult> GetRacun(int id)
        {
            try
            {
                var racun = await _unitOfWork.Racuni.GetSingleAsync(entity => entity.RacunId == id, new List<string> { "RacunStavkas" });
                var racunDTO = _mapper.Map<PredRacun>(racun);

                return Ok(racunDTO);
                //return CreatedAtRoute("GetRacun", new { id = id }, racun);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetRacun)}/{id}");
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost]

        //public async Task<IActionResult> CreateRacun([FromBody] PredRacun racun)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogError($"Invalid CREATE attempt in {nameof(CreateRacun)}");
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        var racuns = _mapper.Map<Racun>(racun);

        //        //await Task.Run(() => _dbContext.AddRange(racuns));


        //        try
        //        {
        //            foreach (var item in racuns.RacunStavkas)
        //            {
        //                racuns.RacunStavkas.Add(_mapper.Map<RacunStavka>(item));
        //            }
        //            await Task.Run(() => _unitOfWork.Racuni.Add(racuns));
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"Something went wrong in the {nameof(CreateRacun)} in added RacunStavka");
        //            return StatusCode(500, ex.Message);
        //        }

        //        _ = await Task.Run(() => _unitOfWork.Save());

        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(CreateRacun)} in added Racun");
        //        return StatusCode(500, ex.Message);
        //    }

        //}

        //OVAJ POST RADI!!!
        //[HttpPost]

        //public async Task<IActionResult> CreateRacun([FromBody] PredRacun predRacun)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogError($"Invalid CREATE attempt in {nameof(CreateRacun)}");
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        Racun racun = new Racun();
        //        racun.KomitentId = predRacun.KomitentId;
        //        racun.BrojDokumenta = predRacun.BrojDokumenta;
        //        racun.Datum = DateTime.Now;
        //        await Task.Run(() => _unitOfWork.Racuni.Add(racun));
        //        racun.Total = 0;
        //        foreach (var item in predRacun.RacunStavkas)
        //        {
        //            racun.RacunStavkas.Add(vratiRacunStavku(item));
        //            racun.Total += item.Kolicina * item.Cena;
        //        }

        //        _ = await Task.Run(() => _unitOfWork.Save());

        //        return Ok();

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Something went wrong in the {nameof(CreateRacun)} in added Racun");
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //private RacunStavka vratiRacunStavku(PredRacunStavka predRacunStavka)
        //{
        //    var racunStavka = new RacunStavka();
        //    racunStavka.ArtikalId = predRacunStavka.ArtikalId;
        //    racunStavka.Kolicina = predRacunStavka.Kolicina;
        //    racunStavka.Cena = predRacunStavka.Cena;
        //    return racunStavka;
        //}

        //RADI!!!
        [HttpPost]

        public async Task<IActionResult> CreateRacun([FromBody] PredRacun predRacun)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid CREATE attempt in {nameof(CreateRacun)}");
                return BadRequest(ModelState);
            }

            try
            {

                var racun = _mapper.Map<Racun>(predRacun);

                await Task.Run(() => _unitOfWork.Racuni.Add(racun));
                racun.Total = 0;
                racun.BrojDokumenta = RedniBrojDokumenta() + "-" + racun.Datum.Month + "-" + racun.Datum.Year;
                foreach (var item in predRacun.RacunStavkas)
                {
                    //racun.RacunStavkas.Add(_mapper.Map<RacunStavka>(item));
                    racun.Total += item.Kolicina * item.Cena;
                }

                _ = await Task.Run(() => _unitOfWork.Save());

                return Ok();
                //var predpredRacun = _mapper.Map<PredRacun>(racun);
                //return CreatedAtRoute("GetRacun", new { id = racun.RacunId }, predRacun);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(CreateRacun)} in added Racun");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRacun(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRacun)}");
                return BadRequest("Invalid ID");
            }
            try
            {
                var racun = await Task.Run(() => _unitOfWork.Racuni.GetSingleAsync(entity => entity.RacunId == id));
                var racunStavke = await Task.Run(() => _unitOfWork.RacunStavke.GetAsync(entity => entity.RacunId == id));

                if (racun == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRacun)}");
                    return BadRequest("Match not found");
                }
                if (racunStavke == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteRacun)}");
                    return BadRequest("Match not found");
                }
                else
                {
                    await Task.Run(() => _unitOfWork.RacunStavke.DeleteRange(racunStavke));
                    await Task.Run(() => _unitOfWork.Racuni.Delete(racun));
                    _ = await Task.Run(() => _unitOfWork.Save());

                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteRacun)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]

        public async Task<IActionResult> UpdateRacun(int id, [FromBody] PredRacun predRacun)
        {
            if (id < 1 || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRacun)}");
                return BadRequest(ModelState);
            }
            try
            {
                var racun = await Task.Run(() => _unitOfWork.Racuni.GetSingleAsync(entity => entity.RacunId == id));
                var racunStavke = await Task.Run(() => _unitOfWork.RacunStavke.GetAsync(entity => entity.RacunId == id));
                //Treba uraditi proveru za komitenta ne dati da se salje i menja

                if (racun == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRacun)}");
                    return BadRequest("Match not found");
                }
                _mapper.Map(predRacun, racun);
                if (racunStavke == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateRacun)}");
                    return BadRequest("Match not found");
                }

                await Task.Run(() => _unitOfWork.RacunStavke.DeleteRange(racunStavke));
                racun.Total = 0;
                foreach (var item in predRacun.RacunStavkas)
                {
                    racun.Total += item.Kolicina * item.Cena;
                }

                await Task.Run(() => _unitOfWork.Racuni.Update(racun));
                await Task.Run(() => _unitOfWork.Save());
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateRacun)}");
                return StatusCode(500, ex.Message);
            }
        }

        private int RedniBrojDokumenta()
        {
            if (DateTime.Now.Day == 0)
                rBrDokumenta = 0;
            return rBrDokumenta++;
        }
    }
}

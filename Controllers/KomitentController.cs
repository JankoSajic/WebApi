using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiEF.Models.Data;
using WebApiEF.Models.DTOs;
using WebApiEF.Repository;

namespace WebApiEF.Controllers
{
    [ApiController, Route("[controller]")]
    public class KomitentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<KomitentController> _logger;
        private readonly IMapper _mapper;

        public KomitentController(IUnitOfWork unitOfWork, ILogger<KomitentController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetKomitenti()
        {
            try
            {
                var komitenti = await Task.Run(() => _unitOfWork.Komitenti.GetAll());
                var komitentiDTO = _mapper.Map<IEnumerable<KomitentDTO>>(komitenti);

                return Ok(komitentiDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetKomitenti)}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id:int}", Name = "GetKomitent")]
        public async Task<IActionResult> GetKomitent(int id)
        {
            try
            {
                var komitent = await Task.Run(() => _unitOfWork.Komitenti.GetSingleAsync(entity => entity.KomitentId == id));
                var komitentDTO = _mapper.Map<KomitentDTO>(komitent);

                return Ok(komitentDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetKomitent)}/{id}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateKomitent([FromBody] IEnumerable<CreateUpdateKomitentDTO> komitentiDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid CREATE attempt in {nameof(CreateKomitent)}");
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var komitenti = new List<Komitent>();

                    foreach (var item in komitentiDTO)
                        komitenti.Add(_mapper.Map<Komitent>(item));

                    await Task.Run(() => _unitOfWork.Komitenti.Add(komitenti));
                    _ = await Task.Run(() => _unitOfWork.Save());

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(CreateKomitent)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateKomitent(int id, [FromBody] CreateUpdateKomitentDTO komitentDTO)
        {
            if (id < 1 || !ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateKomitent)}");
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var komitent = await Task.Run(() => _unitOfWork.Komitenti.GetSingleAsync(entity => entity.KomitentId == id));

                    if (komitent == null)
                    {
                        _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateKomitent)}");
                        return BadRequest("Match not found");
                    }
                    else
                    {
                        _ = _mapper.Map(komitentDTO, komitent);
                        await Task.Run(() => _unitOfWork.Komitenti.Update(komitent));
                        _ = await Task.Run(() => _unitOfWork.Save());

                        return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(UpdateKomitent)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteKomitent(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteKomitent)}");
                return BadRequest("Invalid ID");
            }
            else
            {
                try
                {
                    var komitent = await Task.Run(() => _unitOfWork.Komitenti.GetSingleAsync(entity => entity.KomitentId == id));
                    var relation = await Task.Run(() => _unitOfWork.Racuni.Exists(entity => entity.KomitentId == id));

                    if (komitent == null)
                    {
                        _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteKomitent)}");
                        return BadRequest("Match not found");
                    }
                    else if (relation)
                    {
                        _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteKomitent)}");
                        return BadRequest("Relations detected");
                    }
                    else
                    {
                        await Task.Run(() => _unitOfWork.Komitenti.Delete(komitent));
                        _ = await Task.Run(() => _unitOfWork.Save());

                        return NoContent();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Something went wrong in the {nameof(DeleteKomitent)}");
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}

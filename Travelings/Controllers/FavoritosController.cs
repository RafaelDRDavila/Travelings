using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travelings.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travelings.Controllers
{
    [Table("favoritos")]
    public class Favorito
    {
        [Key]
        public int id { get; set; }
        public int idcliente { get; set; }
        public int idproduto { get; set; }
    }

    [ApiController]
    [Route("api/v1/favoritos")]
    public class FavoritosController : ControllerBase
    {
        private int? GetUserId()
        {
            var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)
                ?? User.FindFirst("sub");
            if (claim != null && int.TryParse(claim.Value, out var id)) return id;
            return null;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            using var ctx = new ConnectionContext();
            var items = ctx.Set<Favorito>().Where(f => f.idcliente == userId.Value).ToList();
            return Ok(items);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add([FromBody] FavoritoInput input)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            using var ctx = new ConnectionContext();

            var existing = ctx.Set<Favorito>()
                .FirstOrDefault(f => f.idcliente == userId.Value && f.idproduto == input.idproduto);
            if (existing != null)
                return Ok(new { id = existing.id });

            var fav = new Favorito { idcliente = userId.Value, idproduto = input.idproduto };
            ctx.Set<Favorito>().Add(fav);
            ctx.SaveChanges();
            return Created("", new { id = fav.id });
        }

        [HttpDelete("{idproduto}")]
        [Authorize]
        public IActionResult Remove(int idproduto)
        {
            var userId = GetUserId();
            if (userId == null) return Unauthorized();

            using var ctx = new ConnectionContext();
            var fav = ctx.Set<Favorito>()
                .FirstOrDefault(f => f.idcliente == userId.Value && f.idproduto == idproduto);
            if (fav == null) return NotFound();

            ctx.Set<Favorito>().Remove(fav);
            ctx.SaveChanges();
            return Ok();
        }
    }

    public class FavoritoInput
    {
        public int idproduto { get; set; }
    }
}

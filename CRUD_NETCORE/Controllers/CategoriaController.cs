using CRUD_NETCORE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace CRUD_NETCORE.Controllers
{
    [EnableCors("CorsRules")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CrudContext _context;

        public CategoriaController(CrudContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var categorias = _context.Categorias.Include(c => c.Productos).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categorias });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idCategoria:int}")]
        public IActionResult Obtener(int idCategoria)
        {
            try
            {
                var categoria = _context.Categorias.Include(c => c.Productos).SingleOrDefault(c => c.Idcategoria == idCategoria);

                if (categoria == null)
                {
                    return NotFound("No se encontró la categoría");
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = categoria });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Categoria objeto)
        {
            try
            {
                var categoria = _context.Categorias.Find(objeto.Idcategoria);

                if (categoria == null)
                {
                    return NotFound("No se encontró la categoría");
                }

                categoria.Descripcion = objeto.Descripcion ?? categoria.Descripcion;

                _context.Categorias.Update(categoria);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "¡Sí!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idCategoria:int}")]
        public IActionResult Eliminar(int idCategoria)
        {
            try
            {
                var categoria = _context.Categorias.Find(idCategoria);

                if (categoria == null)
                {
                    return NotFound("No se encontró la categoría");
                }

                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "¡Sí!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }
    }
}

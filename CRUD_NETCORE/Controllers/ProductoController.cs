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
    public class ProductoController : ControllerBase
    {
        private readonly CrudContext _context;

        public ProductoController(CrudContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            try
            {
                var productos = _context.Productos.Include(p => p.dCategoria).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = productos });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpGet]
        [Route("Obtener/{idProducto:int}")]
        public IActionResult Obtener(int idProducto)
        {
            try
            {
                var producto = _context.Productos.Include(p => p.dCategoria).SingleOrDefault(p => p.IdProducto == idProducto);

                if (producto == null)
                {
                    return NotFound("No se encontró el producto");
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            try
            {
                var producto = _context.Productos.Find(objeto.IdProducto);

                if (producto == null)
                {
                    return NotFound("No se encontró el producto");
                }

                producto.CodigoBarra = objeto.CodigoBarra ?? producto.CodigoBarra;
                producto.Descripcion = objeto.Descripcion ?? producto.Descripcion;
                producto.Marca = objeto.Marca ?? producto.Marca;
                producto.IdCategoria = objeto.IdCategoria ?? producto.IdCategoria;
                producto.Precio = objeto.Precio ?? producto.Precio;

                _context.Productos.Update(producto);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "¡Sí!" });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = e.Message });
            }
        }

        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                var producto = _context.Productos.Find(idProducto);

                if (producto == null)
                {
                    return NotFound("No se encontró el producto");
                }

                _context.Productos.Remove(producto);
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

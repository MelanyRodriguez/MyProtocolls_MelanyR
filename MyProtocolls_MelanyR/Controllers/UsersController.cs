using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProtocolls_MelanyR.Atributtes;
using MyProtocolls_MelanyR.Models;
using MyProtocolls_MelanyR.ModelsDTs;

namespace MyProtocolls_MelanyR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiKeyAtributte]
    public class UsersController : ControllerBase
    {
        private readonly MyProtocolsBDContext _context;

        public UsersController(MyProtocolsBDContext context)
        {
            _context = context;
        }

        //Este get valida el usario que quiere ingresar en la app
        // GET: api/Users
        [HttpGet("ValidarLogin")]
        public async Task<ActionResult<User>> ValidateLogin(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(e => e.Email.Equals(username) &&
                                                                 e.Password==password);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        // GET: api/Users
        [HttpGet("{GetUserInfoByEmail}")]
        public ActionResult<IEnumerable<UserDTO>> GetUserInfoByEmail(string Pemail)
        {
            //aca creamos un linq que convina informacion de dos entidades 
            //(user iner join userrole) y la agrega en el objeto de DTO user

            var query =(from u in _context.Users
                        join ur in _context.UserRoles on
                        u.UserRoleId equals ur.UserRoleId
                        where u.Email == Pemail && u.Active==true &&
                        u.IsBlocked==false
                        
                        select new
                        {
                            UsuarioId= u.UserId,
                            correo = u.Email,
                            Contrasenia=u.Password,
                            Nombre=u.Name,
                            RespaldoCorreo=u.BackUpEmail,
                            Telefono=u.PhoneNumber,
                            Direccion=u.Address,
                            Activo=u.Active,
                            EstaBloqueado=u.IsBlocked,
                            IDRolUsuario=ur.UserRoleId,
                            DescripcionRol = ur.Description
                        }).ToList();

            //creamos un objeto de tipo que retorna la funcion
            List<UserDTO> list = new List<UserDTO>();
            foreach (var item in query)
            {
                UserDTO NewItem = new UserDTO()
                { 
                UsuarioId = item.UsuarioId,
                Correo = item.correo,
                Contrasenia = item.Contrasenia,
                Nombre = item.Nombre,
                RespaldoCorreo = item.RespaldoCorreo,
                Telefono = item.Telefono,
                Direccion = item.Direccion,
                Activo = item.Activo,
                EstaBloqueado = item.EstaBloqueado,
                IDRolUsuario = item.IDRolUsuario,
                DescripcionRol = item.DescripcionRol
                
                 };
                
                list .Add(NewItem);
                

        }
            if (list == null) { return NotFound(); }
            return list;
        }


        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO user)
        {
            if (id != user.UsuarioId)
            {
                return BadRequest();
            }

            //tenemos que hacer la conversion entre el DTO que llega en formato json
            //(en el header) y el objeto que entity framework entiende que es de tipo

           User? newEFUser = new User();

            newEFUser= GetUserByID(id);

            if (newEFUser!=null)
            {
                newEFUser.Email = user.Correo;
                newEFUser.Name = user.Nombre;
                newEFUser.BackUpEmail = user.RespaldoCorreo;
                newEFUser.PhoneNumber = user.Telefono;
                newEFUser.Address = user.Direccion;

                _context.Entry(newEFUser).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'MyProtocolsBDContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }

        private User? GetUserByID(int id)
        {
            var user = _context.Users.Find(id);
            return user;
        }

    }
}




